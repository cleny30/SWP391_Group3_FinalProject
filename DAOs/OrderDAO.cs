using System.Data.SqlClient;
using System.Net;
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
                        order.staffId = "Not Available";
                    }
                    else
                    {
                        order.staffId = _reader.GetString(1);
                    }
                    order.username = _reader.GetString(2);
                    order.startDay= _reader.GetDateTime(4);
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

            var quantityInStock = listpro.FirstOrDefault(p => p.pro_id == c.pro_id).pro_quan;
            if (quantityInStock <= 0 || quantityInStock < c.quantity)
            {
                return;
            }
            _command.CommandText = "INSERT INTO Cart(username, pro_id, pro_name, quantity, price) VALUES (@us, @pi, @pn, @q, @p)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@us", c.username);
            _command.Parameters.AddWithValue("@pi", c.pro_id);
            _command.Parameters.AddWithValue("@pn", c.pro_name);
            _command.Parameters.AddWithValue("@q", c.quantity);
            _command.Parameters.AddWithValue("@p", c.price);
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
        public void Checkout(List<Cart> cart, string des, double total, Addresses add)
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
                        return;
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

                    if (product.discount > 0)
                    {
                        c.price = c.price - ((product.discount * c.price) / 100);
                    }
                    _command.CommandText = "INSERT INTO Order_Details(Order_ID, pro_id, pro_name, quantity, price) VALUES (@oID, @pi, @pn, @q, @p)";
                    _command.Parameters.Clear();
                    _command.Parameters.AddWithValue("@oID", OID);
                    _command.Parameters.AddWithValue("@pi", c.pro_id);
                    _command.Parameters.AddWithValue("@pn", c.pro_name);
                    _command.Parameters.AddWithValue("@q", c.quantity);
                    _command.Parameters.AddWithValue("@p", c.price * c.quantity);
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
            }
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
    }
}
