using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.Filter;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class CartController : Controller
    {
        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
