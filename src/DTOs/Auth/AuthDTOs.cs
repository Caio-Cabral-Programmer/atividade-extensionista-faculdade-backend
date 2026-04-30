namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Auth;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string Password);

public sealed record LoginRequest(
    string Email,
    string Password);

public sealed record VerifyTwoFactorRequest(
    string Email,
    string Code);

public sealed record ForgotPasswordRequest(
    string Email);

public sealed record ResetPasswordRequest(
    string Token,
    string NewPassword);

public sealed record AuthTokenResponse(
    string AccessToken,
    DateTime ExpiresAt);
