using AtividadeExtensionistaFaculdadeBackend.DTOs.Auth;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    JwtTokenService jwtTokenService,
    EmailService emailService,
    CategorySeederService categorySeeder,
    IConfiguration configuration,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var emailLower = request.Email.ToLowerInvariant();

        if (await userRepository.EmailExistsAsync(emailLower, ct))
            throw new BusinessRuleException("Este e-mail já está em uso.");

        var confirmationToken = Guid.NewGuid().ToString("N");

        var user = new User
        {
            Name = request.Name,
            Email = emailLower,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsEmailConfirmed = false,
            EmailConfirmationToken = confirmationToken,
            EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        await userRepository.AddAsync(user, ct);

        await categorySeeder.SeedForUserAsync(user.UserId, ct);

        var frontendUrl = configuration["Email:FrontendBaseUrl"] ?? "http://localhost:5173";
        var confirmationLink = $"{frontendUrl}/confirm-email?token={confirmationToken}";

        await emailService.SendEmailConfirmationAsync(user.Email, user.Name, confirmationLink, ct);

        logger.LogInformation("User registered: {Email}", user.Email);
    }

    public async Task ConfirmEmailAsync(string token, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailConfirmationTokenAsync(token, ct)
            ?? throw new NotFoundException("Token de confirmação inválido ou já utilizado.");

        if (user.EmailConfirmationTokenExpiresAt < DateTime.UtcNow)
            throw new BusinessRuleException("O link de confirmação expirou. Solicite um novo cadastro.");

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        user.EmailConfirmationTokenExpiresAt = null;

        await userRepository.UpdateAsync(user, ct);

        logger.LogInformation("Email confirmed for user: {Email}", user.Email);
    }

    public async Task InitiateLoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), ct)
            ?? throw new BusinessRuleException("Credenciais inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new BusinessRuleException("Credenciais inválidas.");

        if (!user.IsEmailConfirmed)
            throw new BusinessRuleException("Confirme seu e-mail antes de fazer login.");

        var code = Random.Shared.Next(100000, 999999).ToString();

        user.TwoFactorCode = code;
        user.TwoFactorCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);

        await userRepository.UpdateAsync(user, ct);

        await emailService.SendTwoFactorCodeAsync(user.Email, user.Name, code, ct);

        logger.LogInformation("2FA code sent to user: {Email}", user.Email);
    }

    public async Task<AuthTokenResponse> VerifyTwoFactorAsync(VerifyTwoFactorRequest request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), ct)
            ?? throw new BusinessRuleException("Credenciais inválidas.");

        if (user.TwoFactorCode is null || user.TwoFactorCode != request.Code)
            throw new BusinessRuleException("Código inválido.");

        if (user.TwoFactorCodeExpiresAt < DateTime.UtcNow)
            throw new BusinessRuleException("O código expirou. Faça login novamente para receber um novo código.");

        user.TwoFactorCode = null;
        user.TwoFactorCodeExpiresAt = null;

        await userRepository.UpdateAsync(user, ct);

        var (token, expiresAt) = jwtTokenService.GenerateToken(user);

        logger.LogInformation("User logged in: {Email}", user.Email);

        return new AuthTokenResponse(token, expiresAt);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), ct);

        // NOTE: Always return OK to avoid email enumeration attacks (OWASP).
        if (user is null)
        {
            logger.LogInformation("Forgot password requested for non-existent email: {Email}", request.Email);
            return;
        }

        var resetToken = Guid.NewGuid().ToString("N");
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);

        await userRepository.UpdateAsync(user, ct);

        var frontendUrl = configuration["Email:FrontendBaseUrl"] ?? "http://localhost:5173";
        var resetLink = $"{frontendUrl}/reset-password?token={resetToken}";

        await emailService.SendPasswordResetAsync(user.Email, user.Name, resetLink, ct);

        logger.LogInformation("Password reset email sent to: {Email}", user.Email);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct)
    {
        var user = await userRepository.GetByPasswordResetTokenAsync(request.Token, ct)
            ?? throw new NotFoundException("Token de redefinição inválido ou já utilizado.");

        if (user.PasswordResetTokenExpiresAt < DateTime.UtcNow)
            throw new BusinessRuleException("O link de redefinição expirou. Solicite um novo.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiresAt = null;

        await userRepository.UpdateAsync(user, ct);

        logger.LogInformation("Password reset completed for user: {Email}", user.Email);
    }
}
