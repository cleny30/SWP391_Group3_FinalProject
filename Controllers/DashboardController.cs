using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using System.Drawing.Drawing2D;

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
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            // Use LINQ to filter products with quantity equal to 0 or 1
            var filteredProducts = list.Where(product => product.pro_quan == 0 || product.pro_quan == 1).ToList();
            // Use LINQ to filter products with quantity greater than 1
            var filteredProductsGreaterThanOne = list
                .Where(product => product.pro_quan > 1)
                .ToList();
            ViewBag.LowQuantityProduct = filteredProducts;
            ViewBag.NormalProduct = filteredProductsGreaterThanOne;


            return View();
        }


        //Do Post của Import Product
        [HttpPost]
        public IActionResult GetImportProduct(int totalCartPriceNumber)
        {

            var ProductIDs = Request.Form["pro_id"];
            var ProductNames = Request.Form["pro_name"];
            var Quantities = Request.Form["amount"];
            var Prices = Request.Form["price"];

            var ProductImported = new List<Receipt_Product>();

            //Get all the product into recieptProduct
            for (int i = 0; i < ProductIDs.Count; i++)
            {
                var recieptProduct = new Receipt_Product
                {
                    pro_id = ProductIDs[i].ToString().Trim(),
                    pro_name = ProductNames[i].ToString().Trim(),
                    amount = Convert.ToInt32(Quantities[i].ToString().Trim()),
                    price = Convert.ToInt32(Prices[i].ToString().Trim())

                };
                ProductImported.Add(recieptProduct);
            }

            //Create an Import_Reciept
            Import_Reciept IR = new Import_Reciept
            {
                Date_Import = DateTime.Now,
                Person_In_Charge = "Nguyen Huu Duy",
                Payment = totalCartPriceNumber
            };
            ImportRecieptDAO dao = new ImportRecieptDAO();
            dao.CreateOrderReciept(IR, ProductImported);


            return RedirectToAction("ProductPage", "Dashboard");
        }


        //Trang để coi giỏ hàng
        public IActionResult ProductPage()
        {
            ProductDAO dao = new ProductDAO();

            //Get List for Page
            List<Brand> BrandList = dao.GetAllBrand();
            List<Category> CategoryList = dao.GetAllCategory();
            List<Product> ProductList = dao.GetAllProduct();


            //ViewBag
            ViewBag.BrandList = BrandList;
            ViewBag.CategoryList = CategoryList;
            ViewBag.ProductList = ProductList;
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


        //Get Brand Info
        [HttpPost]
        public IActionResult GetBrandInfo(int brand_id)
        {

            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ProductDAO dao = new ProductDAO();
                Brand brand = dao.GetBrandByID(brand_id);

                if (brand != null)
                {
                    Console.WriteLine(brand);
                    return Ok(brand); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Brand not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        //Get Category Info
        [HttpPost]
        public IActionResult GetCategoryInfo(int cate_id)
        {

            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ProductDAO dao = new ProductDAO();
                Category category = dao.GetCatByID(cate_id);

                if (category != null)
                {
                    Console.WriteLine(category);
                    return Ok(category); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Brand not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private readonly IWebHostEnvironment _environment;

        public DashboardController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //Update Brand
        public IActionResult UpdateBrand(Brand brand, IFormFile BrandLogo)
        {

            ProductDAO dao = new ProductDAO();
            if (BrandLogo != null && BrandLogo.Length > 0)
            {
                try
                {
                    var uniqueFileName = brand.brand_id + "_Logo" + Path.GetExtension(BrandLogo.FileName); ;

                    // Define the path where the file will be saved on the server.
                    var webRootPath = _environment.WebRootPath;
                    var uploadPath = Path.Combine(webRootPath, "source_img", "brand_logo");
                    var filePath = Path.Combine(_environment.WebRootPath, "source_img", "brand_logo", uniqueFileName);


                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        // Copy the uploaded file's content to the stream.
                        BrandLogo.CopyTo(stream);
                    }

                    // Create a URL to access the saved file.
                    var imageUrl = "\\source_img\\brand_logo\\" + uniqueFileName;

                    // Now, imageUrl can be used as the source in your HTML.
                    brand.brand_img = imageUrl;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during file upload or processing.
                    // You can add logging or return an error response to the client.
                }
                dao.EditBrand(brand);
            }
            else
            {
                dao.EditBrandWithoutImage(brand);
            }


            return RedirectToAction("ProductPage", "Dashboard");
        }

        //Update Category
        public IActionResult UpdateCategory(Category category)
        {
            ProductDAO dao = new ProductDAO();
            dao.EditCategory(category);
            return RedirectToAction("ProductPage", "Dashboard");

        }

        //Add Brand
        public IActionResult AddBrand(Brand brand, IFormFile Brand_Logo)
        {
            ProductDAO dao = new ProductDAO();

            List<Brand> BrandList = dao.GetAllBrand();
            int highestBrandId = BrandList.Max(b => b.brand_id);
            int nextBrandID = highestBrandId + 1;

            if (Brand_Logo != null && Brand_Logo.Length > 0)
            {
                try
                {
                    var uniqueFileName = nextBrandID + "_Logo" + Path.GetExtension(Brand_Logo.FileName); ;

                    // Define the path where the file will be saved on the server.
                    var webRootPath = _environment.WebRootPath;
                    var uploadPath = Path.Combine(webRootPath, "source_img", "brand_logo");
                    var filePath = Path.Combine(_environment.WebRootPath, "source_img", "brand_logo", uniqueFileName);


                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        // Copy the uploaded file's content to the stream.
                        Brand_Logo.CopyTo(stream);
                    }

                    // Create a URL to access the saved file.
                    var imageUrl = "\\source_img\\brand_logo\\" + uniqueFileName;

                    // Now, imageUrl can be used as the source in your HTML.
                    brand.brand_img = imageUrl;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during file upload or processing.
                    // You can add logging or return an error response to the client.
                }
                dao.AddBrand(brand);
            }
            return RedirectToAction("ProductPage", "Dashboard");
        }

        //Add Category
        public IActionResult AddCategory(Category category)
        {
            ProductDAO dao = new ProductDAO();
            dao.AddCategory(category);
            return RedirectToAction("ProductPage", "Dashboard");
        }

        //Show Product Details
        public IActionResult ProductDetailPage(string ID)
        {
            ProductDAO dao = new ProductDAO();
            Product product = dao.GetProductById(ID);
            List<Brand> BrandList = dao.GetAllBrand();
            List<Category> CategoryList = dao.GetAllCategory();

            //ViewBag
            ViewBag.ProductAttribute = product.pro_attribute;
            ViewBag.ProductImage = product.pro_img;
            ViewBag.CategoryList = CategoryList;
            ViewBag.BrandList = BrandList;
            return View(product);
        }


    }

}
