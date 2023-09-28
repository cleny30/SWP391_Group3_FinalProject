using Microsoft.AspNetCore.Mvc;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MyAddress()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewOrder()
        {
            return View();
        }
    }
}
