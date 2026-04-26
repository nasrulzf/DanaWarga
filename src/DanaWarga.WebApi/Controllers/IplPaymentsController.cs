using DanaWarga.Application.Features.IplPayments.Commands.CreatePayment;
using DanaWarga.Application.Features.IplPayments.Commands.ValidatePayment;
using DanaWarga.Application.Features.IplPayments.Queries.ListPayments;
using DanaWarga.Application.Models.Payments;
using DanaWarga.Contracts.Payments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/ipl-payments")]
[Authorize]
public sealed class IplPaymentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Treasurer,Committee")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var data = await sender.Send(new ListPaymentsQuery(), cancellationToken);
        return Ok(data.Select(ToDto));
    }

    [HttpPost]
    [Authorize(Roles = "Treasurer,Resident")]
    public async Task<IActionResult> Submit([FromBody] SubmitIplPaymentRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new CreatePaymentCommand(request.ResidentId, request.HouseId, request.TotalAmount, request.PaymentDate, request.ProofFilePath), cancellationToken);
        return Ok(new { id });
    }

    [HttpPost("{paymentId:guid}/validate")]
    [Authorize(Roles = "Treasurer")]
    public async Task<IActionResult> Validate(Guid paymentId, [FromBody] ValidatePaymentRequest request, CancellationToken cancellationToken)
    {
        var success = await sender.Send(new ValidatePaymentCommand(paymentId, request.Approve), cancellationToken);
        return Ok(new { success });
    }

    private static IplPaymentDto ToDto(IplPaymentResult result)
        => new(
            result.Id,
            result.ResidentId,
            result.HouseId,
            result.TotalAmount,
            result.PaymentDate,
            result.Status,
            result.Allocations.Select(x => new PaymentAllocationDto(x.Year, x.Month, x.AllocatedAmount)).ToArray());
}