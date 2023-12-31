﻿using System.Web;
using System;
using Microsoft.AspNetCore.Mvc;
using SWP391_Group3_FinalProject.DAOs;
using SWP391_Group3_FinalProject.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using SWP391_Group3_FinalProject.Filter;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SWP391_Group3_FinalProject.Controllers
{

    public class ProductController : Controller
    {
        private readonly IHttpContextAccessor _contx;
        public ProductController(IHttpContextAccessor contx)
        {
            _contx = contx;
        }

        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult Shop()
        {
            try
            {
                var sortFilter = Request.Query["sort"].ToString();
                var orderFilter = Request.Query["order"].ToString();

                #region get List
                ProductDAO dao = new ProductDAO();

                List<Product> list = dao.GetAllProduct();

                list = list.OrderBy(p => p.pro_quan == 0 || !p.isAvailable ? 1 : 0).ToList();


                var sortCombine = Enumerable.Empty<Product>();
                if (sortFilter.Length > 0 && !sortFilter.Equals(""))
                {
                    var discount = Enumerable.Empty<Product>();
                    var selling = Enumerable.Empty<Product>();
                    if (sortFilter.Contains("discount"))
                    {
                        discount = list.Where(pro => pro.discount > 0).ToList();
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
                List<Category> cateList = dao.GetAllCategory().Where(c => c.isAvailable == true).ToList();
                List<Brand> brandList = dao.GetAllBrand().Where(b => b.isAvailable == true).ToList();
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
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ShopDetail(string pro_id)
        {
            try
            {
                OrderDAO orderDAO = new OrderDAO();

                int cart_quan = 0;
                List<Tuple<string, int>> cartCount = new List<Tuple<string, int>>();


                var get = _contx.HttpContext.Session.GetString("Session");
                if (!string.IsNullOrEmpty(get))
                {
                    var cus = JsonConvert.DeserializeObject<Customer>(get);
                    Cart c = orderDAO.GetCartByUsername(cus.username).FirstOrDefault(p => p.pro_id == pro_id);

                    if (c != null)
                    {
                        cart_quan = c.quantity;
                    }

                    List<Cart> cartList = orderDAO.GetCartByUsername(cus.username);

                    foreach (var cart in cartList)
                    {
                        Tuple<string, int> tupple = new Tuple<string, int>(cart.pro_id, cart.quantity);
                        cartCount.Add(tupple);
                    }

                }



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
                List<Product> productByCateList = list.Where(pro => pro.cate_id == pro1.cate_id && pro.pro_quan > 0 && pro.isAvailable == true).ToList();
                ViewBag.pro = pro1;
                ViewBag.productByCateList = productByCateList;
                ViewBag.brandList = brandList;
                ViewBag.RandomProducts = randomProducts;

                ViewBag.cartQuan = cart_quan;

                ViewBag.cartCount = cartCount;

                return View();
            }
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }

        }

        [HttpGet]
        [ServiceFilter(typeof(CustomerFilter))]
        public IActionResult ShopSearch(string searchTerm)
        {
            try
            {
                ProductDAO dao = new ProductDAO();
                List<Product> searchList = dao.GetAllProduct();
                searchList = searchList.OrderBy(p => p.pro_quan == 0 || !p.isAvailable ? 1 : 0).ToList();

                List<Product> foundProducts = new List<Product>();
                // Lấy trang hiện tại từ query string hoặc mặc định là trang đầu tiên
                var currentPage = Request.Query["page"].ToString() != "" ? Convert.ToInt32(Request.Query["page"]) : 1; ; // Default value


                if (searchTerm != null)
                {
                    foundProducts = searchList.Where(product => product.pro_name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Kích thước trang (số sản phẩm mỗi trang)
                int pageSize = 12;

                // Tính chỉ số bắt đầu của sản phẩm cần hiển thị
                int startIndex = (currentPage - 1) * pageSize;

                int totalItems = foundProducts.Count();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                bool isFirstPage = currentPage == 1;
                bool isLastPage = currentPage == totalPages;

                var productToshow = foundProducts.Skip(startIndex).Take(pageSize).ToList();


                ViewBag.foundProducts = productToshow;
                ViewBag.searchterm = searchTerm;
                //Set page navigation
                ViewBag.currentPage = currentPage;
                ViewBag.pageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.IsFirstPage = isFirstPage;
                ViewBag.IsLastPage = isLastPage;
                ViewBag.CurrentPage = currentPage;

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
            catch
            {
                return RedirectToAction("/StatusCodeError");
            }
        }
    }
}
