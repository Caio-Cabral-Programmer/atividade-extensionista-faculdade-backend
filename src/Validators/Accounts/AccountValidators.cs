using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using FluentValidation;

namespace AtividadeExtensionistaFaculdadeBackend.Validators.Accounts;

public sealed class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");
        RuleFor(x => x.Type).Must(t => Enum.TryParse<AccountType>(t, out _)).WithMessage("Tipo de conta inválido. Use: CheckingAccount, Savings, Cash ou Investment.");
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10).WithMessage("Cor inválida.");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50).WithMessage("Ícone inválido.");
    }
}

public sealed class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest>
{
    public UpdateAccountRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");
        RuleFor(x => x.Type).Must(t => Enum.TryParse<AccountType>(t, out _)).WithMessage("Tipo de conta inválido. Use: CheckingAccount, Savings, Cash ou Investment.");
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10).WithMessage("Cor inválida.");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50).WithMessage("Ícone inválido.");
    }
}

public sealed class CreateCreditCardRequestValidator : AbstractValidator<CreateCreditCardRequest>
{
    public CreateCreditCardRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreditLimit).GreaterThan(0).WithMessage("O limite deve ser maior que zero.");
        RuleFor(x => x.ClosingDay).InclusiveBetween(1, 31).WithMessage("Dia de fechamento inválido.");
        RuleFor(x => x.DueDay).InclusiveBetween(1, 31).WithMessage("Dia de vencimento inválido.");
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10);
    }
}
