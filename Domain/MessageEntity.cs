namespace Domain;

public class MessageEntity : Entity
{
    public required string Content { get; set; }
    public Guid SenderId { get; set; }
    public UserEntity? Sender { get; set; }
    public Guid? RecipientId { get; set; }
    public UserEntity? Recipient { get; set; }
    public DateTime Time { get; set; }
}
