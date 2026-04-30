using System.Security.Claims;

namespace AtividadeExtensionistaFaculdadeBackend.Services.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
}
