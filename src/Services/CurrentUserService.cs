using System.Security.Claims;
using AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

namespace AtividadeExtensionistaFaculdadeBackend.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var claim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim is not null ? Guid.Parse(claim) : Guid.Empty;
        }
    }

    public string Email =>
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
}
