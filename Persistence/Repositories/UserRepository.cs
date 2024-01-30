using Domain;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ChatContext _context;

    public UserRepository(ChatContext context)
    {
        _context = context;
    }


    public async Task<UserEntity> GetOrCreateUserByName(string name, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = _context.Users.FirstOrDefault(x => x.Name.Equals(name.ToLower(), StringComparison.CurrentCultureIgnoreCase));
        if (userEntity is null)
        {
            userEntity = new() { Name = name, LastOnline = DateTime.Now };
            await _context.Users.AddAsync(userEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return userEntity;
    }

    public async Task UpdateUserLastOnline(Guid userId, CancellationToken cancellationToken)
    {
        var userEntity = _context.Users.FirstOrDefault(x => x.Id == userId);
        if (userEntity is not null)
        {
            userEntity.LastOnline = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
