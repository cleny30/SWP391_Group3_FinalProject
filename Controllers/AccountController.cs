using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Filter;
using SWP391_Group3_FinalProject.Models;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        public AccountController(IHttpContextAccessor contx)
        {
            _contx = contx;
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult MyAccount()
        {

            // Retrieve the serialized list from the session
            var Cus = _contx.HttpContext.Session.GetString("Session");

            // Deserialize the JSON string into a list
            var acc = JsonConvert.DeserializeObject<Customer>(Cus);
            AccountDAO DAO = new AccountDAO();
            Customer cs = DAO.GetCustomerByUsername(acc.username);
            ViewBag.Account = cs;
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
            OrderDAO DAO = new OrderDAO();
            var Cus = _contx.HttpContext.Session.GetString("Session");
            var uid = JsonConvert.DeserializeObject<Customer>(Cus);
            if (uid != null)
            {
                List<Order> list = DAO.GetOrderByUsername(uid.username);
                ViewBag.ListOrder = list;
            }     
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProfile(Customer n)
        {
            var customer = _contx.HttpContext.Session.GetString("Session");

            // Deserialize the JSON string into a list
            var cus = JsonConvert.DeserializeObject<Customer>(customer);
            n.username = cus.username;
            AccountDAO DAO = new AccountDAO();
            DAO.UpdateCustomer(n);
            return RedirectToAction("MyAccount", "Account");
        }

        [HttpPost]
        public IActionResult ChangePassword(string newPass)
        {
            var customer = _contx.HttpContext.Session.GetString("Session");

            // Deserialize the JSON string into a list
            var cus = JsonConvert.DeserializeObject<Customer>(customer);
            
            AccountDAO DAO = new AccountDAO();
            DAO.ChangePassword(cus.username, newPass);
            return RedirectToAction("ChangePassword", "Account");
        }

        [HttpPost]
        public IActionResult CheckPassword(string pw)
        {
            var customer = _contx.HttpContext.Session.GetString("Session");

            // Deserialize the JSON string into a list
            var cus = JsonConvert.DeserializeObject<Customer>(customer);

            AccountDAO DAO = new AccountDAO();
            Customer cs = DAO.GetCustomer(cus.username, pw);
            if(cs != null)
            {
                return Content("Sucess");
            }
            else
            {
                return Content("Fail");
            }
        }

        [HttpPost]
        public IActionResult OrderDetail(string id)
        {
            OrderDAO DAO = new OrderDAO();
            List<OrderDetail> od = DAO.GetOrderDetail(id);
            return Json(od);
        }
    }
}
