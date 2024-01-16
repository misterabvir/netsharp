using App.Models;

namespace App.Infrastructure;

internal static class Log 
{
    public static void Information(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{DateTime.Now.ToShortTimeString()} {DateTime.Now.ToShortDateString()} - ");
        Console.ResetColor();
        Console.WriteLine(message);
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{DateTime.Now.ToShortTimeString()} {DateTime.Now.ToShortDateString()} - ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Message(Message message)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{message.Username} - ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{message.DateTime.ToShortTimeString()} {message.DateTime.ToShortDateString()}");

        Console.ForegroundColor = message.MessageStatus switch
        {
            MessageStatus.Error => ConsoleColor.Red,
            MessageStatus.System => ConsoleColor.Gray,
            _ => defaultColor
        };

        Console.WriteLine($": {message.Text}");
        Console.ResetColor();
    }
}
