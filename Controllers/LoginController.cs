using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Principal;
using System.Web;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class LoginController : Controller
    {

        private readonly IHttpContextAccessor _contx;
        public LoginController(IHttpContextAccessor contx)
        {
            _contx = contx;
        }

        [HttpGet("/Login")]
        public IActionResult Index()
        {
            try
            {
                int cookievalue = int.Parse(_contx.HttpContext.Request.Cookies["Role"]);
                if (cookievalue == 0 || cookievalue == 1)
                {
                    return RedirectToAction("Index", "Dashboard"); // chuyen sang trang 

                }
                else if (cookievalue == 2)
                {
                    return RedirectToAction("Index", "Home"); // chuyen sang trang 
                }
            }
            catch (Exception ex)
            {

            }
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

        [HttpPost]
        public ActionResult Verify(String username, String password, String isRem)
        {
            AccountDAO dao = new AccountDAO(); // goi ham dao
            Account acc = dao.GetAccount(username, password); // lay thong tin dao

            if (acc != null)
            {
                if (isRem != null)
                {
                    HttpContext.Response.Cookies.Append("Username", acc.username, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(3),
                    });
                    HttpContext.Response.Cookies.Append("Role", acc.role.ToString(), new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(3),
                    });
                }

                if (acc.role == 2)
                {
                    Customer customer = dao.GetCustomer(username);
                    _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(customer));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AdminAndStaff adminandstaff = dao.GetAdminAndStaff(username, acc.role);
                    _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(adminandstaff));
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return RedirectToAction("Index", "Login"); // neu sai quay lai trang login

        }

        //[HttpPost]
        //public ActionResult Register(Custo)
        //{
        //    AccountDAO dao = new AccountDAO(); // goi ham dao
        //    Customer customer = dao.GetCustomer(ustxt); // lay thong tin dao

        //    if (customer != null)
        //    {
        //        return RedirectToAction("SignUp", "Login");
        //    }
        //    else
        //    {
        //        dao.AddCustomer(ustxt, pwdtxt, nametxt, emailtxt, phonetxt);
        //        return RedirectToAction("Index", "Login");
        //    }

        //}
    }
}
