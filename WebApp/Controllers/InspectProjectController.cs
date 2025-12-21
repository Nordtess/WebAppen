using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("InspectProject")]
    public class InspectProjectController : Controller
    {
        // /InspectProject
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["ProjectId"] = 0;
            return View("InspectProject");
        }

        // /InspectProject/{id}
        [HttpGet("{id:int}")]
        public IActionResult ById(int id)
        {
            ViewData["ProjectId"] = id;
            return View("InspectProject");
        }
    }
}
