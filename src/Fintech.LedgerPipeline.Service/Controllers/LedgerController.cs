using Microsoft.AspNetCore.Mvc;
using Fintech.LedgerPipeline.Service.Models;
using Fintech.LedgerPipeline.Service.Services;

namespace Fintech.LedgerPipeline.Service.Controllers;

[ApiController]
[Route("api/ledger")]
public class LedgerController : ControllerBase
{
    private readonly LedgerProcessingService _service;

    public LedgerController(LedgerProcessingService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Ingest(LedgerEntry entry)
    {
        var result = _service.Process(entry);
        return Ok(result);
    }
}
