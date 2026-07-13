namespace Fintech.LedgerPipeline.Service.Events;

public sealed record LedgerEntryAcceptedEvent(string AccountId, decimal Amount, string SourceSystem);
