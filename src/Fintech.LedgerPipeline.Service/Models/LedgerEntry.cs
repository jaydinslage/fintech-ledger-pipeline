namespace Fintech.LedgerPipeline.Service.Models;

public class LedgerEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string AccountId { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string SourceSystem { get; set; } = default!;
    public Dictionary<string, string>? Metadata { get; set; }
}