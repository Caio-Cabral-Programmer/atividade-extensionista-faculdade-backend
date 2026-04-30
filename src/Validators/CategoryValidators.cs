using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;
using FluentValidation;

namespace AtividadeExtensionistaFaculdadeBackend.Validators;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Type).InclusiveBetween(1, 2).WithMessage("Tipo inválido. Use 1 (Receita) ou 2 (Despesa).");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10);
    }
}
