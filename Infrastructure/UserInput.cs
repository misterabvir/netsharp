namespace HW1.Infrastructure;

internal class UserInput
{
    public static async Task<string> ConsoleInput(CancellationToken _cancellationToken)
    {

		try
		{
            _cancellationToken.ThrowIfCancellationRequested();
            return await Task.Run(() => Console.ReadLine() ?? string.Empty);
        }
		catch (OperationCanceledException)
		{

            Log.Error("Operation was cancelled");
            throw;
		}
        
    }
}
