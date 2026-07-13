using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public class LedgerProcessingService
{
    public LedgerEntry Process(LedgerEntry entry)
    {
        // Placeholder for enrichment, validation, routing, etc.
        entry.Metadata ??= new Dictionary<string, string>();
        entry.Metadata["processed"] = "true";
        entry.Metadata["pipeline"] = "ledger-intake";

        return entry;
    }
}
