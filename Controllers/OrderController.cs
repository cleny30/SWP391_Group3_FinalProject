using Microsoft.AspNetCore.Mvc;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
