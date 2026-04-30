using AtividadeExtensionistaFaculdadeBackend.DTOs.Accounts;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/credit-cards")]
[Authorize]
public sealed class CreditCardsController(ICreditCardService service, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(currentUser.UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, currentUser.UserId, ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCreditCardRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CreditCardId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCreditCardRequest request, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, request, currentUser.UserId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, currentUser.UserId, ct);
        return NoContent();
    }
}
