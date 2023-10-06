using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using Newtonsoft.Json;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        public HomeController(IHttpContextAccessor contx)
        {
            _contx = contx;
        }


        public IActionResult Index()
        {
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            List<Product> listMouse = list.Where(pro => pro.cate_id == 2).ToList();
            List<Product> listKeyboard = list.Where(pro => pro.cate_id == 1).ToList();
            List<Brand> brandList = dao.GetAllBrand();
            List<Category> cateList = dao.GetAllCategory();

            List<int> totalProductBrand = new List<int>();
            foreach (Brand brand in brandList)
            {
                totalProductBrand.Add(list.Count(pro => pro.brand_id == brand.brand_id));
            }

            List<int> totalProductCate = new List<int>();
            foreach (Category cate in cateList)
            {
                totalProductCate.Add(list.Count(pro => pro.cate_id == cate.cate_id));
            }
            _contx.HttpContext.Session.SetString("listBrand", JsonConvert.SerializeObject(brandList));
            _contx.HttpContext.Session.SetString("listCate", JsonConvert.SerializeObject(cateList));

            ViewBag.totalProductBrand = totalProductBrand;
            ViewBag.totalProductCate = totalProductCate;
            ViewBag.brandList = brandList;
            ViewBag.cateList = cateList;
            ViewBag.listMouse = listMouse;
            ViewBag.listKeyboard = listKeyboard;
            ViewBag.list = list;

            return View();

        }
    }
}