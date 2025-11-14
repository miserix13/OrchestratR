using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Configurators
{
    internal class ServerManagerTransportConfigurator : IServerManagerTransportConfigurator
    {
        public ServerManagerTransportConfigurator(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            Services = services;
        }

        public Microsoft.Extensions.DependencyInjection.IServiceCollection Services { get; }
    }
}