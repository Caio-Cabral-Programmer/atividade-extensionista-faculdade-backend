using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using FluentValidation;

namespace AtividadeExtensionistaFaculdadeBackend.Validators.Transactions;

public sealed class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Type).InclusiveBetween(1, 3).WithMessage("Tipo inválido: 1=Receita, 2=Despesa, 3=Transferência.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(x => x.Date).NotEmpty().WithMessage("A data é obrigatória.");
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("A conta de origem é obrigatória.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("A categoria é obrigatória.");
        RuleFor(x => x.Status).InclusiveBetween(1, 2).WithMessage("Status inválido: 1=Pendente, 2=Pago.");
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
        RuleFor(x => x.TotalInstallments)
            .GreaterThan(1).WithMessage("O número de parcelas deve ser maior que 1.")
            .When(x => x.IsInstallment);
        RuleFor(x => x.DestinationAccountId)
            .NotEmpty().WithMessage("A conta de destino é obrigatória para transferências.")
            .When(x => x.Type == 3);
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
        RuleFor(x => x.Status).InclusiveBetween(1, 2);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
    }
}
