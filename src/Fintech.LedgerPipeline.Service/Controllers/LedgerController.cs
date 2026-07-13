using Fintech.LedgerPipeline.Service.Models;
using Fintech.LedgerPipeline.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.LedgerPipeline.Service.Controllers;

[ApiController]
[Route("api/ledger")]
public class LedgerController : ControllerBase
{
    private readonly ILedgerProcessingService _service;
    private readonly ILogger<LedgerController> _logger;

    public LedgerController(ILedgerProcessingService service, ILogger<LedgerController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Ingest([FromBody] LedgerEntry entry)
    {
        _logger.LogInformation("Ledger ingestion request received for account {AccountId}", entry?.AccountId ?? "unknown");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Ledger ingestion validation failed for account {AccountId}", entry?.AccountId ?? "unknown");
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = _service.Process(entry!);
            _logger.LogInformation("Ledger entry processed successfully for account {AccountId}", result.AccountId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Ledger ingestion validation error for account {AccountId}", entry?.AccountId ?? "unknown");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while processing ledger entry for account {AccountId}", entry?.AccountId ?? "unknown");
            return StatusCode(500, new { error = "An unexpected error occurred while processing the ledger entry.", detail = ex.Message });
        }
    }
}
