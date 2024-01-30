namespace Infrastructure.Services.Abstractions;

public interface IUserInput
{
    Task<string?> ReadLineAsync();
}
