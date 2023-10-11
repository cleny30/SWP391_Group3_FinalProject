using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.Filter;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult MyAddress()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult ViewOrder()
        {
            return View();
        }
    }
}
