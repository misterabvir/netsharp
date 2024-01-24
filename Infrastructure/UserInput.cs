using App.Models;

namespace App.Infrastructure;

internal interface IInput
{
    Task<string> GetInputAsync(CancellationToken cancellationToken);
}


/// <summary>
/// Wrap class for console input
/// </summary>
internal class ConsoleInput: IInput
{
    /// <summary>
    /// Read input
    /// </summary>
    /// <param name="cancellationToken"> Cancellation token </param>
    /// <returns> return input string or exit command name </returns>
    public async Task<string> GetInputAsync(CancellationToken cancellationToken)
    {
        return await Console.In.ReadLineAsync(cancellationToken) ?? Command.Exit.Name;
    }
}
