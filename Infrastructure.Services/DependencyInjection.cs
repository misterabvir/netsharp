using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Sockets;

namespace Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration,
        string[] args)
    {

        services.AddSingleton<ILog, Log>();
        services.AddSingleton<IUserInput, UserInput>();

        string host = configuration.GetSection("ServerEndPoint:Host").Value ?? throw new Exception();
        int port = int.Parse(configuration.GetSection("ServerEndPoint:Port")?.Value ?? throw new Exception());

        if (args.Length > 0)
        {
            services.AddSingleton<IMessageProvider>(_ => 
            new MessageProvider(new UdpClient(0)));
        }
        else
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(host), port);       
            services.AddSingleton<IMessageProvider>(_ => 
            new MessageProvider(new UdpClient(ep)));
        }
        return services;
    }
}
