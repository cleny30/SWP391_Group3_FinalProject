using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Filter;
using SWP391_Group3_FinalProject.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

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
        [ServiceFilter(typeof(CustomerFilter))]
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
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult MyAddress()
        {
            AccountDAO dao = new AccountDAO();
            var customer = _contx.HttpContext.Session.GetString("Session");

            Customer cus = JsonConvert.DeserializeObject<Customer>(customer);

            ViewBag.ListAddress = cus.addresses;
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        [ServiceFilter(typeof(CustomerFilter))]
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
            if (cs != null)
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
            var customer = _contx.HttpContext.Session.GetString("Session");

            Customer cus = JsonConvert.DeserializeObject<Customer>(customer);

            OrderDAO DAO = new OrderDAO();
            List<OrderDetail> od = DAO.GetOrderDetail(id);
            var a = DAO.GetAddressByOrderID(id);
            var orderThis = DAO.GetOrderByUsername(cus.username).FirstOrDefault(o => o.orderId == id);
            var rs = new
            {
                orderDetails = od,
                addresses = a,
                orderDick = orderThis
            };
            return Json(rs);
        }

        [HttpPost]
        public IActionResult UpdateAddress(Addresses a)
        {
            AccountDAO dao = new AccountDAO();
            dao.UpdateAddress(a);
            var customer = _contx.HttpContext.Session.GetString("Session");

            Customer cus = JsonConvert.DeserializeObject<Customer>(customer);

            Addresses tmp = cus.addresses.FirstOrDefault(z => z.ID == a.ID);
            int index = cus.addresses.IndexOf(tmp);
            cus.addresses[index] = a;
            _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus));
            return RedirectToAction(nameof(MyAddress));
        }
        [HttpPost]
        public IActionResult AddAddress(Addresses a)
        {
            AccountDAO dao = new AccountDAO();
            var customer = _contx.HttpContext.Session.GetString("Session");

            Customer cus = JsonConvert.DeserializeObject<Customer>(customer);
            if (a.phonenum != null && a.address != null && a.fullname != null)
            {
                int? kq = dao.AddAddress(a, cus.username);
                if (kq != null)
                {
                    a.ID = kq.Value;
                }
                if (cus.addresses == null)
                {
                    cus.addresses.Add(new Addresses
                    {
                        ID = a.ID,
                        address = a.address,
                        fullname = a.fullname,
                        phonenum = a.phonenum,
                    });
                }
                else
                {
                    cus.addresses.Add(a);
                }

            }
            else
            {
                ViewBag.Message = "Invalid";
            }

            _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus));
            return RedirectToAction(nameof(MyAddress));
        }

        [HttpPost]
        public IActionResult DeleteAddress(int id)
        {
            AccountDAO dao = new AccountDAO();
            var customer = _contx.HttpContext.Session.GetString("Session");

            Customer cus = JsonConvert.DeserializeObject<Customer>(customer);

            dao.DeleteAddress(id);
            var address = cus.addresses.FirstOrDefault(a => a.ID == id);
            cus.addresses.Remove(address);
            _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(cus));
            return Content("Success");
        }

        [HttpPost]
        public IActionResult CheckEmail(string email)
        {
            AccountDAO dao = new AccountDAO();
            int result = dao.CheckEmail(email);

            return result == 1 ? Content("true") : Content("false");
        }

        [HttpPost]
        public IActionResult SendOTP(string email)
        {
            string fromEmail = "clenynguyen@gmail.com";
            string password = "pyaotxulqjcgttwl";

            string reciever = email;

            Random random = new Random();

            string otp = random.Next(100000, 999999).ToString();

            string htmlContent = @"
<!DOCTYPE html>
<html>

<head>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
    <title>OTP SENDING</title>
    <link href='css/styledone.css' rel='stylesheet'>
    <style>
        body {

            font-family: Arial, sans-serif;
            text-align: center;
            padding: 20px;
        }

        .container {
            max-width: 900px;
            margin: 0 auto;
            background-color: rgba(255, 247, 247, 0.1);
            height: 650px;
        }

        .circle {
            width: 150px;
            height: 150px;
            border-radius: 50%;
            background-color: #008000;

            margin: 20px auto;
        }

        .circle::before {
            content: '\2714';
            font-size: 110px;
            color: #fff;
        }

        h1 {
            color: #008000;
            margin-top: 20px;
        }

        .divider {
            margin-top: 20px;
            border-top: 2px solid #333;
        }

        .thank-you {
            font-weight: bold;
            margin-top: 20px;
        }

        .button {
            display: inline-block;
            background-color: #008000;
            color: #fff;
            padding: 10px 20px;
            border-radius: 5px;
            text-decoration: none;
            margin-top: 20px;
        }

        .hotline {
            font-size: 24px;
            color: red;
        }

        p {
            font-size: 17px;
            color: #333;
            margin-left: 20px;
        }

        .container_1 {
            max-width: 620px;
            margin: 0 auto;
            background-color: rgba(255, 247, 247, 0.1);
            height: 100%;
            border: 2px solid gray;

            border-radius: 10px;
        }

        .container_1 p {
            text-align: left;

        }

        .circle_1 {

            border-radius: 50%;

            margin: 20px auto;
        }


        img {
            width: 25%;
        }

        .divider_1 {

            margin-top: 20px;
            border-top: 2px solid gray;
            width: 570px;
            margin-left: 24px;
        }

        .hotline_1 {
            font-size: 50px;
            color: Black;
        }

        .footer {
            max-width: 40%;
            margin-left: 29%;
            text-align: center;
        }

        .footer p {
            color: grey;
            text-align: center;
            font-size: 15px;
        }
    </style>
</head>

<body style=""text-align: center;"">
    <div class='container_1'>
        <div class='circle_1'>
             <img src=""cid:imageId"" alt=""Circle Image"">
        </div>

        <p style='color: black; font-size: 33px; text-align: center; margin-top: 10px;'>Verify your recovery email</p>


        <div class='divider_1'></div>

        <p class='thank-you'></p>
        <p>GEARSHOP has received a request to use <span style='font-weight: bold;'>" + reciever + @"</span>to
            recovery your GEARSHOP's account.
        </p>

        <p> Use this code to finish setting up this recovery account:</p><br>

        <span class='hotline_1'>" + otp + @"</span>
        <br>
        <p style='text-align: center; margin-bottom:0;'>If you do not recognize this request, you can safely
            ignore this email.<br>
        <p style='text-align: center; margin: 0;'><strong>Please do not provide this OTP code to
                others</strong></p>
        </p>

    </div>
    <div class='footer'>
        <p>We are sending this email to let you know
            about important changes to Google Accounts and
            your service.</p>
        <p>2023 Google LLC, 227 Thi Tran Phong Dien, Thanh Pho
            Can Tho, Viet Nam.</p>
    </div>
</body>
</html>";



            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.Subject = "The OTP to reset password";
            message.To.Add(new MailAddress(reciever));
            message.Body = htmlContent;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlContent, null, MediaTypeNames.Text.Html);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Create the relative path to the image
            string relativeImagePath = "wwwroot/source_img/advertising_img/logoGearshop.png";

            //Combine the base directory and the relative path to get the full path
            string fullPath = Path.Combine(baseDirectory, relativeImagePath);

            //Create the LinkedResource using the full path
           LinkedResource imageResource = new LinkedResource(fullPath, MediaTypeNames.Image.Jpeg)
           {
               ContentId = "imageId"
           };
            //Load the image and attach it as linked resource
            alternateView.LinkedResources.Add(imageResource);
            message.AlternateViews.Add(alternateView);

            // Send the email
            try
            {
                smtpClient.Send(message);
                return Content(otp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
            return Content(otp);
            //smtpClient.Send(message);
        }
    }
}
