using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using Microsoft.Owin.BuilderProperties;
using SWP391_Group3_FinalProject.Models;
using SWP391_Group3_FinalProject.NewFolder;
namespace SWP391_Group3_FinalProject.DAOs
{
    public class OrderDAO
    {
        private SqlConnection conn;
        private SqlCommand _command;
        private SqlDataReader _reader;
        public OrderDAO()
        {
            conn = DbConnection.GetConnection();
            _command = new SqlCommand();
            _command.Connection = conn;
        }
        public List<Order> GetOrderByUsername(string username)
        {
            List<Order> list = new List<Order>();
            _command.CommandText = "SELECT * from [Order] where username=@username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Order order = new Order();
                    order.orderId = _reader.GetString(0);
                    order.startDay = _reader.GetDateTime(4);
                    order.status = _reader.GetInt32(7);
                    if (order.status == 4)
                    {
                        order.endDay = _reader.GetDateTime(5);
                    }

                    decimal tp = _reader.GetDecimal(3);
                    order.totalPrice = (double)tp;
                    list.Add(order);
                }
            }
            return list;
        }

        public List<Order> GetAllOrder()
        {
            List<Order> list = new List<Order>();
            _command.CommandText = "SELECT * from [Order]";
            _command.Parameters.Clear();

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Order order = new Order();
                    order.orderId = _reader.GetString(0);
                    if (_reader.IsDBNull(1))
                    {
                        order.staffId = null;
                    }
                    else
                    {
                        order.staffId = _reader.GetInt32(1);
                    }
                    order.username = _reader.GetString(2);
                    order.startDay = _reader.GetDateTime(4);
                    if (_reader.IsDBNull(5))
                    {
                        order.endDay = null;
                    }
                    else
                    {
                        order.endDay = _reader.GetDateTime(5);
                    }
                    order.description = _reader.GetString(6);
                    order.status = _reader.GetInt32(7);

                    decimal tp = _reader.GetDecimal(3);
                    order.totalPrice = (double)tp;
                    list.Add(order);
                }
            }
            return list;
        }

        public List<Tuple<string, double>> GetTop10Customer()
        {
            List<Tuple<string, double>> topCustomers = new List<Tuple<string, double>>();
            _command.CommandText = "SELECT [Customer].Fullname, SUM([Order].Total_Price) AS TotalPriceSum\r\nFROM [Order]\r\nINNER JOIN [Customer] ON [Order].username = [Customer].username\r\nWHERE [Order].[Status] = 4\r\nGROUP BY [Customer].Fullname";
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    string fullname = _reader.GetString(0);
                    double totalspent = (double)_reader.GetDecimal(1);
                    topCustomers.Add(Tuple.Create(fullname, totalspent));
                }
            }
            return topCustomers;
        }

        public List<Tuple<string, double>> AllCustomerMonth()
        {
            List<Tuple<string, double>> topCustomers = new List<Tuple<string, double>>();
            _command.CommandText = "SELECT [Customer].Fullname, SUM([Order].Total_Price) AS TotalPriceSum\r\nFROM [Order]\r\nINNER JOIN [Customer] ON [Order].username = [Customer].username\r\nWHERE [Order].[Status] = 4\r\n  AND MONTH([Order].End_date) = @Month\r\n  AND YEAR([Order].End_date) = @Year\r\nGROUP BY [Customer].Fullname;";
            _command.Parameters.Clear();
            DateTime currentDate = DateTime.Now;

            // Extract the current month and year from the current date.
            int currentMonth = currentDate.Month;
            int currentYear = currentDate.Year;
            _command.Parameters.AddWithValue("@Month", currentMonth);
            _command.Parameters.AddWithValue("@Year", currentYear);
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    string fullname = _reader.GetString(0);
                    double totalspent = (double)_reader.GetDecimal(1);
                    topCustomers.Add(Tuple.Create(fullname, totalspent));
                }
            }
            return topCustomers;
        }

        public List<OrderDetail> GetAllOrderDetail()
        {
            List<OrderDetail> list = new List<OrderDetail>();
            _command.CommandText = "SELECT * from [Order_Details]\r\ninner join [Order]\r\non Order_Details.Order_ID = [Order].Order_ID\r\nwhere [Order].[Status] = 4;";
            _command.Parameters.Clear();

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {

                    OrderDetail order = new OrderDetail();
                    order.orderID = _reader.GetString(0);
                    order.productID = _reader.GetString(1);
                    order.productName = _reader.GetString(2);
                    order.quantity = _reader.GetInt32(3);
                    order.price = (double)_reader.GetDecimal(4);

                    list.Add(order);
                }
            }
            return list;
        }

        public Order_Address GetAllOrderAddressBasedOnID(string ID)
        {
            Order_Address order_Address = new Order_Address();
            _command.CommandText = "SELECT * from [Order_Address] where Order_ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", ID);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    order_Address = new Order_Address
                    {
                        address = _reader.GetString(3),
                        phonenum = _reader.GetString(2),
                        fullname = _reader.GetString(1),
                    };

                }
            }
            return order_Address;
        }

        public List<OrderDetail> GetOrderDetail(string id)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            _command.CommandText = "SELECT * from Order_Details where Order_ID=@Order_ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Order_ID", id);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    OrderDetail order = new OrderDetail();
                    order.orderID = _reader.GetString(0);
                    order.productID = _reader.GetString(1);
                    order.productName = _reader.GetString(2);
                    order.quantity = _reader.GetInt32(3);
                    decimal tp = _reader.GetDecimal(4);
                    order.price = (double)tp;
                    list.Add(order);
                }
            }
            return list;
        }
        public Addresses GetAddressByOrderID(string orderID)
        {
            Addresses a = new Addresses();
            _command.CommandText = "SELECT * from [Order_Address] where Order_ID=@id";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@id", orderID);

            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    a.fullname = _reader.GetString(1);
                    a.phonenum = _reader.GetString(2);
                    a.address = _reader.GetString(3);
                }
            }
            return a;
        }
        public List<Cart> GetCartByUsername(string username)
        {
            List<Cart> list = new List<Cart>();
            _command.CommandText = "SELECT * from Cart where username=@username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Cart cart = new Cart();
                    cart.username = _reader.GetString(0);
                    cart.pro_id = _reader.GetString(1);
                    cart.pro_name = _reader.GetString(2);
                    cart.quantity = _reader.GetInt32(3);
                    cart.price = (double)_reader.GetDecimal(4);
                    list.Add(cart);
                }
            }
            return list;
        }
        public void AddCart(Cart c)
        {
            ProductDAO Pdao = new ProductDAO();
            List<Product> listpro = Pdao.GetAllProduct();

            var pro = listpro.FirstOrDefault(p => p.pro_id == c.pro_id);
            if (pro.pro_quan <= 0 || pro.pro_quan < c.quantity)
            {
                return;
            }

            _command.CommandText = "INSERT INTO Cart(username, pro_id, pro_name, quantity, price) VALUES (@us, @pi, @pn, @q, @p)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@us", c.username);
            _command.Parameters.AddWithValue("@pi", c.pro_id);
            _command.Parameters.AddWithValue("@pn", c.pro_name);
            _command.Parameters.AddWithValue("@q", c.quantity);
            _command.Parameters.AddWithValue("@p", (decimal)c.price);
            _command.ExecuteNonQuery();
        }
        public string CartQuantity(Cart c)
        {
            ProductDAO Pdao = new ProductDAO();
            List<Product> listpro = Pdao.GetAllProduct();

            var quantityInStock = listpro.FirstOrDefault(p => p.pro_id == c.pro_id).pro_quan;
            if (quantityInStock <= 0 || quantityInStock < c.quantity)
            {
                return "Out of Stock!";
            }
            _command.CommandText = "Update Cart SET quantity=@q WHERE username=@us AND pro_id=@pi";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@us", c.username);
            _command.Parameters.AddWithValue("@pi", c.pro_id);
            _command.Parameters.AddWithValue("@q", c.quantity);
            _command.ExecuteNonQuery();
            return "Success";
        }
        public void DeleteCart(string us, string pro_id)
        {
            _command.CommandText = "Delete from Cart WHERE username=@us AND pro_id=@pi";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@us", us);
            _command.Parameters.AddWithValue("@pi", pro_id);
            _command.ExecuteNonQuery();
        }
        public int Checkout(List<Cart> cart, string des, double total, Addresses add)
        {
            if (cart.Count > 0)
            {
                ProductDAO Pdao = new ProductDAO();
                List<Product> listpro = Pdao.GetAllProduct();
                foreach (var item in cart)
                {
                    var quantityInStock = listpro.FirstOrDefault(p => p.pro_id == item.pro_id).pro_quan;
                    if (quantityInStock <= 0 || quantityInStock < item.quantity)
                    {
                        return 0;
                    }
                }
                string OID = GetNewOrderID();
                _command.CommandText = "INSERT INTO [Order](Order_ID, Staff_ID, username, Total_Price, Start_date, End_date, Description, Status) VALUES (@oID, NULL, @us, @total, GETDATE(), NULL, @des, 1)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@oID", OID);
                _command.Parameters.AddWithValue("@us", cart[0].username);
                _command.Parameters.AddWithValue("@total", (float)total);
                _command.Parameters.AddWithValue("@des", des);
                _command.ExecuteNonQuery();

                foreach (var c in cart)
                {
                    var product = listpro.FirstOrDefault(p => p.pro_id == c.pro_id);
                    var quantityUpdate = product.pro_quan - c.quantity;
                    _command.CommandText = "Update Product SET pro_quan=@q WHERE pro_id=@pi";
                    _command.Parameters.Clear();
                    _command.Parameters.AddWithValue("@q", quantityUpdate);
                    _command.Parameters.AddWithValue("@pi", c.pro_id);
                    _command.ExecuteNonQuery();

                    _command.CommandText = "INSERT INTO Order_Details(Order_ID, pro_id, pro_name, quantity, price) VALUES (@oID, @pi, @pn, @q, @p)";
                    _command.Parameters.Clear();
                    _command.Parameters.AddWithValue("@oID", OID);
                    _command.Parameters.AddWithValue("@pi", c.pro_id);
                    _command.Parameters.AddWithValue("@pn", c.pro_name);
                    _command.Parameters.AddWithValue("@q", c.quantity);
                    _command.Parameters.AddWithValue("@p", (decimal)(c.price * c.quantity));
                    _command.ExecuteNonQuery();
                }
                _command.CommandText = "INSERT INTO Order_Address values (@oID, @fn, @pn, @ai)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@oID", OID);
                _command.Parameters.AddWithValue("@fn", add.fullname);
                _command.Parameters.AddWithValue("@pn", add.phonenum);
                _command.Parameters.AddWithValue("@ai", add.address);
                _command.ExecuteNonQuery();

                _command.CommandText = "Delete from Cart WHERE username=@us";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@us", cart[0].username);
                _command.ExecuteNonQuery();
                return 1;
            }
            return 0;
        }
        public string GetNewOrderID()
        {
            _command.CommandText = "SELECT TOP 1 Order_ID FROM [Order] ORDER BY Order_ID DESC";
            _command.Parameters.Clear();
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
                str = "OD";
                int newNumber = 1;
                string newOrderId = $"{str}{newNumber:D3}";
                return newOrderId;
            }
            else
            {
                string numericPart = str.Substring(2);
                int currentNumber = int.Parse(numericPart);

                // Increment the number
                int newNumber = currentNumber + 1;

                // Format the new product ID
                string newOrderId = $"{str.Substring(0, 2)}{newNumber:D3}";

                return newOrderId;
            }
            return str;
        }

        public double GetTotalIncome()
        {
            DateTime currentDate = DateTime.Now;

            // Get the current month as an integer (1 for January, 2 for February, and so on)
            int currentMonth = currentDate.Month;
            int currentYear = currentDate.Year;

            _command.CommandText = "SELECT  SUM(Total_Price) AS Total_Price FROM [Order] WHERE End_date IS NOT NULL";
            _command.Parameters.Clear();

            double totalIncome = 0;
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    totalIncome = _reader.IsDBNull(0) ? 0 : (double)_reader.GetDecimal(0);
                }
            }
            return totalIncome;
        }

        public double GetTotalPayment()
        {
            DateTime currentDate = DateTime.Now;

            // Get the current month as an integer (1 for January, 2 for February, and so on)
            int currentMonth = currentDate.Month;
            int currentYear = currentDate.Year;
            _command.CommandText = "SELECT SUM(Payment) AS TotalPayment FROM [Import_Receipt] ";
            _command.Parameters.Clear();

            double totalPayment = 0;
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    totalPayment = _reader.IsDBNull(0) ? 0 : (double)_reader.GetDecimal(0);
                }
            }
            return totalPayment;
        }

        public List<Tuple<string, int>> GetTotalQuantityOnCateName()
        {
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            _command.CommandText = "SELECT" +
    " c.Cat_Name AS CategoryName," +
    " SUM(od.quantity) AS TotalQuantity" +
    " FROM Category c" +
    " LEFT JOIN [Product] p ON c.Cat_ID = p.Cat_ID" +
    " LEFT JOIN [Order_Details] od ON p.pro_id = od.pro_id" +
    " LEFT JOIN [Order] o ON od.Order_ID = o.Order_ID AND o.End_date IS NOT NULL" +
    " GROUP BY c.Cat_Name";


            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    string name = _reader.GetString(0);

                    int quantity = 0;
                    if (!_reader.IsDBNull(1))
                    {
                        quantity = _reader.GetInt32(1);
                    } else
                    {
                        quantity = 0;
                    }

                    



                    Tuple<string, int> tupple = new Tuple<string, int>(name, quantity);
                    list.Add(tupple);
                }
            }
            return list;
        }

        public List<Tuple<string, double>> GetIncomeForEachMonth()
        {
            List<Tuple<string, double>> list = new List<Tuple<string, double>>();

            // Thêm giá trị từ 1 đến 12 vào danh sách
            for (int month = 1; month <= 12; month++)
            {
                list.Add(new Tuple<string, double>(month.ToString(), 0));
            }

            DateTime currentDate = DateTime.Now;
            int currentYear = currentDate.Year;

            _command.CommandText = "SELECT " +
                "YEAR(End_date) AS OrderYear, " +
                "MONTH(End_date) AS OrderMonth, " +
                "SUM(Total_Price) AS TotalPrice " +
                "FROM [dbo].[Order] " +
                "WHERE YEAR(End_date) = @year AND End_date IS NOT NULL " +
                "GROUP BY YEAR(End_date), MONTH(End_date) " +
                "ORDER BY YEAR(End_date), MONTH(End_date)";

            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@year", currentYear);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    int month = _reader.GetInt32(1);
                    double price = _reader.IsDBNull(2) ? 0 : (double)_reader.GetDecimal(2);

                    // Cập nhật giá trị cho tháng tương ứng
                    list[month - 1] = new Tuple<string, double>(month.ToString(), price);
                }
            }

            return list;
        }



        public List<Tuple<string, double>> GetPaymentForEachMonth()
        {
            List<Tuple<string, double>> list = new List<Tuple<string, double>>();

            // Thêm giá trị từ 1 đến 12 vào danh sách
            for (int month = 1; month <= 12; month++)
            {
                list.Add(new Tuple<string, double>(month.ToString(), 0));
            }

            DateTime currentDate = DateTime.Now;

            // Get the current month as an integer (1 for January, 2 for February, and so on)
            int currentYear = currentDate.Year;

            _command.CommandText = "SELECT " +
    "YEAR(Date_Import) AS ImportYear, " +
    "MONTH(Date_Import) AS ImportMonth, " +
    "SUM(Payment) AS TotalPayment " +
    "FROM [dbo].[Import_Receipt] " +
    "WHERE YEAR(Date_Import) = @year " +
    "GROUP BY YEAR(Date_Import), MONTH(Date_Import) " +
    "ORDER BY ImportYear, ImportMonth;";


            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@year", currentYear);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    int month = _reader.GetInt32(1);
                    double price = _reader.IsDBNull(2) ? 0 : (double)_reader.GetDecimal(2);

                    // Cập nhật giá trị cho tháng tương ứng
                    list[month - 1] = new Tuple<string, double>(month.ToString(), price);
                }
            }

            return list;
        }
        //Change order status to Cancel
        public void CancelOrder(string orderid)
        {
            _command.CommandText = "UPDATE \"Order\" SET Status = 0 WHERE Order_ID = @Order_ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Order_ID", orderid);
            _command.ExecuteNonQuery();
        }
        //Return product to store after cancelling the order
        public void ReturnProduct(string orderid)
        {
            List<OrderDetail> returnProductList = new List<OrderDetail>();
            _command.CommandText = "Select * from Order_Details where Order_ID = @Order_ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Order_ID", orderid);
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    OrderDetail returnProduct = new OrderDetail();
                    returnProduct.orderID = _reader.GetString(0);
                    returnProduct.productID = _reader.GetString(1);
                    returnProduct.productName = _reader.GetString(2);
                    returnProduct.quantity = _reader.GetInt32(3);
                    decimal tp = _reader.GetDecimal(4);
                    returnProduct.price = (double)tp;
                    returnProductList.Add(returnProduct);
                }
            }
            foreach(OrderDetail product in returnProductList)
            {
                _command.CommandText = "Update Product Set pro_quan = pro_quan + @return_quan where pro_id = @pro_id";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@return_quan", product.quantity);
                _command.Parameters.AddWithValue("@pro_id", product.productID);
                _command.ExecuteNonQuery();
            }

        }
        //Change order status to Accept
        public void AcceptOrder(string orderid, int staffid)
        {
            _command.CommandText = "Update \"Order\" Set Status = 2, Staff_ID = @Staff_ID where Order_ID = @Order_ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Staff_ID", staffid);
            _command.Parameters.AddWithValue("@Order_ID", orderid);
            _command.ExecuteNonQuery();
        }
        //Change order status to Ship
        public void ShippedOrder(string orderid)
        {
            _command.CommandText = "Update \"Order\" Set Status = 3, End_date = GETDATE() where Order_ID = @Order_ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Order_ID", orderid);
            _command.ExecuteNonQuery();
        }
        //Change order status to Complete
        public void CompletedOrder(string orderid)
        {
            _command.CommandText = "UPDATE \"Order\" SET Status = 4 WHERE Order_ID = @Order_ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Order_ID", orderid);
            _command.ExecuteNonQuery();
        }
    }


}
