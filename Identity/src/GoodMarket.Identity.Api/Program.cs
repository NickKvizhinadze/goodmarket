using GoodMarket.Identity.Api.Endpoints.Account;
using GoodMarket.Identity.Api.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext(configuration)
    .AddIdentity()
    .AddMediator()
    .AddValidators()
    .AddOpenApiCustom()
    .AddVersioning();

var app = builder.Build();

app.CreateApiVersioning()
    .UseScalar(app.Environment)
    .UseHttpsRedirection()
    .UseRouting()
    .UseIdentityServer();

app.MapIdentityEndpoints();

app.Run();