using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Shop()
        {
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            List<Category> cateList = dao.GetAllCategory();
            List<Brand> brandList = dao.GetAllBrand();
            ViewBag.cateList = cateList;
            ViewBag.brandList = brandList;
            ViewBag.list = list;
            return View();
        }

        [HttpGet]
        public IActionResult ShopDetail(string pro_id)
        {
            ProductDAO dao = new ProductDAO();
            Product pro = dao.GetProductById(pro_id);
            foreach(var item in pro.pro_img)
            {
                Console.WriteLine(item);

            }
            ViewBag.pro = pro;

            return View();
        }
    }
}
