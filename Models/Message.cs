namespace HW1.Models;

internal class Message
{
    public required string Username { get; init; }
    public required string Text { get; init; }
    public DateTime DateTime { get; } = DateTime.Now;


}
