using HW1.Models;
using System.Text;
using System.Text.Json;

namespace HW1.Extensions;

internal static class MessageExtension
{
    public static string ToJson(this Message message)
        => JsonSerializer.Serialize(message);

    public static Message? FromJson(this string json)
        => JsonSerializer.Deserialize<Message?>(json);

    public static byte[] ToBytes(this Message message)
        => Encoding.UTF8.GetBytes(message.ToJson());

    public static Message? FromBytes(this byte[] bytes)
        => Encoding.UTF8.GetString(bytes).FromJson();

    public static void Print(this Message message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{message.NickName} - ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{message.DateTime.ToShortTimeString()} {message.DateTime.ToShortDateString()}");
        Console.ResetColor();
        Console.WriteLine($"{message.Text}");
    }
}
