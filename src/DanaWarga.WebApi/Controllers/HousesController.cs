using DanaWarga.Application.Features.Houses.Commands.CreateHouse;
using DanaWarga.Application.Features.Houses.Queries.ListHouses;
using DanaWarga.Application.Models.Houses;
using DanaWarga.Contracts.Houses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/houses")]
[Authorize]
public sealed class HousesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Treasurer,Committee")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var data = await sender.Send(new ListHousesQuery(), cancellationToken);
        return Ok(data.Select(ToDto));
    }

    [HttpPost]
    [Authorize(Roles = "Treasurer,Committee")]
    public async Task<IActionResult> Post([FromBody] CreateHouseRequest request, CancellationToken cancellationToken)
    {
        var id = await sender.Send(new CreateHouseCommand(request.HouseNumber, request.Address, request.ResidentId), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    private static HouseDto ToDto(HouseResult result)
        => new(result.Id, result.HouseNumber, result.Address, result.ResidentId, result.ResidentName);
}