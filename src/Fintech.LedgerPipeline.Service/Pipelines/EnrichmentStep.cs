using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public sealed class EnrichmentStep : ILedgerPipelineStep
{
    public LedgerEntry Execute(LedgerEntry entry)
    {
        entry.Metadata ??= [];
        entry.Metadata["enrichment"] = "enriched";
        return entry;
    }
}
