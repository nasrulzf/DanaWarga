using DanaWarga.WebMvc.Models;
using DanaWarga.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebMvc.Controllers;

public sealed class HomeController(ApiGateway apiGateway) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var residents = await apiGateway.GetResidentsAsync(cancellationToken);
        var houses = await apiGateway.GetHousesAsync(cancellationToken);
        var payments = await apiGateway.GetPaymentsAsync(cancellationToken);

        return View(new DashboardViewModel
        {
            ResidentCount = residents.Count,
            HouseCount = houses.Count,
            PendingPayments = payments.Count(x => x.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
        });
    }
}
