using AtividadeExtensionistaFaculdadeBackend.DTOs.Auth;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        await authService.RegisterAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, new { message = "Cadastro realizado! Verifique seu e-mail para confirmar a conta." });
    }

    [HttpGet("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, CancellationToken ct)
    {
        await authService.ConfirmEmailAsync(token, ct);
        return Ok(new { message = "E-mail confirmado com sucesso! Você já pode fazer login." });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        await authService.InitiateLoginAsync(request, ct);
        return Ok(new { message = "Código de verificação enviado para o seu e-mail." });
    }

    [HttpPost("verify-2fa")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] VerifyTwoFactorRequest request, CancellationToken ct)
    {
        var token = await authService.VerifyTwoFactorAsync(request, ct);
        return Ok(token);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        await authService.ForgotPasswordAsync(request, ct);
        return Ok(new { message = "Se este e-mail estiver cadastrado, você receberá um link para redefinir sua senha." });
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        await authService.ResetPasswordAsync(request, ct);
        return Ok(new { message = "Senha redefinida com sucesso! Você já pode fazer login." });
    }
}
