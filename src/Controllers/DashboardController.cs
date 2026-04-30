using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public sealed class DashboardController(IDashboardService service, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct) =>
        Ok(await service.GetSummaryAsync(currentUser.UserId, ct));

    [HttpGet("charts/expenses-by-category")]
    public async Task<IActionResult> GetExpensesByCategory(
        [FromQuery] int? year, [FromQuery] int? month, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return Ok(await service.GetExpensesByCategoryAsync(currentUser.UserId, year ?? now.Year, month ?? now.Month, ct));
    }

    [HttpGet("charts/balance-evolution")]
    public async Task<IActionResult> GetBalanceEvolution([FromQuery] int? year, CancellationToken ct) =>
        Ok(await service.GetBalanceEvolutionAsync(currentUser.UserId, year ?? DateTime.UtcNow.Year, ct));

    [HttpGet("balance-projection")]
    public async Task<IActionResult> GetBalanceProjection(CancellationToken ct) =>
        Ok(await service.GetBalanceProjectionAsync(currentUser.UserId, ct));
}
