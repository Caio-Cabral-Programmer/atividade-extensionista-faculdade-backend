using AtividadeExtensionistaFaculdadeBackend.DTOs.Goals;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/financial-goals")]
[Authorize]
public sealed class FinancialGoalsController(IFinancialGoalService service, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(currentUser.UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, currentUser.UserId, ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGoalRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.FinancialGoalId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGoalRequest request, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, request, currentUser.UserId, ct));

    [HttpPatch("{id:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] GoalDepositWithdrawRequest request, CancellationToken ct) =>
        Ok(await service.DepositAsync(id, request, currentUser.UserId, ct));

    [HttpPatch("{id:guid}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] GoalDepositWithdrawRequest request, CancellationToken ct) =>
        Ok(await service.WithdrawAsync(id, request, currentUser.UserId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, currentUser.UserId, ct);
        return NoContent();
    }
}
