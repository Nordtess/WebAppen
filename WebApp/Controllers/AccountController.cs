using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Domain.Identity;

namespace WebApp.Controllers;

/// <summary>
/// Enkla endpoints som kopplar ihop applikationens navigation/UI med ASP.NET Core Identity.
/// Inloggning/registrering hanteras av Razor Pages under /Identity/Account/.
/// </summary>
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    /// <summary>
    /// POST: /Account/Logout
    /// Loggar ut användaren och omdirigerar till startsidan.
    /// </summary>
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        return LocalRedirect(returnUrl ?? Url.Action("Index", "Home")!);
    }
}
