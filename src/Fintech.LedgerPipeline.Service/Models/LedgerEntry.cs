using System.ComponentModel.DataAnnotations;

namespace Fintech.LedgerPipeline.Service.Models;

public class LedgerEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MinLength(1)]
    public string AccountId { get; set; } = default!;

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    [MinLength(3)]
    public string Currency { get; set; } = "USD";

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [MinLength(1)]
    public string SourceSystem { get; set; } = default!;

    public Dictionary<string, string>? Metadata { get; set; }
}