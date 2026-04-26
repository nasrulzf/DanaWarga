using DanaWarga.Contracts.Payments;
using DanaWarga.WebMvc.Models;
using DanaWarga.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebMvc.Controllers;

public sealed class PaymentsController(ApiGateway apiGateway) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(new PaymentsPageViewModel
        {
            Payments = await apiGateway.GetPaymentsAsync(cancellationToken),
            Residents = await apiGateway.GetResidentsAsync(cancellationToken),
            Houses = await apiGateway.GetHousesAsync(cancellationToken)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Submit(SubmitIplPaymentRequest request, CancellationToken cancellationToken)
    {
        await apiGateway.SubmitPaymentAsync(request, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Validate(Guid paymentId, bool approve, CancellationToken cancellationToken)
    {
        await apiGateway.ValidatePaymentAsync(paymentId, approve, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}