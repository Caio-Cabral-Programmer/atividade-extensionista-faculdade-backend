using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;
using AtividadeExtensionistaFaculdadeBackend.Entities;
using AtividadeExtensionistaFaculdadeBackend.Exceptions;
using AtividadeExtensionistaFaculdadeBackend.Extensions;
using AtividadeExtensionistaFaculdadeBackend.Repositories.Interfaces;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class CreditCardService(ICreditCardRepository repository) : ICreditCardService
{
    public async Task<List<CreditCardResponse>> GetAllAsync(Guid userId, CancellationToken ct)
    {
        var cards = await repository.GetAllByUserAsync(userId, ct);
        var responses = new List<CreditCardResponse>();

        foreach (var card in cards)
        {
            var invoiceTotal = await repository.GetCurrentInvoiceTotalAsync(card.CreditCardId, ct);
            responses.Add(card.ToResponse(invoiceTotal));
        }

        return responses;
    }

    public async Task<CreditCardResponse> GetByIdAsync(Guid creditCardId, Guid userId, CancellationToken ct)
    {
        var card = await repository.GetByIdAndUserAsync(creditCardId, userId, ct)
            ?? throw new NotFoundException("Cartão de crédito não encontrado.");

        var invoiceTotal = await repository.GetCurrentInvoiceTotalAsync(card.CreditCardId, ct);
        return card.ToResponse(invoiceTotal);
    }

    public async Task<CreditCardResponse> CreateAsync(CreateCreditCardRequest request, Guid userId, CancellationToken ct)
    {
        var card = new CreditCard
        {
            UserId = userId,
            Name = request.Name,
            CreditLimit = request.CreditLimit,
            ClosingDay = request.ClosingDay,
            DueDay = request.DueDay,
            Color = request.Color
        };

        await repository.AddAsync(card, ct);
        return card.ToResponse(0);
    }

    public async Task<CreditCardResponse> UpdateAsync(Guid creditCardId, UpdateCreditCardRequest request, Guid userId, CancellationToken ct)
    {
        var card = await repository.GetByIdAndUserAsync(creditCardId, userId, ct)
            ?? throw new NotFoundException("Cartão de crédito não encontrado.");

        card.Name = request.Name;
        card.CreditLimit = request.CreditLimit;
        card.ClosingDay = request.ClosingDay;
        card.DueDay = request.DueDay;
        card.Color = request.Color;
        card.IsActive = request.IsActive;

        await repository.UpdateAsync(card, ct);

        var invoiceTotal = await repository.GetCurrentInvoiceTotalAsync(card.CreditCardId, ct);
        return card.ToResponse(invoiceTotal);
    }

    public async Task DeleteAsync(Guid creditCardId, Guid userId, CancellationToken ct)
    {
        var card = await repository.GetByIdAndUserAsync(creditCardId, userId, ct)
            ?? throw new NotFoundException("Cartão de crédito não encontrado.");

        await repository.DeleteAsync(card, ct);
    }
}
