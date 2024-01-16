namespace App.Models;

internal class Message
{
    public required string Username { get; init; }
    public required string Text { get; init; }
    public DateTime DateTime { get; } = DateTime.Now;
    public MessageStatus MessageStatus { get; init; } = MessageStatus.Common;
}
