using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Services;

public class LedgerProcessingService : ILedgerProcessingService
{
    private readonly ILogger<LedgerProcessingService> _logger;

    public LedgerProcessingService(ILogger<LedgerProcessingService> logger)
    {
        _logger = logger;
    }

    public LedgerEntry Process(LedgerEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        if (string.IsNullOrWhiteSpace(entry.AccountId))
        {
            _logger.LogWarning("Ledger processing rejected entry with missing AccountId");
            throw new ArgumentException("AccountId is required.", nameof(entry));
        }

        if (string.IsNullOrWhiteSpace(entry.SourceSystem))
        {
            _logger.LogWarning("Ledger processing rejected entry with missing SourceSystem");
            throw new ArgumentException("SourceSystem is required.", nameof(entry));
        }

        if (entry.Amount <= 0)
        {
            _logger.LogWarning("Ledger processing rejected entry with invalid amount {Amount}", entry.Amount);
            throw new ArgumentException("Amount must be greater than zero.", nameof(entry));
        }

        if (string.IsNullOrWhiteSpace(entry.Currency))
        {
            entry.Currency = "USD";
        }

        entry.Metadata ??= [];
        entry.Metadata["processed"] = "true";
        entry.Metadata["pipeline"] = "ledger-intake";

        _logger.LogInformation("Ledger entry processed for account {AccountId} from {SourceSystem}", entry.AccountId, entry.SourceSystem);
        return entry;
    }
}
