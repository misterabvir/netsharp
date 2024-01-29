namespace Core.UIWrappers;

public interface IUserInput
{
    Task<string?> ReadLineAsync();
}
