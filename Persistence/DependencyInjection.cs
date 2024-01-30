using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {                
        services.AddDbContext<ChatContext>(
            options => options
            .UseInMemoryDatabase(
                configuration
                .GetConnectionString("MemoryConnection") 
                ?? throw new InvalidOperationException("Not found any connection strings")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        return services; 
    }
}
