using Contracts.Shared;
using Core.Abstraction.Services;

namespace Infrastructure.Services;


public class Log : ILog
{
    public void Error(string message, string? stackTrace)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss "));
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Error: ");
        Console.ResetColor();
        Console.WriteLine(message, stackTrace);
    }


    public void Info(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss "));
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Info: ");
        Console.ResetColor();
        Console.WriteLine(text);
    }

    public void Info(Message message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss "));
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Info: ");
        Console.ResetColor();
        Console.WriteLine($"User {message.Sender.Name} sended message for {message.Recipient?.Name ?? "all"}");
    }

    public void Message(Message message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(message.Time.ToString("dd/MM/yyyy HH:mm:ss "));
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write($"{message.Sender.Name}: ");
        Console.ResetColor();
        Console.WriteLine(message.Content);
    }
}
