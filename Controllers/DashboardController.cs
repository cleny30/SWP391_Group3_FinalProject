using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using System.Drawing.Drawing2D;
using System.Web.Helpers;

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
            ProductDAO dao = new ProductDAO();
            List<Brand> BrandList = dao.GetAllBrand();
            
            //ViewBag
            ViewBag.BrandList = BrandList;

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


        private readonly IWebHostEnvironment _environment;

        public DashboardController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //Update Brand
        public IActionResult UpdateBrand(Brand brand ,IFormFile BrandLogo)
        {

            ProductDAO dao = new ProductDAO();
            if (BrandLogo != null && BrandLogo.Length > 0)
            {
                try
                {
                    var uniqueFileName =  brand.brand_id + "_Logo" + Path.GetExtension(BrandLogo.FileName); ;

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
            }  else
            {
                dao.EditBrandWithoutImage(brand);
            }

            
            return RedirectToAction("ProductPage", "Dashboard");
        }



    }
}
