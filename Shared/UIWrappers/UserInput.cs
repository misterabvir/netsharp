namespace Core.UIWrappers;

public class UserInput : IUserInput
{
    public async Task<string?> ReadLineAsync()
    {
        return await Console.In.ReadLineAsync();
    }
}
