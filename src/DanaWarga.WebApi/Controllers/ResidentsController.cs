using DanaWarga.Application.Features.Residents.Commands.CreateResident;
using DanaWarga.Application.Features.Residents.Queries.ListResidents;
using DanaWarga.Application.Models.Residents;
using DanaWarga.Contracts.Residents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/residents")]
[Authorize]
public sealed class ResidentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Treasurer,Committee")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var data = await sender.Send(new ListResidentsQuery(), cancellationToken);
        return Ok(data.Select(ToDto));
    }

    [HttpPost]
    [Authorize(Roles = "Treasurer,Committee")]
    public async Task<IActionResult> Post([FromBody] CreateResidentRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new CreateResidentCommand(request.FullName, request.Email, request.PhoneNumber), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    private static ResidentDto ToDto(ResidentResult result)
        => new(result.Id, result.FullName, result.Email, result.PhoneNumber);
}