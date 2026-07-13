using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public class LedgerProcessingService : ILedgerProcessingService
{
    public LedgerEntry Process(LedgerEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        if (string.IsNullOrWhiteSpace(entry.AccountId))
        {
            throw new ArgumentException("AccountId is required.", nameof(entry));
        }

        if (string.IsNullOrWhiteSpace(entry.SourceSystem))
        {
            throw new ArgumentException("SourceSystem is required.", nameof(entry));
        }

        if (entry.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(entry));
        }

        if (string.IsNullOrWhiteSpace(entry.Currency))
        {
            entry.Currency = "USD";
        }

        entry.Metadata ??= [];
        entry.Metadata["processed"] = "true";
        entry.Metadata["pipeline"] = "ledger-intake";

        return entry;
    }
}
