using Fintech.LedgerPipeline.Service.Events;
using Fintech.LedgerPipeline.Service.Models;
using Fintech.LedgerPipeline.Service.Repositories;

namespace Fintech.LedgerPipeline.Service.Services;

public class LedgerProcessingService : ILedgerProcessingService
{
    private readonly ILogger<LedgerProcessingService> _logger;
    private readonly ILedgerRepository _repository;
    private readonly IReadOnlyList<ILedgerPipelineStep> _pipelineSteps;

    public LedgerProcessingService(
        ILogger<LedgerProcessingService> logger,
        ILedgerRepository? repository = null,
        IReadOnlyList<ILedgerPipelineStep>? pipelineSteps = null)
    {
        _logger = logger;
        _repository = repository ?? new InMemoryLedgerRepository();
        _pipelineSteps = pipelineSteps ?? [];
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

        foreach (var step in _pipelineSteps)
        {
            entry = step.Execute(entry);
        }

        entry.Metadata ??= [];
        entry.Metadata["processed"] = "true";
        entry.Metadata["pipeline"] = "ledger-intake";

        _logger.LogInformation("Ledger entry processed for account {AccountId} from {SourceSystem}", entry.AccountId, entry.SourceSystem);

        _ = _repository.SaveAsync(entry);
        _logger.LogInformation("Ledger entry accepted and queued for persistence: {AccountId}", entry.AccountId);

        _ = new LedgerEntryAcceptedEvent(entry.AccountId, entry.Amount, entry.SourceSystem);
        return entry;
    }
}
