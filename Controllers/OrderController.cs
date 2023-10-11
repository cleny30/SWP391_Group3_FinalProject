using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.Filter;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
