

using Core.Abstraction.Services;

namespace Infrastructure.Services;

public class UserInput : IUserInput
{
    public async Task<string?> ReadLineAsync()
    {
        return await Console.In.ReadLineAsync();
    }
}
