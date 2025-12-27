using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebApp.Domain.Identity;

namespace WebApp.Middleware;

/// <summary>
/// Mellanlager som tvingar inloggade användare att komplettera obligatoriska profilfält
/// innan de får fortsätta till övriga sidor. Om profilen är ofullständig omdirigeras
/// användaren till /AccountProfile/Edit och en temporär toast-meddelande sätts.
/// </summary>
public class ProfileCompletionMiddleware
{
    private readonly RequestDelegate _next;

    public ProfileCompletionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, ITempDataDictionaryFactory tempDataFactory)
    {
        // Utför endast kontroll för inloggade användare.
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            // Lista över sökvägs-prefix som ska undantas från tvångskontrollen.
            var excludedPrefixes = new[]
            {
                "/Identity",
                "/AccountProfile/Edit",
                "/AccountProfile/ChangePassword",
                "/EditCV",
                "/MyCv",
                "/css",
                "/js",
                "/images",
                "/lib"
            };

            // Om sökvägen inte matchar något undantag utförs profilkontrollen.
            if (!excludedPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                var user = await userManager.GetUserAsync(context.User);

                if (user != null)
                {
                    // Profil anses ofullständig om något av dessa fält saknas.
                    var isProfileIncomplete = string.IsNullOrWhiteSpace(user.FirstName)
                        || string.IsNullOrWhiteSpace(user.LastName)
                        || string.IsNullOrWhiteSpace(user.City)
                        || string.IsNullOrWhiteSpace(user.PostalCode);

                    if (isProfileIncomplete)
                    {
                        // Sätt temporära meddelanden som visas efter omdirigering.
                        var tempData = tempDataFactory.GetTempData(context);
                        tempData["ToastTitle"] = "Välkommen!";
                        tempData["ToastMessage"] = "Komplettera ditt konto med dina personliga uppgifter så att du blir synlig för andra.";

                        context.Response.Redirect("/AccountProfile/Edit");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }
}
