using AtividadeExtensionistaFaculdadeBackend.DTOs.Transactions;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public sealed class TransactionsController(ITransactionService service, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] TransactionFilterRequest filter, CancellationToken ct) =>
        Ok(await service.GetAllAsync(currentUser.UserId, filter, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, currentUser.UserId, ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.TransactionId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTransactionRequest request, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, request, currentUser.UserId, ct));

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTransactionStatusRequest request, CancellationToken ct) =>
        Ok(await service.UpdateStatusAsync(id, request, currentUser.UserId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, currentUser.UserId, ct);
        return NoContent();
    }
}
