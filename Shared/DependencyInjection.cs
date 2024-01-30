using Core.Abstraction.Common;
using Core.Implementation;
using Microsoft.Extensions.DependencyInjection;
namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services, ChatMode mode)
    {

        switch (mode)
        {
            case ChatMode.Client: services.AddSingleton<ChatBase, ClientChat>(); break;
            default: services.AddSingleton<ChatBase, ServerChat>(); break;
        }
        return services;
    }
}
