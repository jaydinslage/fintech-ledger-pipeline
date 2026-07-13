using Fintech.LedgerPipeline.Service.Models;
using Fintech.LedgerPipeline.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fintech.LedgerPipeline.Service.Controllers;

[ApiController]
[Route("api/ledger")]
public class LedgerController : ControllerBase
{
    private readonly ILedgerProcessingService _service;

    public LedgerController(ILedgerProcessingService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Ingest([FromBody] LedgerEntry entry)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = _service.Process(entry);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred while processing the ledger entry.", detail = ex.Message });
        }
    }
}
