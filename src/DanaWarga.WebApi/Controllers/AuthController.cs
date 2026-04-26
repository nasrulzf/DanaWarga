using DanaWarga.Application.Features.Auth.Queries.Login;
using DanaWarga.Application.Models.Auth;
using DanaWarga.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginQuery(request.UserName, request.Password), cancellationToken);
        return Ok(ToDto(result));
    }

    private static LoginResponse ToDto(LoginResult result)
        => new(result.AccessToken, result.ExpiresAtUtc, result.Roles);
}