using App.Models;

namespace App.Infrastructure;

internal class UserInput
{
    public static async Task<string> ConsoleInput(CancellationToken cancellationToken)
    {
        return await Console.In.ReadLineAsync(cancellationToken) ?? Command.Exit.Name;
    }
}
