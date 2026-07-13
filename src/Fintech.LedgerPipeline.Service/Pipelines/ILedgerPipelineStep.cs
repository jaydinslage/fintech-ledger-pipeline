using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public interface ILedgerPipelineStep
{
    LedgerEntry Execute(LedgerEntry entry);
}
