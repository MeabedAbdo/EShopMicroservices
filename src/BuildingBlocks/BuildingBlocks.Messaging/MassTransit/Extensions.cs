

using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services
            , IConfiguration _configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                if (assembly != null)
                    config.AddConsumers(assembly);
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(_configuration["MessageBroker:Host"], h =>
                    {
                        h.Username(_configuration["MessageBroker:Username"]!);
                        h.Password(_configuration["MessageBroker:Password"]!);
                    });
                    cfg.ConfigureEndpoints(context);
                    cfg.Message<BasketCheckoutEvent>(evt =>
                    {
                        evt.SetEntityName("basketcheckout-integrationevent");
                    });
                });
            });
            return services;
        }
    }
}
