using Fintech.LedgerPipeline.Service.Models;

namespace Fintech.LedgerPipeline.Service.Repositories;

public interface ILedgerRepository
{
    Task SaveAsync(LedgerEntry entry, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LedgerEntry>> GetAllAsync(CancellationToken cancellationToken = default);
}
