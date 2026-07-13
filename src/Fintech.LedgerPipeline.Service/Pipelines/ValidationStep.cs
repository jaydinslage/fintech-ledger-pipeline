using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public sealed class ValidationStep : ILedgerPipelineStep
{
    public LedgerEntry Execute(LedgerEntry entry)
    {
        entry.Metadata ??= [];
        entry.Metadata["validation"] = "validated";
        return entry;
    }
}
