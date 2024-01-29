namespace Core.Abstraction.EventArgs;

public record ErrorExceptArgs(string Message, string? StackTrace);