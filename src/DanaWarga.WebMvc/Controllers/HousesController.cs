using DanaWarga.Contracts.Houses;
using DanaWarga.WebMvc.Models;
using DanaWarga.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebMvc.Controllers;

public sealed class HousesController(ApiGateway apiGateway) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(new HousesPageViewModel
        {
            Houses = await apiGateway.GetHousesAsync(cancellationToken),
            Residents = await apiGateway.GetResidentsAsync(cancellationToken)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHouseRequest request, CancellationToken cancellationToken)
    {
        await apiGateway.CreateHouseAsync(request, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}