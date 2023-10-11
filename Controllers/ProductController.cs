using System.Web;
using System;
using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using SWP391_Group3_FinalProject.Filter;

namespace SWP391_Group3_FinalProject.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Shop()
        {
            var sortFilter = Request.Query["sort"].ToString();
            var orderFilter = Request.Query["order"].ToString();

            #region get List
            ProductDAO dao = new ProductDAO();

            List<Product> list = dao.GetAllProduct();

            var sortCombine = Enumerable.Empty<Product>();
            if (sortFilter.Length > 0 && !sortFilter.Equals(""))
            {
                var discount = Enumerable.Empty<Product>();
                var selling = Enumerable.Empty<Product>();
                if (sortFilter.Contains("discount"))
                {
                    discount = list.Where(pro => pro.discount > 0).ToList();
                }

                if (sortFilter.Contains("best_selling"))
                {
                    selling = dao.SortProductByDiscount();
                }

                if (discount.Count() > 0 && selling.Count() > 0)
                {
                    sortCombine = discount.Intersect(selling);
                }
                else if (sortFilter.Contains("discount"))
                {
                    sortCombine = discount;
                }
                else if (sortFilter.Contains("best_selling"))
                {
                    sortCombine = selling;
                }
            }

            if (sortCombine.Count() > 0)
            {
                list = sortCombine.ToList();
            }


            if (orderFilter.Equals("highest"))
            {
                list = list.OrderByDescending(product => product.pro_price - (product.pro_price * product.discount) / 100).ToList();
            }
            else if (orderFilter.Equals("lowest"))
            {
                list = list.OrderBy(product => product.pro_price - (product.pro_price * product.discount) / 100).ToList();
            }
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

            if (selectedCategoryIds.Count() > 0 && selectedBrandIds.Count() > 0)
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



            int totalItems = selectedCategoryIds.Count == 0 && selectedBrandIds.Count == 0
                     ? list.Count()
                     : combineProduct.Count();

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            bool isFirstPage = currentPage == 1;
            bool isLastPage = currentPage == totalPages;

            //Get total product by brand
            List<int> totalProductBrand = new List<int>();
            foreach (Brand brand in brandList)
            {
                totalProductBrand.Add(list.Count(pro => pro.brand_id == brand.brand_id));
            }

            //Get total product by category
            List<int> totalProductCate = new List<int>();
            foreach (Category cate in cateList)
            {
                totalProductCate.Add(list.Count(pro => pro.cate_id == cate.cate_id));
            }

            //Get total product on sale
            int totalProductOnsale = list.Count(pro => pro.discount > 0);


            #region Set attribute to View Bag
            //Set category list and brand list
            ViewBag.cateList = cateList;
            ViewBag.brandList = brandList;

            //Set category and brand filter used
            ViewBag.selectedCategoryIds = selectedCategoryIds;
            ViewBag.selectedBrandIds = selectedBrandIds;

            //Set page navigation
            ViewBag.currentPage = currentPage;
            ViewBag.pageSize = pageSize;

            //Amount of total product and product filter
            ViewBag.totalProduct = list.Count();
            ViewBag.combineProduct = combineProduct;
            ViewBag.totalProductBrand = totalProductBrand;
            ViewBag.totalProductCate = totalProductCate;
            ViewBag.totalProductOnsale = totalProductOnsale;

            //Store product in list
            ViewBag.combineProduct = combineProduct;
            ViewBag.list = productToshow;

            ViewBag.sort = sortFilter;
            ViewBag.order = orderFilter;
            ViewBag.TotalPages = totalPages;
            ViewBag.IsFirstPage = isFirstPage;
            ViewBag.IsLastPage = isLastPage;
            ViewBag.CurrentPage = currentPage;
            //ViewBag.currentUrl = fullPathWithQuery;
            #endregion

            return View();
        }

        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ShopDetail(string pro_id)
        {
            int numberOfRandomProducts = 5;
            Random random = new Random();
            ProductDAO dao = new ProductDAO();
            List<Product> list = dao.GetAllProduct();
            List<Brand> brandList = dao.GetAllBrand();
            Product pro1 = dao.GetProductById(pro_id);
            List<Product> randomProducts = new List<Product>();
            for (int i = 0; i < numberOfRandomProducts && list.Count > 0; i++)
            {
                int randomIndex = random.Next(0, list.Count);
                Product randomProduct = list[randomIndex];
                randomProducts.Add(randomProduct);
                //remove duplicated item.
                list.RemoveAt(randomIndex);
            }
            List<Product> productByCateList = list.Where(pro => pro.brand_id == pro1.brand_id).ToList();
            ViewBag.pro = pro1;
            ViewBag.productByCateList = productByCateList;
            ViewBag.brandList = brandList;
            ViewBag.RandomProducts = randomProducts;
            return View();
        }
    
    }
}
