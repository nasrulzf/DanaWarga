using DanaWarga.Application.Features.FinancialPeriods.Commands.ClosePeriod;
using DanaWarga.Contracts.Finance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/financial-periods")]
[Authorize(Roles = "Treasurer")]
public sealed class FinancialPeriodsController(ISender sender) : ControllerBase
{
    [HttpPost("close")]
    public async Task<IActionResult> Close([FromBody] ClosePeriodRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new ClosePeriodCommand(request.Year, request.Month, request.ClosedBy), cancellationToken);
        return Ok(new { id });
    }
}
