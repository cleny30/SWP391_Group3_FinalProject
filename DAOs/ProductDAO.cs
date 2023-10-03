using SWP391_Group3_FinalProject.Models;
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
                    list.Add(cat);
                }
            }
            return list;
        }

        //Add Category to Database
        public void AddCategory(Category cate)
        {
            _command.CommandText = "INSERT INTO Category (Cat_Name, isAvailable) " +
                      "VALUES (@cat_name, @isAvailable)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@cat_name", cate.cate_name);
            _command.Parameters.AddWithValue("@isAvailable", 1);
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
                while (_reader.Read())
                {
                    cate.cate_id = _reader.GetInt32(0);
                    cate.cate_name = _reader.GetString(1);
                    cate.isAvailable = _reader.GetBoolean(2);
                }
            }

            return cate;
        }


        //------------------------------------------------------------------------------------------------------------




















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
