using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public sealed class RoutingStep : ILedgerPipelineStep
{
    public LedgerEntry Execute(LedgerEntry entry)
    {
        entry.Metadata ??= [];
        entry.Metadata["routing"] = "routed";
        return entry;
    }
}
