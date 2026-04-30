using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtividadeExtensionistaFaculdadeBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Financial");

            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmationToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EmailConfirmationTokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TwoFactorCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TwoFactorCodeExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PasswordResetTokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "Financial",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    InitialBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Financial",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Category_ParentCategory",
                        column: x => x.ParentCategoryId,
                        principalSchema: "Financial",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Category_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditCard",
                schema: "Financial",
                columns: table => new
                {
                    CreditCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ClosingDay = table.Column<int>(type: "int", nullable: false),
                    DueDay = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.CreditCardId);
                    table.ForeignKey(
                        name: "FK_CreditCard_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialGoal",
                schema: "Financial",
                columns: table => new
                {
                    FinancialGoalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialGoal", x => x.FinancialGoalId);
                    table.ForeignKey(
                        name: "FK_FinancialGoal_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "Financial",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Budget",
                schema: "Financial",
                columns: table => new
                {
                    BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    LimitAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.BudgetId);
                    table.ForeignKey(
                        name: "FK_Budget_Category",
                        column: x => x.CategoryId,
                        principalSchema: "Financial",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Budget_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecurringTransaction",
                schema: "Financial",
                columns: table => new
                {
                    RecurringTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransaction", x => x.RecurringTransactionId);
                    table.ForeignKey(
                        name: "FK_RecurringTransaction_Account",
                        column: x => x.AccountId,
                        principalSchema: "Financial",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransaction_Category",
                        column: x => x.CategoryId,
                        principalSchema: "Financial",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringTransaction_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "Financial",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreditCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsInstallment = table.Column<bool>(type: "bit", nullable: false),
                    InstallmentNumber = table.Column<int>(type: "int", nullable: true),
                    TotalInstallments = table.Column<int>(type: "int", nullable: true),
                    InstallmentGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecurringTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Account",
                        column: x => x.AccountId,
                        principalSchema: "Financial",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Category",
                        column: x => x.CategoryId,
                        principalSchema: "Financial",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_CreditCard",
                        column: x => x.CreditCardId,
                        principalSchema: "Financial",
                        principalTable: "CreditCard",
                        principalColumn: "CreditCardId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_DestinationAccount",
                        column: x => x.DestinationAccountId,
                        principalSchema: "Financial",
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_RecurringTransaction",
                        column: x => x.RecurringTransactionId,
                        principalSchema: "Financial",
                        principalTable: "RecurringTransaction",
                        principalColumn: "RecurringTransactionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transaction_User",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTag",
                schema: "Financial",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTag", x => new { x.TransactionId, x.TagId });
                    table.ForeignKey(
                        name: "FK_TransactionTag_Tag",
                        column: x => x.TagId,
                        principalSchema: "Financial",
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionTag_Transaction",
                        column: x => x.TransactionId,
                        principalSchema: "Financial",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                schema: "Financial",
                table: "Account",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Budget_CategoryId",
                schema: "Financial",
                table: "Budget",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ_Budget_UserCategoryYearMonth",
                schema: "Financial",
                table: "Budget",
                columns: new[] { "UserId", "CategoryId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                schema: "Financial",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UserId",
                schema: "Financial",
                table: "Category",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_UserId",
                schema: "Financial",
                table: "CreditCard",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoal_UserId",
                schema: "Financial",
                table: "FinancialGoal",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransaction_AccountId",
                schema: "Financial",
                table: "RecurringTransaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransaction_CategoryId",
                schema: "Financial",
                table: "RecurringTransaction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransaction_UserId",
                schema: "Financial",
                table: "RecurringTransaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                schema: "Financial",
                table: "Tag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                schema: "Financial",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CategoryId",
                schema: "Financial",
                table: "Transaction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreditCardId",
                schema: "Financial",
                table: "Transaction",
                column: "CreditCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestinationAccountId",
                schema: "Financial",
                table: "Transaction",
                column: "DestinationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_InstallmentGroupId",
                schema: "Financial",
                table: "Transaction",
                column: "InstallmentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RecurringTransactionId",
                schema: "Financial",
                table: "Transaction",
                column: "RecurringTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                schema: "Financial",
                table: "Transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserIdDate",
                schema: "Financial",
                table: "Transaction",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTag_TagId",
                schema: "Financial",
                table: "TransactionTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "UQ_User_Email",
                schema: "Identity",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budget",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "FinancialGoal",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "TransactionTag",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "CreditCard",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "RecurringTransaction",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "Account",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Financial");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Identity");
        }
    }
}
