﻿using SWP391_Group3_FinalProject.Models;
using SWP391_Group3_FinalProject.NewFolder;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace SWP391_Group3_FinalProject.DAOs
{
    public class ProductDAO
    {
        private SqlConnection conn;
        private SqlCommand _command;
        private SqlDataReader _reader;
        public ProductDAO()
        {
            conn = DbConnection.GetConnection();
            _command = new SqlCommand();
            _command.Connection = conn;
        }

        //Get all product
        public List<Product> GetAllProduct()
        {
            List<Product> list = new List<Product>();
            _command.CommandText = "Select * from Product";
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product pro = new Product();
                    pro.pro_id = _reader.GetString(0);
                    pro.brand_id = _reader.GetInt32(1);
                    pro.cate_id = _reader.GetInt32(2);
                    pro.pro_name = _reader.GetString(3);
                    pro.pro_quan = _reader.GetInt32(4);
                    pro.pro_des = _reader.GetString(5);
                    pro.pro_price = double.Parse(_reader.GetValue(6).ToString());
                    pro.discount = _reader.GetInt32(7);
                    pro.isAvailable = _reader.GetBoolean(8);
                    list.Add(pro);
                }
            }
            foreach (var item in list)
            {
                _command.CommandText = "SELECT * FROM Product_Image WHERE pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_img.Add(_reader.GetString(1));
                    }
                }
            }

            foreach (var item in list)
            {
                _command.CommandText = "Select * from Product_Attribute where pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_attribute[_reader.GetString(1)] = _reader.GetString(2);
                    }
                }
            }
            return list;
        }


        //Get Product by ID to show it's full details
        public Product GetProductById(string productId)
        {
            Product product = null;
            _command.CommandText = "SELECT * FROM Product WHERE pro_id = @productId";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@productId", productId);

            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    product = new Product();
                    product.pro_id = _reader.GetString(0);
                    product.brand_id = _reader.GetInt32(1);
                    product.cate_id = _reader.GetInt32(2);
                    product.pro_name = _reader.GetString(3);
                    product.pro_quan = _reader.GetInt32(4);
                    product.pro_des = _reader.GetString(5);
                    product.pro_price = double.Parse(_reader.GetValue(6).ToString());
                    product.discount = _reader.GetInt32(7);
                    product.isAvailable = _reader.GetBoolean(8);
                }
            }

            if (product != null)
            {
                _command.CommandText = "SELECT * FROM Product_Image WHERE pro_id = @productId";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@productId", productId);

                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        product.pro_img.Add(_reader.GetString(1));
                    }
                }

                _command.CommandText = "SELECT * FROM Product_Attribute WHERE pro_id = @productId";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@productId", productId);

                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        product.pro_attribute[_reader.GetString(1)] = _reader.GetString(2);
                    }
                }
            }

            return product;
        }


        public List<Product> SortProductByDiscount()
        {
            List<Product> list = new List<Product>();
            _command.CommandText = "Select * from Product where discount_percent > 0";
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product pro = new Product();
                    pro.pro_id = _reader.GetString(0);
                    pro.brand_id = _reader.GetInt32(1);
                    pro.cate_id = _reader.GetInt32(2);
                    pro.pro_name = _reader.GetString(3);
                    pro.pro_quan = _reader.GetInt32(4);
                    pro.pro_des = _reader.GetString(5);
                    pro.pro_price = double.Parse(_reader.GetValue(6).ToString());
                    pro.discount = _reader.GetInt32(7);
                    pro.isAvailable = _reader.GetBoolean(8);
                    list.Add(pro);
                }
            }
            foreach (var item in list)
            {
                _command.CommandText = "SELECT * FROM Product_Image WHERE pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_img.Add(_reader.GetString(1));
                    }
                }
            }

            foreach (var item in list)
            {
                _command.CommandText = "Select * from Product_Attribute where pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_attribute[_reader.GetString(1)] = _reader.GetString(2);
                    }
                }
            }
            return list;
        }


        public List<Product> SortProductByPrice(int key)
        {
            List<Product> list = new List<Product>();
            if (key == 1)
            {
                _command.CommandText = "SELECT * FROM Product ORDER BY pro_price - ( pro_price * discount_percent) / 100  DESC";
            }
            else
            {
                _command.CommandText = "SELECT * FROM Product ORDER BY pro_price - ( pro_price * discount_percent) / 100  ASC";
            }
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product pro = new Product();
                    pro.pro_id = _reader.GetString(0);
                    pro.brand_id = _reader.GetInt32(1);
                    pro.cate_id = _reader.GetInt32(2);
                    pro.pro_name = _reader.GetString(3);
                    pro.pro_quan = _reader.GetInt32(4);
                    pro.pro_des = _reader.GetString(5);
                    pro.pro_price = double.Parse(_reader.GetValue(6).ToString());
                    pro.discount = _reader.GetInt32(7);
                    pro.isAvailable = _reader.GetBoolean(8);
                    list.Add(pro);
                }
            }
            foreach (var item in list)
            {
                _command.CommandText = "SELECT * FROM Product_Image WHERE pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_img.Add(_reader.GetString(1));
                    }
                }
            }

            foreach (var item in list)
            {
                _command.CommandText = "Select * from Product_Attribute where pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_attribute[_reader.GetString(1)] = _reader.GetString(2);
                    }
                }
            }
            return list;

        }

        /*This method use to add Product's Details
         * 
         * 
         */
        public void AddProductWithDetails(Product pro)
        {
            // Thêm thông tin chung của sản phẩm
            _command.CommandText = "INSERT INTO Product (pro_id, Brand_ID, Cat_ID, pro_name, pro_quan, pro_des, pro_price, discount_percent, isAvailable) " +
                       "VALUES (@pro_id, @brand_id, @cate_id, @pro_name, @pro_quan, @pro_des, @pro_price, @discount, @isAvailable)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@pro_id", pro.pro_id);
            _command.Parameters.AddWithValue("@brand_id", pro.brand_id);
            _command.Parameters.AddWithValue("@cate_id", pro.cate_id);
            _command.Parameters.AddWithValue("@pro_name", pro.pro_name);
            _command.Parameters.AddWithValue("@pro_quan", pro.pro_quan);
            _command.Parameters.AddWithValue("@pro_des", pro.pro_des);
            _command.Parameters.AddWithValue("@pro_price", pro.pro_price);
            _command.Parameters.AddWithValue("@discount", 0);
            _command.Parameters.AddWithValue("@isAvailable", 1);

            // Thực thi câu lệnh
            _command.ExecuteNonQuery();

            // Thêm hình ảnh của sản phẩm
            foreach (var img in pro.pro_img)
            {
                _command.CommandText = "INSERT INTO Product_Image (pro_id, Product_Image) VALUES (@pro_id, @pro_img)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@pro_id", pro.pro_id);
                _command.Parameters.AddWithValue("@pro_img", img);

                // Thực thi câu lệnh
                _command.ExecuteNonQuery();
            }

            // Thêm các thuộc tính của sản phẩm
            foreach (var kvp in pro.pro_attribute)
            {
                _command.CommandText = "INSERT INTO Product_Attribute (pro_id, Feature, Des) VALUES (@pro_id, @feature, @des)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@pro_id", pro.pro_id);
                _command.Parameters.AddWithValue("@feature", kvp.Key);
                _command.Parameters.AddWithValue("@des", kvp.Value);

                // Thực thi câu lệnh
                _command.ExecuteNonQuery();
            }
        }


        //Update Product
        public void UpdateProductWithDetails(Product pro)
        {
            // Thêm thông tin chung của sản phẩm
            _command.CommandText = "UPDATE Product SET pro_name = @pro_name, Brand_ID = @Brand_ID, Cat_ID = @Cat_ID, pro_des = " +
                                    "@pro_des, pro_price = @pro_price, discount_percent = @discount WHERE pro_id = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Brand_ID", pro.brand_id);
            _command.Parameters.AddWithValue("@Cat_ID", pro.cate_id);
            _command.Parameters.AddWithValue("@pro_name", pro.pro_name);
            _command.Parameters.AddWithValue("@pro_des", pro.pro_des);
            _command.Parameters.AddWithValue("@pro_price", pro.pro_price);
            _command.Parameters.AddWithValue("@discount", pro.discount);
            _command.Parameters.AddWithValue("@ID", pro.pro_id);

            // Thực thi câu lệnh
            _command.ExecuteNonQuery();

            // Thêm hình ảnh của sản phẩm
            foreach (var img in pro.pro_img)
            {
                _command.CommandText = "INSERT INTO Product_Image (pro_id, Product_Image) VALUES (@pro_id, @pro_img)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@pro_id", pro.pro_id);
                _command.Parameters.AddWithValue("@pro_img", img);

                // Thực thi câu lệnh
                _command.ExecuteNonQuery();
            }

            // Thêm các thuộc tính của sản phẩm
            foreach (var kvp in pro.pro_attribute)
            {
                _command.CommandText = "INSERT INTO Product_Attribute (pro_id, Feature, Des) VALUES (@pro_id, @feature, @des)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@pro_id", pro.pro_id);
                _command.Parameters.AddWithValue("@feature", kvp.Key);
                _command.Parameters.AddWithValue("@des", kvp.Value);

                // Thực thi câu lệnh
                _command.ExecuteNonQuery();
            }
        }
        //Update Product

        public void AddProductQuantity(string ID, int amount)
        {
            _command.CommandText = @"UPDATE Product
                                     SET pro_quan = pro_quan + @quantity
                                     WHERE pro_id = @ID;";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@quantity", amount);
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }

        public void DeleteAttributeByID(string ID)
        {
            _command.CommandText = "delete from Product_Attribute where pro_id = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }


        public int countProductImage(string ID)
        {
            int count = 0;
            _command.CommandText = "select count(Product_Image)\r\nfrom Product_Image\r\nwhere pro_id = @ID;";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    count = _reader.GetInt32(0);
                }
            }
            return count;
        }

        public void DeleteImageByPath(string path)
        {
            _command.CommandText = "delete from Product_Image where Product_Image = @path";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@path", path);
            _command.ExecuteNonQuery();
        }



        public void DisableProduct(string ID)
        {
            _command.CommandText = "update  Product set isAvailable = 0 where pro_id = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }

        public void EnableProduct(string ID)
        {
            _command.CommandText = "update  Product set isAvailable = 1 where pro_id = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }

        public void DisableBrand(int ID)
        {
            _command.CommandText = "update Brand set isAvailable = 0 where Brand_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();

            _command.CommandText = "update Product set isAvailable = 0\r\nwhere Brand_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }

        public void DisableCategory(int ID)
        {
            _command.CommandText = "update Category set isAvailable = 0 where Cat_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();

            _command.CommandText = "update Product set isAvailable = 0\r\nwhere Cat_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }

        public void EnableBrand(int ID)
        {
            _command.CommandText = "update Brand set isAvailable = 1 where Brand_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();

            _command.CommandText = "update Product set isAvailable = 1\r\nwhere Brand_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }


        public void EnableCategory(int ID)
        {
            _command.CommandText = "update Category set isAvailable = 1 where Cat_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();

            _command.CommandText = "update Product set isAvailable = 1\r\nwhere Cat_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            _command.ExecuteNonQuery();
        }
        //------------------------------------------------------------------------------------------------------------
        //START BRAND CRUD
        public List<Brand> GetAllBrand()
        {
            List<Brand> list = new List<Brand>();
            _command.CommandText = "Select * from Brand";
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Brand brand = new Brand();
                    brand.brand_id = _reader.GetInt32(0);
                    brand.brand_name = _reader.GetString(1);
                    brand.isAvailable = _reader.GetBoolean(2);
                    brand.brand_img = _reader.GetString(3);
                    list.Add(brand);
                }
            }
            return list;
        }


        //Get Brand By ID
        public Brand GetBrandByID(int ID)
        {
            Brand brand = new Brand();
            _command.CommandText = "Select * from Brand where Brand_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    brand.brand_id = _reader.GetInt32(0);
                    brand.brand_name = _reader.GetString(1);
                    brand.isAvailable = _reader.GetBoolean(2);
                    brand.brand_img = _reader.GetString(3);
                }
            }

            return brand;
        }

        //Add Brand
        public void AddBrand(Brand brand)
        {
            _command.CommandText = "INSERT INTO Brand (Brand_Name, isAvailable, Brand_Logo) " +
                      "VALUES (@brand_name, @isAvailable, @brand_logo)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@brand_name", brand.brand_name);
            _command.Parameters.AddWithValue("@isAvailable", 1);
            _command.Parameters.AddWithValue("@brand_logo", brand.brand_img);
            _command.ExecuteNonQuery();
        }

        //Edit Brand
        public void EditBrand(Brand brand)
        {
            _command.CommandText = @"UPDATE Brand
                    SET Brand_Name = @Name, Brand_Logo = @Logo
                    WHERE Brand_ID = @ID;";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Name", brand.brand_name);
            _command.Parameters.AddWithValue("@Logo", brand.brand_img);
            _command.Parameters.AddWithValue("@ID", brand.brand_id);
            _command.ExecuteNonQuery();

        }


        public void EditBrandWithoutImage(Brand brand)
        {
            _command.CommandText = @"UPDATE Brand
                    SET Brand_Name = @Name
                    WHERE Brand_ID = @ID;";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Name", brand.brand_name);
            _command.Parameters.AddWithValue("@ID", brand.brand_id);
            _command.ExecuteNonQuery();

        }

        //------------------------------------------------------------------------------------------------------------



        //------------------------------------------------------------------------------------------------------------
        //START CATEGORY CRUD

        //Get all Category
        public List<Category> GetAllCategory()
        {
            List<Category> list = new List<Category>();
            _command.CommandText = "Select * from Category";
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Category cat = new Category();
                    cat.cate_id = _reader.GetInt32(0);
                    cat.cate_name = _reader.GetString(1);
                    cat.isAvailable = _reader.GetBoolean(2);
                    cat.keyword = _reader.GetString(3);
                    list.Add(cat);
                }
            }
            return list;
        }

        //Add Category to Database
        public void AddCategory(Category cate)
        {
            _command.CommandText = "INSERT INTO Category (Cat_Name, isAvailable, keyword) " +
                      "VALUES (@cat_name, @isAvailable, @keyword)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@cat_name", cate.cate_name);
            _command.Parameters.AddWithValue("@isAvailable", 1);
            _command.Parameters.AddWithValue("@keyword", cate.keyword);
            _command.ExecuteNonQuery();
        }

        //Update Category by ID to Database
        public void EditCategory(Category category)
        {
            _command.CommandText = @"UPDATE Category
                    SET Cat_Name = @Name
                    WHERE Cat_ID = @ID;";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Name", category.cate_name);
            _command.Parameters.AddWithValue("@ID", category.cate_id);
            _command.ExecuteNonQuery();

        }

        //Get Category by ID
        public Category GetCatByID(int ID)
        {
            Category cate = new Category();
            _command.CommandText = "Select * from Category where Cat_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    cate.cate_id = _reader.GetInt32(0);
                    cate.cate_name = _reader.GetString(1);
                    cate.isAvailable = _reader.GetBoolean(2);
                    cate.keyword = _reader.GetString(3);
                }
            }

            return cate;
        }


        //------------------------------------------------------------------------------------------------------------

        public string GetNewProductID(int cate_id)
        {
            _command.CommandText = "SELECT TOP 1 pro_id FROM Product where Cat_ID = @cate_id ORDER BY pro_id DESC";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@cate_id", cate_id);
            string? str = null;
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    str = _reader.GetString(0);
                }
            }

            if (str == null)
            {
                _command.CommandText = "select keyword from Category where Cat_ID = @cate_id";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@cate_id", cate_id);
                using (_reader = _command.ExecuteReader())
                {
                    if (_reader.Read())
                    {
                        str = _reader.GetString(0);
                        int newNumber = 1;
                        string newProductId = $"{str}{newNumber:D3}";
                        return newProductId;
                    }
                }
            }
            else
            {
                string numericPart = str.Substring(2);
                int currentNumber = int.Parse(numericPart);

                // Increment the number
                int newNumber = currentNumber + 1;

                // Format the new product ID
                string newProductId = $"{str.Substring(0, 2)}{newNumber:D3}";

                return newProductId;
            }
            return str;

        }













        /**
         * Method use to get all product have cat_id = 2 ( Mouse)
         */
        public List<Product> getMouse()
        {
            List<Product> listMouse = new List<Product>();
            _command.CommandText = "Select * from Product Where Cat_ID = '2'";
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product pro = new Product();
                    pro.pro_id = _reader.GetString(0);
                    pro.brand_id = _reader.GetInt32(1);
                    pro.cate_id = _reader.GetInt32(2);
                    pro.pro_name = _reader.GetString(3);
                    pro.pro_quan = _reader.GetInt32(4);
                    pro.pro_des = _reader.GetString(5);
                    pro.pro_price = double.Parse(_reader.GetValue(6).ToString());
                    pro.discount = _reader.GetInt32(7);
                    pro.isAvailable = _reader.GetBoolean(8);
                    listMouse.Add(pro);
                }
            }
            foreach (var item in listMouse)
            {
                _command.CommandText = "SELECT * FROM Product_Image WHERE pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_img.Add(_reader.GetString(1));
                    }
                }
            }

            foreach (var item in listMouse)
            {
                _command.CommandText = "Select * from Product_Attribute where pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_attribute[_reader.GetString(1)] = _reader.GetString(2);
                    }
                }
            }
            return listMouse;
        }



        /**
         * Method use to get all product have cat_id = 1 ( Keyboard)
         */
        public List<Product> getAllKeyboard()
        {
            List<Product> listKeyboard = new List<Product>();
            _command.CommandText = "Select * from Product Where Cat_ID = '1'";
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product pro = new Product();
                    pro.pro_id = _reader.GetString(0);
                    pro.brand_id = _reader.GetInt32(1);
                    pro.cate_id = _reader.GetInt32(2);
                    pro.pro_name = _reader.GetString(3);
                    pro.pro_quan = _reader.GetInt32(4);
                    pro.pro_des = _reader.GetString(5);
                    pro.pro_price = double.Parse(_reader.GetValue(6).ToString());
                    pro.discount = _reader.GetInt32(7);
                    pro.isAvailable = _reader.GetBoolean(8);
                    listKeyboard.Add(pro);
                }
            }
            foreach (var item in listKeyboard)
            {
                _command.CommandText = "SELECT * FROM Product_Image WHERE pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_img.Add(_reader.GetString(1));
                    }
                }
            }

            foreach (var item in listKeyboard)
            {
                _command.CommandText = "Select * from Product_Attribute where pro_id = '" + item.pro_id + "'";
                //_command.Parameters.AddWithValue("@pro_id", item.pro_id);
                _command.Parameters.Clear();
                using (_reader = _command.ExecuteReader())
                {
                    while (_reader.Read())
                    {
                        item.pro_attribute[_reader.GetString(1)] = _reader.GetString(2);
                    }
                }
            }
            return listKeyboard;
        }



    }
}
