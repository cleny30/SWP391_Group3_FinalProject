using Microsoft.AspNet.SignalR.Messaging;
using SWP391_Group3_FinalProject.Models;
using SWP391_Group3_FinalProject.NewFolder;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace SWP391_Group3_FinalProject.DAOs
{
    public class ImportRecieptDAO
    {
        private SqlConnection conn;
        private SqlCommand _command;
        private SqlDataReader _reader;

        public ImportRecieptDAO()
        {
            conn = DbConnection.GetConnection();
            _command = new SqlCommand();
            _command.Connection = conn;
        }

        //Add Date, Person-In-Charge and Total Into Database
        public void CreateOrderReciept(Import_Reciept IR, List<Receipt_Product> RP)
        {
            ProductDAO dao = new ProductDAO();
            _command.CommandText = "INSERT INTO Import_Receipt(Date_Import, Person_In_Charge, Payment) " +
                                   "VALUES(@date, @person, @totalprice)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@date", IR.Date_Import);
            _command.Parameters.AddWithValue("@person", IR.Person_In_Charge);
            _command.Parameters.AddWithValue("@totalprice", IR.Payment);
            _command.ExecuteNonQuery();

            foreach (var items in RP)
            {
                _command.CommandText = "INSERT INTO Receipt_Product (Receipt_ID, pro_id, pro_name, amount, price) " +
                             "VALUES ((SELECT MAX(Receipt_ID) FROM Import_Receipt), @pro_id, @pro_name, @amount, @price)";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@pro_id", items.pro_id);
                _command.Parameters.AddWithValue("@pro_name", items.pro_name);
                _command.Parameters.AddWithValue("@amount", items.amount);
                _command.Parameters.AddWithValue("@price", items.price);
                _command.ExecuteNonQuery();
                dao.AddProductQuantity(items.pro_id, items.amount);
            }
        }
    }
}
