using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Domain.Identity;

namespace WebApp.Controllers;

/// <summary>
/// Convenience endpoints that integrate your custom navigation/UI with ASP.NET Core Identity.
/// Login/Register pages are Razor Pages under /Identity/Account/...
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
    /// Logs the user out and redirects to home.
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
