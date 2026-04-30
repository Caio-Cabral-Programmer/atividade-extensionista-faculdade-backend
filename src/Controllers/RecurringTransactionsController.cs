using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/recurring-transactions")]
[Authorize]
public sealed class RecurringTransactionsController(
    IRecurringTransactionService service,
    ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(currentUser.UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, currentUser.UserId, ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecurringTransactionRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.RecurringTransactionId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecurringTransactionRequest request, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, request, currentUser.UserId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, currentUser.UserId, ct);
        return NoContent();
    }
}
