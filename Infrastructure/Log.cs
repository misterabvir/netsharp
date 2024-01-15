using System.Runtime.CompilerServices;

namespace HW1.Infrastructure;

internal static class Log 
{
    public static void Information(string message)
    {
        Console.WriteLine(message);
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
