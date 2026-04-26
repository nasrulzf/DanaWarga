using DanaWarga.Contracts.Residents;
using DanaWarga.WebMvc.Models;
using DanaWarga.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebMvc.Controllers;

public sealed class ResidentsController(ApiGateway apiGateway) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(new ResidentsPageViewModel
        {
            Residents = await apiGateway.GetResidentsAsync(cancellationToken)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateResidentRequest request, CancellationToken cancellationToken)
    {
        await apiGateway.CreateResidentAsync(request, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}