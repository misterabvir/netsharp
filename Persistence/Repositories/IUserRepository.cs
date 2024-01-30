using Domain;

namespace Persistence.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity> GetOrCreateUserByName(string name, CancellationToken cancellationToken);
    Task UpdateUserLastOnline(Guid userId, CancellationToken cancellationToken);
}
