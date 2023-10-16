using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Filter;
using SWP391_Group3_FinalProject.Models;

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
