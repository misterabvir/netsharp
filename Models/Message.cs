namespace HW1.Models;

internal class Message
{
    public required string NickName { get; init; }
    public required string Text { get; init; }
    public DateTime DateTime { get; } = DateTime.Now;


}
