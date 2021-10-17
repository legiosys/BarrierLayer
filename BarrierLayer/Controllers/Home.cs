using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}