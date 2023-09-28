using Microsoft.AspNetCore.Mvc;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
