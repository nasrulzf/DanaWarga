using DanaWarga.Contracts.Auth;
using DanaWarga.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DanaWarga.WebMvc.Controllers;

public sealed class AccountController(ApiGateway apiGateway) : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginRequest(string.Empty, string.Empty));

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await apiGateway.LoginAsync(request, cancellationToken);
        HttpContext.Session.SetString("access_token", result.AccessToken);
        HttpContext.Session.SetString("roles", string.Join(',', result.Roles));
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}