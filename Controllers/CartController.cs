using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Filter;
using SWP391_Group3_FinalProject.Models;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        public CartController(IHttpContextAccessor contx)
        {
            _contx = contx;
        }

        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Index()
        {
            var get = _contx.HttpContext.Session.GetString("Session");
            var cus = JsonConvert.DeserializeObject<Customer>(get);
            OrderDAO dao = new OrderDAO();
            ProductDAO PDAO = new ProductDAO();
            var listC = dao.GetCartByUsername(cus.username);
            List<Product> ProductList = new List<Product>();

            foreach (var c in listC)
            {
                var product = PDAO.GetAllProduct().FirstOrDefault(p => p.pro_id == c.pro_id);
                if (product != null)
                {
                    ProductList.Add(product);
                }
                if (product.discount > 0)
                {
                    c.price = c.price - ((product.discount * c.price) / 100);
                }
            }

            ViewBag.ProductList = ProductList;
            ViewBag.ListCart = listC;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteCart(string us, string pro_id)
        {
            OrderDAO dao = new OrderDAO();
            dao.DeleteCart(us, pro_id);
            int Count = dao.GetCartByUsername(us).Count();
            _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(Count));
            return Content("Success");
        }

        [HttpPost]
        public IActionResult AddToCart(string pro_id, int quantity)
        {
            var get = _contx.HttpContext.Session.GetString("Session");
            var cus = JsonConvert.DeserializeObject<Customer>(get);
            OrderDAO dao = new OrderDAO();
            ProductDAO Pdao = new ProductDAO();

            var List = dao.GetCartByUsername(cus.username).ToList();
            Cart Ca = List.FirstOrDefault(ca => ca.pro_id == pro_id);
            if (Ca == null)
            {
                var pro = Pdao.GetProductById(pro_id);
                Cart c = new Cart
                {
                    username = cus.username,
                    pro_id = pro.pro_id,
                    pro_name = pro.pro_name,
                    price = pro.pro_price,
                    quantity = quantity
                };
                dao.AddCart(c);
                var count = List.Count(c => c.username == cus.username) + 1;
                _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(count));
                return Content(count.ToString());
            }
            else
            {
                List[List.IndexOf(Ca)].quantity = Ca.quantity + quantity;
                dao.CartQuantity(List[List.IndexOf(Ca)]);
                var count = List.Count(c => c.username == cus.username);
                _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(count));
                return Content(count.ToString());
            }
        }
        [HttpGet]
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Checkout()
        {
            OrderDAO Odao = new OrderDAO();
            AccountDAO Adao = new AccountDAO();
            ProductDAO Pdao = new ProductDAO();

            var get = _contx.HttpContext.Session.GetString("Session");
            var cus = JsonConvert.DeserializeObject<Customer>(get);
            var CartList = Odao.GetCartByUsername(cus.username);
            foreach (var c in CartList)
            {
                var product = Pdao.GetAllProduct().FirstOrDefault(p => p.pro_id == c.pro_id);

                if (product.discount > 0)
                {
                    c.price = c.price - ((product.discount * c.price) / 100);
                }
            }
            ViewBag.ListCart = CartList;
            ViewBag.Addresses = Adao.GetCustomerAddress(cus.username);
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Addresses a, string des, double bill)
        {
            OrderDAO Odao = new OrderDAO();
            AccountDAO Adao = new AccountDAO();
            ProductDAO Pdao = new ProductDAO();

            var get = _contx.HttpContext.Session.GetString("Session");
            var cus = JsonConvert.DeserializeObject<Customer>(get);
            var CartList = Odao.GetCartByUsername(cus.username);
            foreach (var c in CartList)
            {
                var product = Pdao.GetAllProduct().FirstOrDefault(p => p.pro_id == c.pro_id);

                if (product.discount > 0)
                {
                    c.price = c.price - ((product.discount * c.price) / 100);
                }
            }
            Odao.Checkout(CartList, des, bill, a);
            int Count = Odao.GetCartByUsername(cus.username).Count();
            _contx.HttpContext.Session.SetString("Count", JsonConvert.SerializeObject(Count));
            return Content("Success");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(Cart c)
        {           
            OrderDAO Odao= new OrderDAO();
            string noti=Odao.CartQuantity(c);

            return Json(noti);
        }
    }
}
