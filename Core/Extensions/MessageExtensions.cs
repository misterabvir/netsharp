using Domain.Models;
using System.Text;
using System.Text.Json;


namespace Core.Extensions;

internal static class MessageExtensions
{
    public static byte[] ToBytes(this Message message) => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
    public static Message? ToMessage(this byte[] bytes) => JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(bytes));

}
