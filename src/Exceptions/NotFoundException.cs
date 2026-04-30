namespace AtividadeExtensionistaFaculdadeBackend.Exceptions;

/// <summary>
/// Thrown when a requested resource is not found. Translated to 404 Not Found by the global exception handler.
/// </summary>
public sealed class NotFoundException(string message) : Exception(message);
