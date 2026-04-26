using DanaWarga.WebMvc.Models;
using DanaWarga.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebMvc.Controllers;

public sealed class ReportsController(ApiGateway apiGateway) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int? year, CancellationToken cancellationToken)
    {
        var selectedYear = year ?? DateTime.UtcNow.Year;
        var report = await apiGateway.GetIplMatrixAsync(selectedYear, cancellationToken);
        return View(new ReportsPageViewModel
        {
            Year = selectedYear,
            Report = report
        });
    }
}