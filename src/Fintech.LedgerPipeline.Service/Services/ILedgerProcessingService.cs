using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public interface ILedgerProcessingService
{
    LedgerEntry Process(LedgerEntry entry);
}
