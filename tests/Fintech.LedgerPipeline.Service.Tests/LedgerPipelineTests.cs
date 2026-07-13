using Fintech.LedgerPipeline.Service.Models;
using Fintech.LedgerPipeline.Service.Repositories;
using Fintech.LedgerPipeline.Service.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fintech.LedgerPipeline.Service.Tests;

[TestClass]
public class LedgerPipelineTests
{
    [TestMethod]
    public async Task Process_WithPipelineSteps_PersistsAndEnrichesEntry()
    {
        var repository = new InMemoryLedgerRepository();
        var steps = new List<ILedgerPipelineStep>
        {
            new ValidationStep(),
            new EnrichmentStep(),
            new RoutingStep()
        };

        var service = new LedgerProcessingService(NullLogger<LedgerProcessingService>.Instance, repository, steps);
        var entry = new LedgerEntry
        {
            AccountId = "acct-1001",
            Amount = 55.5m,
            Currency = "USD",
            SourceSystem = "core-banking"
        };

        var result = service.Process(entry);
        var persisted = await repository.GetAllAsync();

        Assert.AreEqual("validated", result.Metadata!["validation"]);
        Assert.AreEqual("enriched", result.Metadata["enrichment"]);
        Assert.AreEqual("routed", result.Metadata["routing"]);
        Assert.AreEqual(1, persisted.Count);
    }
}
