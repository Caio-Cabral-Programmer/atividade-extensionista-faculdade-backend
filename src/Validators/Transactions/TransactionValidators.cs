using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using FluentValidation;

namespace AtividadeExtensionistaFaculdadeBackend.Validators.Transactions;

public sealed class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Type).Must(t => Enum.TryParse<TransactionType>(t, out _)).WithMessage("Tipo inválido. Use: Income, Expense ou Transfer.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(x => x.Date).NotEmpty().WithMessage("A data é obrigatória.");
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("A conta de origem é obrigatória.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("A categoria é obrigatória.");
        RuleFor(x => x.Status).Must(s => Enum.TryParse<TransactionStatus>(s, out _)).WithMessage("Status inválido. Use: Pending ou Paid.");
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
        RuleFor(x => x.TotalInstallments)
            .GreaterThan(1).WithMessage("O número de parcelas deve ser maior que 1.")
            .When(x => x.IsInstallment);
        RuleFor(x => x.DestinationAccountId)
            .NotEmpty().WithMessage("A conta de destino é obrigatória para transferências.")
            .When(x => x.Type == nameof(TransactionType.Transfer));
    }
}

public sealed class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Status).Must(s => Enum.TryParse<TransactionStatus>(s, out _)).WithMessage("Status inválido. Use: Pending ou Paid.");
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
    }
}
