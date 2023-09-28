using Microsoft.AspNetCore.Mvc;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Shop()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ShopDetail()
        {
            return View();
        }
    }
}
