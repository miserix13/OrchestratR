using Xunit;
using OrchestratR.Core;
using OrchestratR.Server;
using OrchestratR.ServerManager;
using OrchestratR.Extension.RabbitMq;
using OrchestratR.ServerManager.Domain;
using OrchestratR.ServerManager.Persistence;

namespace OrchestratR.Tests
{
    public class ProjectSmokeTests
    {
        [Fact]
        public void CanInstantiateCoreTypes()
        {
            Assert.NotNull(typeof(OrchestratR.Core.OrchestratedJobStatus));
        }

        [Fact]
        public void CanInstantiateServerTypes()
        {
            Assert.NotNull(typeof(OrchestratR.Server.JobArgument));
        }

        [Fact]
        public void CanInstantiateServerManagerTypes()
        {
            Assert.NotNull(typeof(OrchestratR.ServerManager.OrchestratorManagerService));
        }

        [Fact]
        public void CanInstantiateRabbitMqTypes()
        {
            Assert.NotNull(typeof(OrchestratR.Extension.RabbitMq.OrchestratorServerExtension));
        }

        [Fact]
        public void CanInstantiateDomainTypes()
        {
            Assert.NotNull(typeof(OrchestratR.ServerManager.Domain.Models.OrchestratedJob));
        }

        [Fact]
        public void CanInstantiatePersistenceTypes()
        {
            Assert.NotNull(typeof(OrchestratR.ServerManager.Persistence.OrchestratorDbContext));
        }
    }
}
