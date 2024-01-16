namespace HW1.Models;

internal class Message
{
    public required string Username { get; init; }
    public required string Text { get; init; }
    public DateTime DateTime { get; } = DateTime.Now;

    public static Message JoinedServerMessage(string username) =>
        new() { Text = $"{username} was joined to chat", Username="Server" };
    public static Message QuitServerMessage(string username) =>
       new() { Text = $"{username} was out from chat", Username = "Server" };

    public static Message JoinedClientMessage(string username) =>
        new() { Text = $"/join", Username = username };
    public static Message QuitClientMessage(string username) =>
    new() { Text = $"/exit", Username = username };

    public static Message Create (string username, string text)=>
        new() { Text = text, Username = username };
}
