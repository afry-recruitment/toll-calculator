using Microsoft.AspNetCore.Mvc;

namespace AfryTollApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
