using Microsoft.Extensions.DependencyInjection;

namespace OrchestratR.Core.Configurators
{
    public interface IServerTransportConfigurator
    {
        string OrchestratorServerName { get; }
        int MaxWorkersCount { get; }
        IServiceCollection Services { get; }
        // Add additional configuration properties/methods as needed for MassTransit 8+
    }
}