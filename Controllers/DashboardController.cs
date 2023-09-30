using Microsoft.AspNetCore.Mvc;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class DashboardController : Controller
    {
        //Trang chủ của dashboard
        public IActionResult Index()
        {
            return View();
        }

        //Trang để cho admin thêm sản phẩm để bán
        public IActionResult ImportProduct()
        {
            return View();
        }

        //Trang để coi giỏ hàng
        public IActionResult ProductPage()
        {
            return View();
        }

        //Statistic page
        public IActionResult Statistic()
        {
            return View();
        }

        //Coi đơn hàng của khách hàng
        public IActionResult OrderRecieptPage()
        {
            return View();
        }
    }
}
