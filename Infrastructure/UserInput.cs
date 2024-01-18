using App.Models;

namespace App.Infrastructure;

/// <summary>
/// Wrap class for console input
/// </summary>
internal class UserInput
{
    /// <summary>
    /// Read user input
    /// </summary>
    /// <param name="cancellationToken"> Cancellation token </param>
    /// <returns> return user input string or exit command name </returns>
    public static async Task<string> ConsoleInput(CancellationToken cancellationToken)
    {
        return await Console.In.ReadLineAsync(cancellationToken) ?? Command.Exit.Name;
    }
}
