using AtividadeExtensionistaFaculdadeBackend.DTOs.Auth;
using FluentValidation;

namespace AtividadeExtensionistaFaculdadeBackend.Validators.Auth;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Informe um e-mail válido.")
            .MaximumLength(256).WithMessage("O e-mail deve ter no máximo 256 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");
    }
}

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Informe um e-mail válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}

public sealed class VerifyTwoFactorRequestValidator : AbstractValidator<VerifyTwoFactorRequest>
{
    public VerifyTwoFactorRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Informe um e-mail válido.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("O código é obrigatório.")
            .Length(6).WithMessage("O código deve ter 6 dígitos.")
            .Matches(@"^\d{6}$").WithMessage("O código deve conter apenas dígitos.");
    }
}

public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("O token é obrigatório.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("A nova senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
            .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");
    }
}
