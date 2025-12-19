using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class InspectProjectController : Controller
    {
        public IActionResult Index()
        {
            return View("InspectProject");
        }
    }
}
