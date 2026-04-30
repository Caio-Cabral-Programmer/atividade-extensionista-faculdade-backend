using System.Text;
using AtividadeExtensionistaFaculdadeBackend.Middlewares;
using AtividadeExtensionistaFaculdadeBackend.Repositories;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Formatting.Compact;

// ─────────────────────────────────────────────────────────────
// Serilog bootstrap (reads from appsettings)
// ─────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ── Serilog ───────────────────────────────────────────────
    builder.Host.UseSerilog((context, services, config) =>
        config
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .WriteTo.Console(new CompactJsonFormatter())
            .WriteTo.File(
                new CompactJsonFormatter(),
                path: "logs/log-.json",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30));

    // ── Database ──────────────────────────────────────────────
    // Build connection string from individual environment variables.
    // Supports MSSQL_SERVER, MSSQL_DATABASE, MSSQL_USERID, MSSQL_PASSWORD.
    var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    var connectionString = rawConnectionString.Contains('%')
        ? BuildConnectionStringFromEnv()
        : rawConnectionString;

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    // ── JWT Authentication ────────────────────────────────────
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var rawJwtSecretKey = jwtSettings["SecretKey"]
        ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
    var secretKey = ResolveEnvVar(rawJwtSecretKey);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();

    // ── CORS ──────────────────────────────────────────────────
    var allowedOrigins = builder.Configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? [];

    builder.Services.AddCors(options =>
        options.AddPolicy("FrontendPolicy", policy =>
            policy
                .WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

    // ── FluentValidation ──────────────────────────────────────
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    // ── Swagger / OpenAPI ─────────────────────────────────────
    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, context, ct) =>
        {
            document.Info.Title = "My Smart Money API";
            document.Info.Version = "v1";
            document.Info.Description = "API de controle financeiro pessoal para a comunidade.";
            return Task.CompletedTask;
        });
    });

    // ── Controllers ───────────────────────────────────────────
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();

    // ── Global Exception Handler ──────────────────────────────
    builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
    builder.Services.AddProblemDetails();

    // ── Application Services ──────────────────────────────────
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    // Auth
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<JwtTokenService>();
    builder.Services.AddScoped<EmailService>();
    builder.Services.AddScoped<CategorySeederService>();

    // Accounts
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IAccountService, AccountService>();

    // Credit Cards
    builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>();
    builder.Services.AddScoped<ICreditCardService, CreditCardService>();

    // Categories
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();

    // Tags
    builder.Services.AddScoped<ITagRepository, TagRepository>();
    builder.Services.AddScoped<ITagService, TagService>();

    // Transactions
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();

    // Recurring Transactions
    builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
    builder.Services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();

    // Budgets
    builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
    builder.Services.AddScoped<IBudgetService, BudgetService>();

    // Financial Goals
    builder.Services.AddScoped<IFinancialGoalRepository, FinancialGoalRepository>();
    builder.Services.AddScoped<IFinancialGoalService, FinancialGoalService>();

    // Dashboard
    builder.Services.AddScoped<IDashboardService, DashboardService>();

    // ─────────────────────────────────────────────────────────
    // Build & Pipeline
    // ─────────────────────────────────────────────────────────
    var app = builder.Build();

    // 1. Exception Handler (must be first)
    app.UseExceptionHandler();

    // 2. HTTPS Redirection
    app.UseHttpsRedirection();

    // 3. Routing
    app.UseRouting();

    // 4. CORS
    app.UseCors("FrontendPolicy");

    // 5. Authentication
    app.UseAuthentication();

    // 6. Authorization
    app.UseAuthorization();

    // 7. Prometheus metrics
    app.UseHttpMetrics();

    // 8. OpenAPI spec + Scalar UI (all environments for dev convenience)
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "My Smart Money API";
        options.Theme = ScalarTheme.DeepSpace;
    });

    // 9. Map controllers & metrics endpoint
    app.MapControllers();
    app.MapMetrics();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}

static string ResolveEnvVar(string value)
{
    if (!value.StartsWith('%') || !value.EndsWith('%') || value.Length < 3)
        return value;

    var varName = value[1..^1];
    return Environment.GetEnvironmentVariable(varName) ?? value;
}

static string BuildConnectionStringFromEnv()
{
    var server = Environment.GetEnvironmentVariable("MSSQL_SERVER") ?? "localhost";
    var database = Environment.GetEnvironmentVariable("MSSQL_DATABASE") ?? "MySmartMoney_DB";
    var userId = Environment.GetEnvironmentVariable("MSSQL_USERID") ?? string.Empty;
    var password = Environment.GetEnvironmentVariable("MSSQL_PASSWORD") ?? string.Empty;
    return $"Server={server};Database={database};User Id={userId};Password={password};TrustServerCertificate=True;";
}
