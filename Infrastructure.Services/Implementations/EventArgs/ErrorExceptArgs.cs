namespace Infrastructure.Services.Implementations.EventArgs;

public record ErrorExceptArgs(string Message, string? StackTrace);