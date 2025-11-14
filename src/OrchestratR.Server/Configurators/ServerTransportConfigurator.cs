using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.Server.Configurators
{
    internal class ServerTransportConfigurator : IServerTransportConfigurator
    {
        public ServerTransportConfigurator(string orchestratorServerName, int maxWorkersCount, IServiceCollection services)
        {
            OrchestratorServerName = orchestratorServerName;
            MaxWorkersCount = maxWorkersCount;
            Services = services;
        }

        public string OrchestratorServerName { get; }
        public int MaxWorkersCount { get; }
        public IServiceCollection Services { get; }
    }
}