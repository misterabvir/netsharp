using Core.Abstraction;
using Core.Implementation;
using Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using System.Net;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration,
        string[] args)
    {

        string host = configuration.GetSection("ServerEndPoint:Host").Value ?? throw new Exception();
        int port = int.Parse(configuration.GetSection("ServerEndPoint:Port")?.Value ?? throw new Exception());

        if (args.Length != 0)
        {
            services.AddSingleton<ChatBase>(o =>
            {
                return new ClientChat(args.First(),
                    o.GetRequiredService<IMessageProvider>(),
                    new IPEndPoint(IPAddress.Parse(host), port),
                    o.GetRequiredService<ILog>(),
                    o.GetRequiredService<IUserInput>());
            });
        }
        else
        {
            services.AddSingleton<ChatBase>(o =>
                new ServerChat(
                    o.GetRequiredService<IMessageProvider>(),
                    o.GetRequiredService<ILog>(),
                    o.GetRequiredService<IUserRepository>(),
                    o.GetRequiredService<IMessageRepository>()));
        }
        return services;
    }
}
