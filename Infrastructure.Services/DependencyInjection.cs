using Core;
using Core.Abstraction.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;

namespace Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ChatMode mode)
    {
        ServerConfiguration.Host = configuration.GetSection("ServerEndPoint:Host").Value ?? throw new Exception("Not read server host address");
        ServerConfiguration.Port = int.Parse(configuration.GetSection("ServerEndPoint:Port")?.Value ?? throw new Exception("Not read server port"));

        services.AddSingleton<ILog, Log>();
        services.AddSingleton<IUserInput, UserInput>();
        services.AddSingleton(o => mode == ChatMode.Client ? new UdpClient(0) : new UdpClient(ServerConfiguration.ServerUrl));
        services.AddSingleton<IMessageProvider, MessageProvider>();
        
        return services;
    }
}
