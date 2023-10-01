using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class HomeController : Controller
    {

   
        public IActionResult Index()
        {
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            List<Product> listMouse = dao.getMouse();
            List<Product> listKeyboard = dao.getAllKeyboard();
            List<Brand> brandList = dao.GetAllBrand();
            List<Category> cateList = dao.GetAllCategory();
         
           
            int total1 = dao.countTotalProductByBrand(1);
          
         
       

            ViewBag.Total = total1;
            ViewBag.brandList = brandList;
            ViewBag.cateList = cateList;
            ViewBag.listMouse = listMouse;
            ViewBag.listKeyboard = listKeyboard;
            ViewBag.list = list;
         
            return View();
        
        }
   
    }





}