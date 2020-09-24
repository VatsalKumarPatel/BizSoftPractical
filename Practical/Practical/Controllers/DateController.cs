using Microsoft.AspNetCore.Mvc;

namespace Practical.Controllers
{
    public class DateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
