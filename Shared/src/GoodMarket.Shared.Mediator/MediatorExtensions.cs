using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using GoodMarket.Shared.Mediator.Query;
using GoodMarket.Shared.Mediator.Command;

namespace GoodMarket.Shared.Mediator;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[]?  assemblies)
    {
        services.AddSingleton<IMediator, Mediator>();

        if (assemblies is not null)
        {
            foreach (var assembly in assemblies)
            {
                var handlerTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                    .Where(x =>
                        x.Interface.IsGenericType &&
                        (x.Interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                         x.Interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                    .ToList();

                foreach (var handler in handlerTypes)
                {
                    services.AddTransient(handler.Interface, handler.Type);
                }
            }
        }

        return services;
    }
}