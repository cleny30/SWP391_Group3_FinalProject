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

        //Get All Import Reciepts
        public List<Import_Reciept> GetAllImportReceipt() {
        List<Import_Reciept> import_Reciepts = new List<Import_Reciept>();
            _command.CommandText = "Select * from Import_Receipt";
            _command.Parameters.Clear();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Import_Reciept IR = new Import_Reciept();
                    IR.Reciept_ID = _reader.GetInt32(0);
                    IR.Person_In_Charge = _reader.GetString(2);
                    decimal payment = _reader.GetDecimal(3);
                    IR.Date_Import = _reader.GetDateTime(1);
                    IR.Payment = (int)payment;

                    import_Reciepts.Add(IR);
                }
            }

            return import_Reciepts;
        }

        public async Task<Import_Reciept> GetImportReceiptByID(int ID)
        {
            Import_Reciept import_Reciepts = new Import_Reciept();

            try
            {
                _command.CommandText = "Select * from Import_Receipt where Receipt_ID = @ID";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@ID", ID);

                using (_reader = await _command.ExecuteReaderAsync())
                {
                    while (await _reader.ReadAsync())
                    {
                        import_Reciepts.Reciept_ID = _reader.GetInt32(0);
                        import_Reciepts.Person_In_Charge = _reader.GetString(1);
                        decimal payment = _reader.GetDecimal(2);
                        import_Reciepts.Date_Import = _reader.GetDateTime(3);
                        import_Reciepts.Payment = (int)payment;
                    }
                }
            }
            finally
            {
                // Ensure that the connection is closed, even in case of exceptions
                if (_reader != null && !_reader.IsClosed)
                {
                    _reader.Close();
                }
            }

            return import_Reciepts;
        }


        public async Task<List<Receipt_Product>> GetRPByID(int ID)
        {
            List<Receipt_Product> list = new List<Receipt_Product>();

            try
            {
                _command.CommandText = "Select * from Receipt_Product where Receipt_ID = @ID";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@ID", ID);

                using (_reader = await _command.ExecuteReaderAsync())
                {
                    while (await _reader.ReadAsync())
                    {
                        Receipt_Product RP = new Receipt_Product();
                        RP.pro_id = _reader.GetString(1);
                        RP.pro_name = _reader.GetString(2);
                        RP.amount = _reader.GetInt32(3);
                        var payment = _reader.GetDecimal(4);
                        RP.price = (int)payment;
                        list.Add(RP);
                    }
                }
            }
            finally
            {
                // Ensure that the connection is closed, even in case of exceptions
                if (_reader != null && !_reader.IsClosed)
                {
                    _reader.Close();
                }
            }

            return list;
        }




    }
}
