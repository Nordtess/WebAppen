using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
public class EditCVController : Controller
{
    public IActionResult Index()
    {
        return View("EditCV");
    }
}
