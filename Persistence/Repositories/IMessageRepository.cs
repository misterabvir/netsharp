using Domain;

namespace Persistence.Repositories;

public interface IMessageRepository : IRepository<MessageEntity>
{
    Task CreateMessage(MessageEntity entity, CancellationToken cancellationToken);
    Task<IEnumerable<MessageEntity>> GetUnreadedMessages(Guid userId, DateTime last, CancellationToken cancellationToken);
}
