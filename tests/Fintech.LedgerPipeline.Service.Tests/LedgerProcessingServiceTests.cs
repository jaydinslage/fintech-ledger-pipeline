using Fintech.LedgerPipeline.Service.Models;
using Fintech.LedgerPipeline.Service.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fintech.LedgerPipeline.Service.Tests;

[TestClass]
public class LedgerProcessingServiceTests
{
    [TestMethod]
    public void Process_WithValidEntry_ReturnsProcessedEntry()
    {
        var service = new LedgerProcessingService(NullLogger<LedgerProcessingService>.Instance);
        var entry = new LedgerEntry
        {
            AccountId = "acct-1001",
            Amount = 125.5m,
            Currency = "USD",
            SourceSystem = "core-banking"
        };

        var result = service.Process(entry);

        Assert.IsNotNull(result);
        Assert.AreEqual("acct-1001", result.AccountId);
        Assert.AreEqual("USD", result.Currency);
        Assert.AreEqual("true", result.Metadata!["processed"]);
        Assert.AreEqual("ledger-intake", result.Metadata["pipeline"]);
    }

    [TestMethod]
    public void Process_WithMissingAccountId_ThrowsArgumentException()
    {
        var service = new LedgerProcessingService(NullLogger<LedgerProcessingService>.Instance);
        var entry = new LedgerEntry
        {
            AccountId = string.Empty,
            Amount = 10m,
            SourceSystem = "core-banking"
        };

        var ex = Assert.ThrowsException<ArgumentException>(() => service.Process(entry));
        StringAssert.Contains(ex.Message, "AccountId");
    }
}
