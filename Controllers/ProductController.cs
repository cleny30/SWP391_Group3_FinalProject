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
            #region get List
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            List<Category> cateList = dao.GetAllCategory();
            List<Brand> brandList = dao.GetAllBrand();
            #endregion

            // Lấy trang hiện tại từ query string hoặc mặc định là trang đầu tiên
            var currentPage = Request.Query["page"].ToString() != "" ? Convert.ToInt32(Request.Query["page"]) : 1; ; // Default value

            // Kích thước trang (số sản phẩm mỗi trang)
            int pageSize = 9;

            // Tính chỉ số bắt đầu của sản phẩm cần hiển thị
            int startIndex = (currentPage - 1) * pageSize;

            #region Processing Filter
            var SelectedCategory = Request.Query["category"].ToString().Split(',');
            var SelectedBrand = Request.Query["brand"].ToString().Split(',');
            var selectedCategoryIds = new List<int>();
            var selectedBrandIds = new List<int>();

            //Filter by category
            if (SelectedCategory.Length > 0 && !SelectedCategory.Equals(""))
            {
                foreach (var item in SelectedCategory)
                {
                    if (int.TryParse(item, out int parsedCategory))
                    {
                        selectedCategoryIds.Add(parsedCategory);
                    }
                    else
                    {
                        // Handle invalid category value (e.g., log an error, skip, or take appropriate action)
                        Console.WriteLine("Invalid Category: " + item);
                    }
                }

            }
            var productsByCategory = list.Where(pro => selectedCategoryIds.Contains(pro.cate_id)).ToList();

            //Filter by brand
            if (SelectedBrand.Length > 0 && !SelectedBrand.Equals(""))
            {
                foreach (var item in SelectedBrand)
                {
                    if (int.TryParse(item, out int parsedCategory))
                    {
                        selectedBrandIds.Add(parsedCategory);
                    }
                    else
                    {
                        // Handle invalid category value (e.g., log an error, skip, or take appropriate action)
                        Console.WriteLine("Invalid Category: " + item);
                    }
                }

            }
            var productsByBrand = list.Where(pro => selectedBrandIds.Contains(pro.brand_id)).ToList();
            #endregion

            #region Processing list to display
            var productToshow = list.Skip(startIndex).Take(pageSize);
            var combineProduct = Enumerable.Empty<Product>();

            if (productsByCategory.Count() > 0 && productsByBrand.Count() > 0)
            {
                combineProduct = productsByBrand.Intersect(productsByCategory);
            }
            else if (productsByCategory.Count() > 0)
            {
                combineProduct = productsByCategory;
            }
            else if (productsByBrand.Count() > 0)
            {
                combineProduct = productsByBrand;
            }


            if (combineProduct.Count() > 0)
            {
                productToshow = combineProduct.Skip(startIndex).Take(pageSize);
            }
            else if (combineProduct.Count() == 0 && (selectedBrandIds.Count() > 0 || selectedCategoryIds.Count() > 0))
            {
                productToshow = null;
            }
            #endregion

            #region Set attribute to View Bag
            ViewBag.cateList = cateList;
            ViewBag.brandList = brandList;
            ViewBag.selectedCategoryIds = selectedCategoryIds;
            ViewBag.selectedBrandIds = selectedBrandIds;
            ViewBag.currentPage = currentPage;
            ViewBag.isFilterUsed = (selectedBrandIds.Count() > 0 || selectedCategoryIds.Count() > 0) ? true : false;
            ViewBag.list = productToshow;
            ViewBag.combineProduct = combineProduct;
            ViewBag.pageSize = pageSize;
            ViewBag.totalProduct = list.Count();
            ViewBag.combineProduct = combineProduct.Count();
            #endregion

            return View();
        }

        [HttpGet]
        public IActionResult ShopDetail()
        {

            return View();
        }
    }
}
