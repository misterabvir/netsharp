using App.Models;
using System.Text;
using System.Text.Json;

namespace App.Extensions;

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

    public static string FirstWord(this string text)
    {
        ReadOnlySpan<char> inputSpan = text.AsSpan();
        int spaceIndex = inputSpan.IndexOf(' ') == -1 ? text.Length : inputSpan.IndexOf(' ');
        return text[..spaceIndex];
    }
}
