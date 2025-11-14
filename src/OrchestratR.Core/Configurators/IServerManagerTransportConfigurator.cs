
using Microsoft.Extensions.DependencyInjection;

namespace OrchestratR.Core.Configurators
{
    public interface IServerManagerTransportConfigurator
    {
        IServiceCollection Services { get; }
        // Add additional configuration properties/methods as needed for MassTransit 8+
    }
}