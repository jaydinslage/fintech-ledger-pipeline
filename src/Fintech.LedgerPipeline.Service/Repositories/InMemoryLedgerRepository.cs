using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Repositories;

public sealed class InMemoryLedgerRepository : ILedgerRepository
{
    private readonly List<LedgerEntry> _entries = [];

    public Task SaveAsync(LedgerEntry entry, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entry);
        _entries.Add(entry);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<LedgerEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<LedgerEntry>>(_entries.AsReadOnly().ToList());
    }
}
