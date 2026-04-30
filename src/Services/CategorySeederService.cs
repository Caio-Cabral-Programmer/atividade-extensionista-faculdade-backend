using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Entities.Enums;
using AtividadeExtensionistaFaculdadeBackend.Repositories;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class CategorySeederService(AppDbContext db)
{
    private static readonly (string Name, CategoryType Type, string Icon, string Color, string[] SubCategories)[] DefaultCategories =
    [
        ("Moradia",      CategoryType.Expense, "home",         "#4A90D9", ["Aluguel", "Condomínio", "Água", "Luz", "Internet", "Gás"]),
        ("Alimentação",  CategoryType.Expense, "shopping-cart","#E67E22", ["Supermercado", "Restaurante", "Delivery", "Padaria"]),
        ("Transporte",   CategoryType.Expense, "car",          "#27AE60", ["Combustível", "Ônibus/Metrô", "Uber/Táxi", "Manutenção"]),
        ("Saúde",        CategoryType.Expense, "heart",        "#E74C3C", ["Plano de Saúde", "Farmácia", "Consulta", "Academia"]),
        ("Educação",     CategoryType.Expense, "book",         "#9B59B6", ["Mensalidade", "Cursos", "Material Escolar"]),
        ("Lazer",        CategoryType.Expense, "smile",        "#F39C12", ["Cinema", "Viagem", "Restaurante", "Assinatura"]),
        ("Roupas",       CategoryType.Expense, "shirt",        "#1ABC9C", ["Vestuário", "Calçados", "Acessórios"]),
        ("Outros",       CategoryType.Expense, "more-horizontal","#95A5A6", []),
        ("Salário",      CategoryType.Income,  "briefcase",    "#2ECC71", []),
        ("Freelance",    CategoryType.Income,  "laptop",       "#3498DB", []),
        ("Investimentos",CategoryType.Income,  "trending-up",  "#F1C40F", []),
        ("Outros",       CategoryType.Income,  "plus-circle",  "#95A5A6", [])
    ];

    public async Task SeedForUserAsync(Guid userId, CancellationToken ct)
    {
        var categories = new List<Category>();

        foreach (var (name, type, icon, color, subCategories) in DefaultCategories)
        {
            var parent = new Category
            {
                UserId = userId,
                Name = name,
                Type = type,
                Icon = icon,
                Color = color
            };

            categories.Add(parent);

            foreach (var subName in subCategories)
            {
                categories.Add(new Category
                {
                    UserId = userId,
                    Name = subName,
                    Type = type,
                    Icon = icon,
                    Color = color,
                    ParentCategoryId = parent.CategoryId
                });
            }
        }

        await db.Categories.AddRangeAsync(categories, ct);
        await db.SaveChangesAsync(ct);
    }
}
