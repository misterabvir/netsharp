using Domain;
using System.Text;
using System.Text.Json;

namespace Contracts.Shared;

public partial class Message
{
    public string Content { get; set; } = string.Empty;
    public User Sender { get; set; } = null!;
    public User? Recipient { get; set; }
    public Command Command { get; set; } = Command.None;
    public DateTime Time { get; set; } = DateTime.Now;
    public IEnumerable<User> UsersList { get; set; } = [];


    public static Message FromDomain(MessageEntity message) => new() { Command = Command.None, Sender = User.FromDomain(message.Sender)!, Recipient = User.FromDomain(message.Recipient), Content = message.Content, Time = message.Time };

    public static Message Join(User user) => new() { Command = Command.Join, Sender = user };
    public static Message Join(User user, User recipient) => new() { Command = Command.Join, Sender = user, Recipient = recipient };
    public static Message Leave(User user) => new() { Command = Command.Exit, Sender = user };
    public static Message Users(User user, IEnumerable<User> usersList) => new() { Command = Command.Users, Sender = user, UsersList = usersList };
    public static Message History(User user) => new() { Command = Command.History, Sender = user };
    public static Message Common(User sender, User? recipient, string content) => new() { Command = Command.None, Sender = sender, Recipient = recipient, Content = content };


    public byte[] ToBytes() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this));

    public static Message? FromBytes(byte[] bytes) => JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(bytes));

    public MessageEntity ToDomain() => new() {  
        SenderId = Sender.Id,
        RecipientId = Recipient?.Id,  
        Content = Content, Time = Time };
}