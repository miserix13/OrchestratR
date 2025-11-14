using System;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;
using OrchestratR.Core.Messages;
using OrchestratR.Extension.RabbitMq.Options;
using OrchestratR.Server;
using OrchestratR.Server.Consumers;
using OrchestratR.ServerManager.Consumers;

namespace OrchestratR.Extension.RabbitMq
{
    public static class OrchestratorServerExtension
    {
        public static void UseRabbitMqTransport(this IServerTransportConfigurator configurator, RabbitMqOptions options)
        {
            configurator.Services.AddSingleton<IOrchestratrObserverFaultRule>(
                new OrchestratrObserverFaultRule($"/{OrchestratorQueueConstants.OrchestratorJobs}", typeof(RabbitMqConnectionException)));

            configurator.Services.AddMassTransit(x =>
            {
                x.AddConsumer<JobConsumer>();
                x.AddConsumer<JobCancellationConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.Host, h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });

                    EndpointConvention.Map<IServerCreatedMessage>(
                        new Uri($"queue:{OrchestratorQueueConstants.Manager}"));

                    EndpointConvention.Map<IServerDeletedMessage>(
                        new Uri($"queue:{OrchestratorQueueConstants.Manager}"));

                    EndpointConvention.Map<IJobStatusMessage>(
                        new Uri($"queue:{OrchestratorQueueConstants.Manager}"));

                    // RoundRobin
                    cfg.ReceiveEndpoint(OrchestratorQueueConstants.OrchestratorJobs, e =>
                    {
                        e.Durable = true;
                        e.UseMessageRetry(retryConfig => retryConfig.Immediate(int.MaxValue));
                        e.PrefetchCount = configurator.MaxWorkersCount;
                        e.ConfigureConsumer<JobConsumer>(context);
                    });

                    // Fan-out
                    cfg.ReceiveEndpoint(OrchestratorQueueConstants.CancellationJobsPrefix + configurator.OrchestratorServerName + '_' + Guid.NewGuid(), e =>
                    {
                        e.PrefetchCount = 1;
                        e.AutoDelete = true;
                        e.ConfigureConsumer<JobCancellationConsumer>(context);
                    });
                });
            });
        }
        
        public static void UseRabbitMqTransport(this IServerManagerTransportConfigurator configurator, RabbitMqOptions options)
        {
            configurator.Services.AddMassTransit(x =>
            {
                x.AddConsumer<ServerConsumer>();
                x.AddConsumer<JobStatusConsumer>();
                x.AddConsumer<HeartBeatConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.Host, h =>
                    {
                        h.Username(options.UserName);
                        h.Password(options.Password);
                    });

                    EndpointConvention.Map<IStartJobMessage>(
                        new Uri($"queue:{OrchestratorQueueConstants.OrchestratorJobs}"));

                    cfg.ReceiveEndpoint(OrchestratorQueueConstants.Manager, e =>
                    {
                        e.UseMessageRetry(retryConfig => retryConfig.Interval(10, TimeSpan.FromSeconds(5)));
                        e.PrefetchCount = 1;
                        e.ConfigureConsumer<ServerConsumer>(context);
                        e.ConfigureConsumer<JobStatusConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(OrchestratorQueueConstants.HeartBeats, e =>
                    {
                        e.PrefetchCount = 1;
                        e.AutoDelete = true;
                        e.ConfigureConsumer<HeartBeatConsumer>(context);
                    });
                });
            });
        }
    }
}