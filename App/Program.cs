using Core;
using Core.Abstraction;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;



var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(path:"appsettings.json", optional: false, reloadOnChange: true)
    .Build();


IServiceCollection services = new ServiceCollection();

services
    .AddPersistence(configuration)
    .AddInfrastructureServices(configuration, args)
    .AddCore(configuration, args);

await services.BuildServiceProvider().GetRequiredService<ChatBase>().StartAsync();