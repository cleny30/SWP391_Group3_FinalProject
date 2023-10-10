using System.Data.SqlClient;
using System.Security.Principal;
using SWP391_Group3_FinalProject.Models;
using SWP391_Group3_FinalProject.NewFolder;

namespace SWP391_Group3_FinalProject.DAOs
{
    public class AccountDAO
    {
        private SqlConnection conn;
        private SqlCommand _command;
        private SqlDataReader _reader;
        public AccountDAO()
        {
            conn = DbConnection.GetConnection();
            _command = new SqlCommand();
            _command.Connection = conn;
        }
        public Account GetAccount(String username, String password)
        {
            _command.CommandText = "Select * from account where username= @username and password = @password";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@password", password);
            using (_reader = _command.ExecuteReader())
            {
                Account acc = new Account();
                if (_reader.Read())
                {
                    acc.username = _reader.GetString(0);
                    acc.password = _reader.GetString(1);
                    acc.role = _reader.GetInt32(2);
                    return acc;
                }
            }
            return null;
        }
        public AdminAndStaff GetAdminAndStaff(String username, int role)
        {
            if (role == 0)
            {
                _command.CommandText = "Select * from Admin where username= @username";
            }
            else
            {
                _command.CommandText = "Select * from Staff where username= @username";
            }
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            using (_reader = _command.ExecuteReader())
            {
                AdminAndStaff adminandstaff = new AdminAndStaff();
                if (_reader.Read())
                {
                    adminandstaff.username = _reader.GetString(1);
                    adminandstaff.fullname = _reader.GetString(2);
                    adminandstaff.email = _reader.GetString(3);
                    adminandstaff.SSN = _reader.GetString(4);
                    adminandstaff.address = _reader.GetString(5);
                    adminandstaff.phone = _reader.GetString(6);
                    return adminandstaff;
                }
            }
            return null;
        }
        public Customer GetCustomer(String username)
        {
            _command.CommandText = "Select * from Customer where username= @username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            using (_reader = _command.ExecuteReader())
            {
                Customer customer = new Customer();
                if (_reader.Read())
                {
                    customer.username = _reader.GetString(1);
                    customer.fullname = _reader.GetString(2);
                    customer.phone = _reader.GetString(3);
                    customer.email = _reader.GetString(4);
                    return customer;
                }
            }
            return null;
        }
        public void AddCustomer(String username, String password, String fullname, String email, String phone_num)
        {
            _command.CommandText = "INSERT INTO Account(username, password, Role_ID) values(@username,@password, 2)  ";
            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@password", password);
            _command.ExecuteNonQuery();

            _command.CommandText = "INSERT INTO Customer(username, fullname, email, phone_num) values(@username, @fullname, @email, @phone_num)  ";
            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@fullname", fullname);
            _command.Parameters.AddWithValue("@email", email);
            _command.Parameters.AddWithValue("@phone_num", phone_num);
            _command.ExecuteNonQuery();
        }
    }
}
