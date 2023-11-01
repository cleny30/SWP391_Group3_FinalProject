using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework.Profiler;
using Newtonsoft.Json;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Filter;
using SWP391_Group3_FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SWP391_Group3_FinalProject.Controllers
{
    [ServiceFilter(typeof(LoginFilter))]
    [ServiceFilter(typeof(ManagerFilter))]
    public class DashboardController : Controller
    {
        //Trang chủ của dashboard
        private readonly IHttpContextAccessor _contx;
        private readonly IWebHostEnvironment _environment;

        public DashboardController(IHttpContextAccessor contx, IWebHostEnvironment environment)
        {
            _contx = contx;
            _environment = environment;
        }

        public IActionResult Index()
        {
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            var filteredProducts = list.Where(product => product.pro_quan == 0 || product.pro_quan == 1).ToList();
            ViewBag.LowQuantityProduct = filteredProducts;

            OrderDAO ORDao = new OrderDAO();
            ImportRecieptDAO IRDao = new ImportRecieptDAO();
            DateTime currentDate = DateTime.Now;
            int currentYear = currentDate.Year;
            int currentMonth = currentDate.Month;
            //Completed Order
            var CompletedOrder = ORDao.GetAllOrder().Where(o => o.status == 4 && o.endDay != null && o.endDay.Value.Year == currentYear && o.endDay.Value.Month == currentMonth).ToList();
            int CompletedOrderCount = CompletedOrder.Count();

            

            //Total Income this month
            
            var IncomeList = ORDao.GetAllOrder().Where(o => o.status == 4 && o.endDay != null && o.endDay.Value.Year == currentYear && o.endDay.Value.Month == currentMonth).Select(o => o.totalPrice).ToList();

            double TotalIncome = IncomeList.Sum();

            //Total Revenue this month
            var RevenueList = IRDao.GetAllImportReceipt().Where(IR => IR.Date_Import.Year == currentYear && IR.Date_Import != null
                                                                     && IR.Date_Import.Month == currentMonth).Select(IR => IR.Payment).ToList();

            //Top 10 Products
            List<OrderDetail> orderDetails = ORDao.GetAllOrderDetail();
            var consolidatedProducts = orderDetails
            .GroupBy(detail => detail.productID)
              .Select(group => new
           {
              productID = group.Key,
               productName = group.First().productName, // Take the product name from the first item in the group
             quantity = group.Sum(detail => detail.quantity)
               })
           .OrderByDescending(product => product.quantity) // Sort by quantity in descending order
           .Take(10) // Select the top 10 products
            .ToList();

            ViewBag.Top10Product = consolidatedProducts;
            double TotalSpent = RevenueList.Sum();

            double Revenue = TotalIncome - TotalSpent;

            List<Tuple<string, double>> Top10Customers = ORDao.GetTop10Customer();

            ViewBag.Top10Customer = Top10Customers;
            ViewData["TotalIncome"] = TotalIncome;
            ViewData["Revenue"] = Revenue;
            ViewData["CompletedOrder"] = CompletedOrderCount;
            return View();
        }



        public IActionResult StaffList()
        {
            ManagerDAO dao = new ManagerDAO();
            List<Manager> list = dao.GetAllManagers().Where(manager => manager.isAdmin == false).ToList();
            ViewBag.managers = list;
            return View();
        }

        public IActionResult DisableStaff(int id)
        {
            ManagerDAO dao = new ManagerDAO();
            dao.DisableStaff(id);
            return RedirectToAction("StaffList", "Dashboard");
        }
        public IActionResult CreateAccount()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateAccount(Manager manager)
        {
            //Check if the account created is admin
            bool isAdmin = Request.Form["isAdmin"] == "on";

            //---------------Code Here----------------
            ManagerDAO dao = new ManagerDAO();
            manager.isAdmin = isAdmin;
            dao.AddManager(manager);
            //----------------------------------------
            return RedirectToAction("StaffList", "Dashboard");
        }
        //Kiem tra username co duoc su dung chua
        [HttpPost]
        public IActionResult CheckUsername(string username)
        {
            ManagerDAO dao = new ManagerDAO();
            Manager manager = dao.GetManagerByUsername(username);
            if (manager != null)
            {
                return Content("Fail");
            }
            else
            {
                return Content("Success");
            }
        }

        //Kiem tra email co duoc su dung chua
        [HttpPost]
        public IActionResult CheckEmail(string email)
        {
            ManagerDAO dao = new ManagerDAO();
            Manager manager = dao.GetManagerByEmail(email);
            if (manager != null)
            {
                return Content("Fail");
            }
            else
            {
                return Content("Success");
            }
        }

        [HttpPost]
        public IActionResult CheckEmailUpdate(string email)
        {
            ManagerDAO dao = new ManagerDAO();
            Manager manager = dao.GetManagerByEmail(email);
            var serializedManager = _contx.HttpContext.Session.GetString("Session");
            var currentManager = JsonConvert.DeserializeObject<Manager>(serializedManager);
            if (manager != null && !manager.email.Equals(currentManager.email))
            {
                return Content("Fail");
            }
            else
            {
                return Content("Success");
            }
        }


        public IActionResult PersonalProfile()
        {
            return View();
        }

        [HttpPost]
        //Update staff information
        public IActionResult PersonalProfile(Manager staff)
        {
            ManagerDAO dao = new ManagerDAO();
            AccountDAO accDAO = new AccountDAO();
            dao.UpdateStaffAccount(staff);
            Manager manager = accDAO.GetManagerByUsername(staff.username);
            manager.email = staff.email;
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Your profile has been successfully updated!"));
            _contx.HttpContext.Session.SetString("Session", JsonConvert.SerializeObject(manager));
            return RedirectToAction("PersonalProfile", "Dashboard");
        }

        //Trang để coi đơn nhập hàng

        public IActionResult ImportReceipts()
        {
            ImportRecieptDAO IRdao = new ImportRecieptDAO();
            List<Import_Reciept> IRList = IRdao.GetAllImportReceipt();


            //ViewBag
            ViewBag.IRList = IRList;
            return View();
        }

        //Logging out for Admin And Staff

        public IActionResult Logout()
        {
            //Delete Session
            string ManagerInfo, role;
            _contx.HttpContext.Session.Remove("Session");
            _contx.HttpContext.Session.Remove("action");
            int cookievalue = 0;
            if (_contx.HttpContext.Request.Cookies["role"] != null)
            {
                cookievalue = int.Parse(_contx.HttpContext.Request.Cookies["role"]);
            }
            if (cookievalue != null)
            {
                Response.Cookies.Delete("username");
                Response.Cookies.Delete("role");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult GetIRInfo(int ID)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ImportRecieptDAO dao = new ImportRecieptDAO();
                Import_Reciept importReciept = dao.GetImportReceiptByID(ID);

                if (importReciept != null)
                {
                    Console.WriteLine(importReciept);
                    return Ok(importReciept); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("IR not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult GetRPInfo(int ID)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ImportRecieptDAO dao = new ImportRecieptDAO();
                List<Receipt_Product> list = dao.GetRPByID(ID);

                if (list != null)
                {
                    Console.WriteLine(list);
                    return Ok(list); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("IR not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        //Trang để cho admin thêm sản phẩm để bán

        public IActionResult ImportProduct()
        {

            ProductDAO dao = new ProductDAO();
            List<Brand> BrandList = dao.GetAllBrand();
            List<Category> CategoryList = dao.GetAllCategory();
            List<Product> list = dao.GetAllProduct();

            // Use LINQ to filter products with quantity equal to 0 or 1
            var filteredProducts = list.Where(product => product.pro_quan == 0 || product.pro_quan == 1).ToList();
            // Use LINQ to filter products with quantity greater than 1
            var filteredProductsGreaterThanOne = list
                .Where(product => product.pro_quan > 1)
                .ToList();
            //View Bag
            ViewBag.CategoryList = CategoryList;
            ViewBag.BrandList = BrandList;
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
            string Manager = _contx.HttpContext.Session.GetString("Session");
            Manager manager = new Manager();
            if (Manager != null)
            {
                manager = JsonConvert.DeserializeObject<Manager>(Manager);
            }
            //Create an Import_Reciept
            Import_Reciept IR = new Import_Reciept
            {
                Date_Import = DateTime.Now,
                Person_In_Charge = manager.fullname,
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
            ViewBag.BrandList = BrandList.Where(brand => brand.isAvailable == true).ToList();
            ViewBag.CategoryList = CategoryList.Where(cate => cate.isAvailable == true).ToList();
            ViewBag.ProductList = ProductList.Where(pro => pro.isAvailable == true).ToList();
            ViewBag.ProductListDisable = ProductList.Where(pro => pro.isAvailable == false).ToList();
            return View();
        }

        //Statistic page

        public IActionResult Statistic()
        {
            //Get Revenue
            OrderDAO dao = new OrderDAO();
            var totalIncome = dao.GetTotalIncome();
            var totalPayment = dao.GetTotalPayment();
            var Revenue = totalIncome - totalPayment;
            //End Get Revenue


            //Get data for Pie Chart
            List<Tuple<string, int>> list = dao.GetTotalQuantityOnCateName();

            //End Get data for Pie Chart


            //Get data for  Chart are
            List<Tuple<string, double>> listIncomeMonth = dao.GetIncomeForEachMonth();

            List<Tuple<string, double>> listPayment = dao.GetPaymentForEachMonth();

            var listRevenue = listIncomeMonth
    .Join(listPayment,
        income => income.Item1,
        payment => payment.Item1,
        (income, payment) => new Tuple<string, double>(
            income.Item1,
            income.Item2 - payment.Item2
        ))
    .ToList();


            //End Get data for Chart are

            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalPayment = totalPayment;
            ViewBag.Revenue = Revenue;

            ViewBag.listPie = list;


            ViewBag.listIncomeMonth = listIncomeMonth;
            ViewBag.listPayemt = listPayment;
            ViewBag.listRevenue = listRevenue;


            return View();
        }

        //Coi đơn hàng của khách hàng

        public IActionResult OrderRecieptPage()
        {
            OrderDAO dao = new OrderDAO();
            List<Order> list = dao.GetAllOrder();
            ViewBag.OrderPending = list.Where(o => o.status == 1).ToList();

            ViewBag.OrderAccepted = list.Where(o => o.status == 2).ToList();
            ViewBag.OrderShipped = list.Where(o => o.status == 3).ToList();
            ViewBag.OrderCompleted = list.Where(o => o.status == 4).ToList();
            return View();
        }

        public IActionResult AcceptOrder(string ID)
        {
            //----------Code Here------------//
            //Khi accept order thì nó sẽ lấy ID của staff đó gắn vào chỗ Staff_ID
            //chuyển status = 2
            OrderDAO dao = new OrderDAO();
            var serializedmanager = _contx.HttpContext.Session.GetString("Session");
            var manager = JsonConvert.DeserializeObject<Manager>(serializedmanager);
            dao.AcceptOrder(ID, manager.ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " accepted by " + manager.fullname));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }
        public IActionResult CancelOrder(string ID)
        {
            //chuyển status = 0
            OrderDAO dao = new OrderDAO();
            dao.CancelOrder(ID);
            dao.ReturnProduct(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " has been cancelled. Products from order have been sent back to store"));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }
        public IActionResult ShippedOrder(string ID)
        {
            //Gắn End_Date là date lúc bấm nút
            //status = 3
            OrderDAO dao = new OrderDAO();
            dao.ShippedOrder(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " shipped on " + DateTime.Now.ToString("dd/MM/yyyy") ));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }
        public IActionResult CompletedOrder(string ID)
        {
            //chuyển status = 4
            OrderDAO dao = new OrderDAO();
            dao.CompletedOrder(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Order " + ID + " has been delivered"));
            return RedirectToAction("OrderRecieptPage", "Dashboard");
        }


        [HttpPost]
        public IActionResult KeywordExisted(string keyword)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                ProductDAO dao = new ProductDAO();
                List<Category> list = dao.GetAllCategory();
                Category cat = list.FirstOrDefault(c => c.keyword.Equals(keyword));
                if (cat != null)
                {
                    Console.WriteLine(cat);
                    return Ok(cat); // Return a 200 OK response with JSON data
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

            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Brand with ID " + brand.brand_id + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");
        }

        //Update Category
        public IActionResult UpdateCategory(Category category)
        {
            ProductDAO dao = new ProductDAO();
            dao.EditCategory(category);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Update Category with ID " + category.cate_id + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");

        }

        //AddProduct
        [HttpPost]
        public IActionResult AddProduct(Product pro, List<IFormFile> imgFile, List<string> feature, List<string> description)
        {
            ProductDAO dao = new ProductDAO();
            Category cate = dao.GetCatByID(pro.cate_id);
            string folder = cate.cate_name.Trim();
            var webRootPath = _environment.WebRootPath;
            var uploadPath = Path.Combine(webRootPath, "source_img", "product_image", folder);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            int index = 1;
            foreach (var image in imgFile)
            {
                if (image != null && image.Length > 0)
                {

                    string fileName = pro.pro_id + "_" + index + Path.GetExtension(image.FileName);
                    string PathIntoDatabase = Path.Combine("\\" + "source_img", "product_image", folder, fileName);
                    string filePath = Path.Combine(_environment.WebRootPath, "source_img", "product_image", folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    pro.pro_img.Add(PathIntoDatabase);
                    index++;
                }
            }

            int count = feature.Count();
            for (int i = 0; i < count; i++)
            {
                pro.pro_attribute[feature[i]] = description[i];
            }

            dao.AddProductWithDetails(pro);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Product with ID " + pro.pro_id + " Successfully"));
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


            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Brand with name " + brand.brand_name + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");
        }

        //Add Category
        public IActionResult AddCategory(Category category)
        {
            ProductDAO dao = new ProductDAO();
            dao.AddCategory(category);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Add Category with name " + category.cate_name + " Successfully"));
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
            ViewBag.CategoryList = CategoryList.Where(c => c.isAvailable == true).ToList();
            ViewBag.BrandList = BrandList.Where(b => b.isAvailable == true).ToList();
            return View(product);
        }



        //Get new product Id
        public IActionResult GetNewProductID(int cate_id)
        {
            ProductDAO dao = new ProductDAO();
            string newID = dao.GetNewProductID(cate_id);
            return Content(newID);
        }

        [HttpPost]
        public IActionResult EditProduct(List<int> Image_ID, Product pro, List<string> feature, List<string> description, List<IFormFile> imgFile, string selectedImages, string ImagesList)
        {
            ProductDAO dao = new ProductDAO();
            List<string> imageList = ImagesList.Split(',').ToList();
            if (selectedImages != null)
            {
                List<string> selectedImageList = JsonConvert.DeserializeObject<List<string>>(selectedImages);
                foreach (var path in selectedImageList)
                {
                    var webRootPath = _environment.WebRootPath;
                    string filePath = webRootPath + path;
                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            dao.DeleteImageByPath(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("Error: " + ex.Message);
                    }

                }
            }



            Category cate = dao.GetCatByID(pro.cate_id);
            string folder = cate.cate_name.Trim();
            List<IFormFile> file = new List<IFormFile>();
            List<string> name = new List<string>();


            int count = dao.countProductImage(pro.pro_id);
            for (int i = 0; i < count; i++)
            {
                var Images = Request.Form.Files[pro.pro_id + "_" + Image_ID[i]];
                name.Add(pro.pro_id + "_" + Image_ID[i]);
                file.Add(Images);

            }


            for (int i = 0; i <= count - 1; i++)
            {
                var Image = file[i];

                var OriginalImage = imageList[i];

                if (Image != null && Image.Length > 0)
                {
                    try
                    {
                        var uniqueFileName = name[i] + Path.GetExtension(file[i].FileName);
                        string fileExtension = Path.GetExtension(uniqueFileName);
                        var webRootPath = _environment.WebRootPath;
                        var uploadPath2 = Path.Combine("\\" + "source_img", "product_image", folder, uniqueFileName);
                        var uploadPath = Path.Combine(_environment.WebRootPath, "source_img", "product_image", folder, uniqueFileName);
                        string filePath = webRootPath + OriginalImage;
                        try
                        {
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                                dao.DeleteImageByPath(OriginalImage);
                                pro.pro_img.Add(uploadPath2);
                            }
                        }
                        catch (Exception ex)
                        {
                            return Content("Error: " + ex.Message);
                        }
                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            // Copy the uploaded file's content to the stream.
                            Image.CopyTo(stream);
                        }

                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during file upload or processing.
                        // You can add logging or return an error response to the client.
                    }
                }

            }

            int index;

            if (Image_ID.Any()) // Check if the sequence contains any elements
            {
                index = Image_ID.Max() + 1;
            }
            else
            {
                // The sequence is empty, so set index to 1 or any other default value as needed
                index = 1;
            }
            foreach (var image in imgFile)
            {
                if (image != null && image.Length > 0)
                {

                    string fileName = pro.pro_id + "_" + index + Path.GetExtension(image.FileName);
                    string PathIntoDatabase = Path.Combine("\\" + "source_img", "product_image", folder, fileName);
                    string filePath = Path.Combine(_environment.WebRootPath, "source_img", "product_image", folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    pro.pro_img.Add(PathIntoDatabase);
                    index++;
                }
            }

            int counter = feature.Count();
            for (int i = 0; i < counter; i++)
            {
                pro.pro_attribute[feature[i]] = description[i];
            }


            dao.DeleteAttributeByID(pro.pro_id);
            dao.UpdateProductWithDetails(pro);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Update Product with ID " + pro.pro_id + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");

        }

        public IActionResult DisableProduct(string ID)
        {
            ProductDAO dao = new ProductDAO();
            dao.DisableProduct(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Disable Product with ID " + ID + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");
        }

        public IActionResult EnableProduct(string ID)
        {
            ProductDAO dao = new ProductDAO();
            dao.EnableProduct(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Enable Product with ID " + ID + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");
        }


        public IActionResult DisableBrand(int ID)
        {
            ProductDAO dao = new ProductDAO();
            dao.DisableBrand(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Disable Brand with ID " + ID + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");
        }

        public IActionResult DisableCategory(int ID)
        {
            ProductDAO dao = new ProductDAO();
            dao.DisableCategory(ID);
            _contx.HttpContext.Session.SetString("Message", JsonConvert.SerializeObject("Disable Category with ID " + ID + " Successfully"));
            return RedirectToAction("ProductPage", "Dashboard");
        }

        [HttpPost]
        public IActionResult GetStaffInfo(int ID)
        {
            try
            {
                // Assuming you have a data access layer (ProductDAO) to retrieve brand information
                //ImportRecieptDAO dao = new ImportRecieptDAO();
                //Import_Reciept importReciept = dao.GetImportReceiptByID(ID);
                ManagerDAO dao = new ManagerDAO();
                Manager manager = dao.GetAllManagers().FirstOrDefault(_manager => _manager.ID == ID);

                if (manager != null)
                {
                    Console.WriteLine(manager);
                    return Ok(manager); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Staff not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult GetOrderInfo (string ID)
        {
            try
            {
                OrderDAO dao = new OrderDAO();
                AccountDAO AccDAO = new AccountDAO();
                ManagerDAO ManaDAO = new ManagerDAO();
                Order order = dao.GetAllOrder().FirstOrDefault(o => o.orderId.Equals(ID));
                Order_Address address = dao.GetAllOrderAddressBasedOnID(order.orderId);
                Customer cus = AccDAO.GetCustomerByUsername(order.username);
                List<OrderDetail> list = dao.GetOrderDetail(order.orderId);
                Manager staff = ManaDAO.GetAllManagers().FirstOrDefault(m => m.ID == order.staffId);
                if (order != null)
                {
                    var result = new
                    {
                        Order = order,
                        Address = address,
                        Email = cus.email,
                        OrderDetail = list,
                        Staff = staff,
                    };
                    Console.WriteLine(order);
                    return Ok(result); // Return a 200 OK response with JSON data
                }
                else
                {
                    return NotFound("Order not found");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

}
