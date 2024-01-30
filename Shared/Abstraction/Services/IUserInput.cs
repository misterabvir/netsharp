namespace Core.Abstraction.Services;

public interface IUserInput
{
    Task<string?> ReadLineAsync();
}
