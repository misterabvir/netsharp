namespace Core.Abstraction.Services.Events;

public record ErrorExceptArgs(string Message, string? StackTrace);