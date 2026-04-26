using DanaWarga.Application.Features.Finance.Commands.CreateExpense;
using DanaWarga.Application.Features.Finance.Queries.ListExpenses;
using DanaWarga.Application.Models.Finance;
using DanaWarga.Contracts.Finance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/expenses")]
[Authorize(Roles = "Treasurer,Committee")]
public sealed class ExpensesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ListExpensesQuery(), cancellationToken);
        return Ok(result.Select(ToDto));
    }

    [HttpPost]
    [Authorize(Roles = "Treasurer")]
    public async Task<IActionResult> Post([FromBody] CreateExpenseRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new CreateExpenseCommand(request.Date, request.Category, request.Description, request.Amount), cancellationToken);
        return Ok(new { id });
    }

    private static ExpenseDto ToDto(ExpenseResult result)
        => new(result.Id, result.Date, result.Category, result.Description, result.Amount);
}