using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.Filter;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        public HomeController(IHttpContextAccessor contx)
        {
            _contx = contx;
        }

        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Index()
        {
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();

            list = list.Where(p => p.pro_quan > 0 && p.isAvailable==true).ToList();

            List<Product> listMouse = list.Where(pro => pro.cate_id == 2 && pro.pro_quan > 0 && pro.isAvailable == true).ToList();
            List<Product> listKeyboard = list.Where(pro => pro.cate_id == 1 && pro.pro_quan > 0 && pro.isAvailable == true).ToList();
            List<Brand> brandList = dao.GetAllBrand().Where(b=>b.isAvailable==true).ToList();
            List<Category> cateList = dao.GetAllCategory().Where(c => c.isAvailable == true).ToList();

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
            _contx.HttpContext.Session.Remove("ErrorLogin");

            ViewBag.totalProductBrand = totalProductBrand;
            ViewBag.totalProductCate = totalProductCate;
            ViewBag.brandList = brandList;
            ViewBag.cateList = cateList;
            ViewBag.listMouse = listMouse;
            ViewBag.listKeyboard = listKeyboard;
            ViewBag.list = list;

            List<Tuple<string, int>> cartCount = new List<Tuple<string, int>>();
            var get = _contx.HttpContext.Session.GetString("Session");
            if (!string.IsNullOrEmpty(get))
            {
                var cus = JsonConvert.DeserializeObject<Customer>(get);

                OrderDAO orderDAO = new OrderDAO();
                List<Cart> cartList = orderDAO.GetCartByUsername(cus.username);

                foreach (var cart in cartList)
                {
                    Tuple<string, int> tupple = new Tuple<string, int>(cart.pro_id, cart.quantity);
                    cartCount.Add(tupple);
                }
            }

            ViewBag.cartCount = cartCount;

            return View();

        }

        [HttpPost]
        public IActionResult SearchItem(string searchbox)
        {
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            list = list.OrderBy(p => p.pro_quan == 0 || !p.isAvailable ? 1 : 0).ToList();

            List<Product> foundProducts = new List<Product>();
            if (searchbox != null)
            {
                foundProducts = list.Where(product => product.pro_name.Contains(searchbox, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            return Json(foundProducts);

        }

        [Route("/StatusCodeError/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if(statusCode == 404)
            {
                ViewBag.ErrorMessage = "Page not found";
            }
            return View();

        }
    }
    
}