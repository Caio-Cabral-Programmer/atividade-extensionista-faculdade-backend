using AtividadeExtensionistaFaculdadeBackend.DTOs.Budgets;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/budgets")]
[Authorize]
public sealed class BudgetsController(IBudgetService service, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? year, [FromQuery] int? month, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return Ok(await service.GetAllAsync(currentUser.UserId, year ?? now.Year, month ?? now.Month, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBudgetRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBudgetRequest request, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, request, currentUser.UserId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, currentUser.UserId, ct);
        return NoContent();
    }
}
