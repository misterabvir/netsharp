using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ChatContext _context;
    public MessageRepository(ChatContext context)
    {
        _context = context;
    }


    public async Task CreateMessage(MessageEntity entity, CancellationToken cancellationToken)
    {
        _context.Messages.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<MessageEntity>> GetUnreadedMessages(Guid userId, DateTime last, CancellationToken cancellationToken)
    {
        return await _context
            .Messages
            .Where(m =>
            (m.RecipientId == null || m.RecipientId == userId) && m.Time > last)
            .Include(x=>x.Sender)
            .Include(x=>x.Recipient)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
