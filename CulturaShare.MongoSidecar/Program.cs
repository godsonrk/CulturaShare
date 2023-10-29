using CulturaShare.MongoSidecar;
using CulturaShare.MongoSidecar.Application.Base;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ConfigurationApplicationService().SetupServiceProvider();

var app = serviceProvider.GetRequiredService<IApplication>();
await app.RunAsync();

