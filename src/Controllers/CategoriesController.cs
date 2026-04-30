using AtividadeExtensionistaFaculdadeBackend.DTOs.Categories;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtividadeExtensionistaFaculdadeBackend.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public sealed class CategoriesController(ICategoryService service, ICurrentUserService currentUser) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(currentUser.UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        Ok(await service.GetByIdAsync(id, currentUser.UserId, ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, currentUser.UserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CategoryId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct) =>
        Ok(await service.UpdateAsync(id, request, currentUser.UserId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, currentUser.UserId, ct);
        return NoContent();
    }
}
