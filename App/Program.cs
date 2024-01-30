using Core;
using Core.Abstraction.Common;
using Core.Implementation;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

ClientChat.UserName = args.Length > 0 ? args[0] : ClientChat.UserName;
ChatMode mode = args.Length > 0 ? ChatMode.Client : ChatMode.Server;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(path:"appsettings.json", optional: false, reloadOnChange: true)
    .Build();


IServiceCollection services = new ServiceCollection();

services
    .AddPersistence(configuration)
    .AddInfrastructureServices(configuration, mode)
    .AddCore(mode);

await services.BuildServiceProvider().GetRequiredService<ChatBase>().StartAsync();