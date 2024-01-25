using Domain.Abstractions;

namespace Domain.Models;

public sealed class Message : Entity
{
    public MessageType MessageType { get; set; }
    public Guid? SenderId { get; set; }
    public Guid? RecipientId { get; set; }
    public Command? Command { get; set; }
    public Error? Error { get; set; }
    public string? Content { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;

    public Message() { }

    public static Message ErrorMessage(Error error)
    {
        return new Message()
        {
            MessageType = MessageType.Error,
            Error = error
        };
    }


    public static Message InfoMessage(string info)
    {
        return new Message()
        {
            MessageType = MessageType.Info,
            Content = info
        };
    }

    public static Message CommandMessage(Command command)
    {
        return new Message()
        {
            MessageType = MessageType.Command,
            Command = command
        };
    }

    public static Message CommonMessage(string content, Guid senderId, Guid? recepientId = null)
    {
        return new Message()
        {
            MessageType = MessageType.Message,
            Content = content,
            SenderId = senderId,
            RecipientId = recepientId
        };
    }
}