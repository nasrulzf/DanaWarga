using DanaWarga.Application.Features.Finance.Commands.CreateIncome;
using DanaWarga.Application.Features.Finance.Queries.ListIncomes;
using DanaWarga.Application.Models.Finance;
using DanaWarga.Contracts.Finance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/incomes")]
[Authorize(Roles = "Treasurer,Committee")]
public sealed class IncomesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ListIncomesQuery(), cancellationToken);
        return Ok(result.Select(ToDto));
    }

    [HttpPost]
    [Authorize(Roles = "Treasurer")]
    public async Task<IActionResult> Post([FromBody] CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new CreateIncomeCommand(request.Date, request.Source, request.Description, request.Amount), cancellationToken);
        return Ok(new { id });
    }

    private static IncomeDto ToDto(IncomeResult result)
        => new(result.Id, result.Date, result.Source, result.Description, result.Amount);
}