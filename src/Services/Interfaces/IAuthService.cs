using AtividadeExtensionistaFaculdadeBackend.DTOs.Auth;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task ConfirmEmailAsync(string token, CancellationToken ct);
    Task InitiateLoginAsync(LoginRequest request, CancellationToken ct);
    Task<AuthTokenResponse> VerifyTwoFactorAsync(VerifyTwoFactorRequest request, CancellationToken ct);
    Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct);
    Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct);
}
