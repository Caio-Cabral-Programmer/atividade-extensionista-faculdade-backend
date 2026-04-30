using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using FluentValidation;

namespace AtividadeExtensionistaFaculdadeBackend.Validators;

public sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Type).Must(t => Enum.TryParse<CategoryType>(t, out _)).WithMessage("Tipo inválido. Use: Income ou Expense.");
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Color).NotEmpty().MaximumLength(10);
    }
}
