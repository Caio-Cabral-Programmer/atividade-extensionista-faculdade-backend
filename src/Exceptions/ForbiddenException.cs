namespace AtividadeExtensionistaFaculdadeBackend.Exceptions;

/// <summary>
/// Thrown when a user attempts to access or modify a resource that does not belong to them.
/// Translated to 403 Forbidden by the global exception handler.
/// </summary>
public sealed class ForbiddenException(string message) : Exception(message);
