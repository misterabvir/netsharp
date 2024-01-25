using Core.Domain.Models;
using Domain.Abstractions;

namespace Core;

internal static class Log
{
    public static void Error(string error)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(DateTime.Now + " ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("ERROR: ");
        Console.ResetColor();
        Console.WriteLine(error);
    }

    public static void Message(string name, string content, DateTime? date = null)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write((date ?? DateTime.Now) + " ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(name + ": ");
        Console.ResetColor();
        Console.WriteLine(content);
    }

    public static void Info(string content)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(DateTime.Now + " ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("INFO: ");
        Console.ResetColor();
        Console.WriteLine(content);

    }

}
