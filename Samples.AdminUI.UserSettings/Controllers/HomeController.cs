using Microsoft.AspNetCore.Mvc;

namespace Samples.AdminUI.UserSettings.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}