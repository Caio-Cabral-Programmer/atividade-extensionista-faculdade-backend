namespace AtividadeExtensionistaFaculdadeBackend.Exceptions;

/// <summary>
/// Thrown when a business rule is violated. Translated to 400 Bad Request by the global exception handler.
/// </summary>
public sealed class BusinessRuleException(string message) : Exception(message);
