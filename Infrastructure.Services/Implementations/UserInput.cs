using Infrastructure.Services.Abstractions;

namespace Infrastructure.Services.Implementations;

public class UserInput : IUserInput
{
    public async Task<string?> ReadLineAsync()
    {
        return await Console.In.ReadLineAsync();
    }
}
