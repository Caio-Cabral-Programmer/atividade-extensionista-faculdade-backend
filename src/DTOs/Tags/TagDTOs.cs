namespace AtividadeExtensionistaFaculdadeBackend.DTOs.Tags;

public sealed record CreateTagRequest(string Name, string Color);
public sealed record UpdateTagRequest(string Name, string Color);
public sealed record TagResponse(Guid TagId, string Name, string Color);
