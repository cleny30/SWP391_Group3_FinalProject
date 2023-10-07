using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpGet("/ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
