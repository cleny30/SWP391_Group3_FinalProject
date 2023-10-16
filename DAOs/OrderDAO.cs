using System.Data.SqlClient;
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
            _command.CommandText = "SELECT o.Order_ID, o.Start_date, o.End_date, os.Status, o.Total_Price " +
                "FROM [Order] AS o " +
                "INNER JOIN Order_status AS os ON o.Order_ID = os.Order_ID " +
                "WHERE o.username = @username;";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);

            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Order order = new Order();
                    order.orderId = _reader.GetString(0);
                    order.startDay = _reader.GetDateTime(1);
                    order.endDay = _reader.GetDateTime(2);
                    order.status = _reader.GetString(3);
                    decimal tp= _reader.GetDecimal(4);
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
                    order.username = _reader.GetString(2);
                    order.productName = _reader.GetString(3);
                    order.quantity = _reader.GetInt32(4);
                    decimal tp = _reader.GetDecimal(5);
                    order.price = (double)tp;
                    list.Add(order);
                }
            }
            return list;
        }
    }
}
