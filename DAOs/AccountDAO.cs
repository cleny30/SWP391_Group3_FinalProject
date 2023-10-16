using System.Data.SqlClient;
using System.Net;
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
        public Manager GetManager(string username, string password)
        {
            {
                _command.CommandText = "Select * from Manager where username= @username and password = @password";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@username", username);
                _command.Parameters.AddWithValue("@password", password);
                using (_reader = _command.ExecuteReader())
                {
                    Manager manager = new Manager();
                    if (_reader.Read())
                    {
                        manager.username = _reader.GetString(1);
                        manager.password = _reader.GetString(2);
                        manager.fullname = _reader.GetString(3);
                        manager.phone = _reader.GetString(4);
                        manager.email = _reader.GetString(5);
                        manager.SSN = _reader.GetString(6);
                        manager.address = _reader.GetString(7);
                        manager.isAdmin = _reader.GetBoolean(8);
                        return manager;
                    }
                }
                return null;
            }
        }

        public Manager GetManagerByUsername(string username)
        {
            {
                _command.CommandText = "Select * from Manager where username= @username";
                _command.Parameters.Clear();
                _command.Parameters.AddWithValue("@username", username);
                using (_reader = _command.ExecuteReader())
                {
                    Manager manager = new Manager();
                    if (_reader.Read())
                    {
                        manager.username = _reader.GetString(1);
                        manager.password = _reader.GetString(2);
                        manager.fullname = _reader.GetString(3);
                        manager.phone = _reader.GetString(4);
                        manager.email = _reader.GetString(5);
                        manager.SSN = _reader.GetString(6);
                        manager.address = _reader.GetString(7);
                        manager.isAdmin = _reader.GetBoolean(8);
                        return manager;
                    }
                }
                return null;
            }
        }

        public Customer GetCustomer(string username, string password)
        {
            _command.CommandText = "Select * from Customer where username= @username and password= @password";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            _command.Parameters.AddWithValue("@password", password);
            using (_reader = _command.ExecuteReader())
            {
                Customer customer = new Customer();
                if (_reader.Read())
                {
                    customer.username = _reader.GetString(0);
                    customer.password = _reader.GetString(1);
                    customer.fullname = _reader.GetString(2);
                    customer.phone = _reader.GetString(3);
                    customer.email = _reader.GetString(4);
                    return customer;
                }
            }
            return null;
        }

        public Customer GetCustomerByUsername(string username)
        {
            Customer customer = new Customer();
            _command.CommandText = "Select * from Customer where username= @username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    customer.username = _reader.GetString(0);
                    customer.password = _reader.GetString(1);
                    customer.fullname = _reader.GetString(2);
                    customer.phone = _reader.GetString(3);
                    customer.email = _reader.GetString(4);
                }
            }

            List<Addresses> list = new List<Addresses>();
            _command.CommandText = "Select * from Delivery_Address where username= @username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            using (_reader = _command.ExecuteReader())
            {
                Addresses add = new Addresses();
                while (_reader.Read())
                {
                    add.address = _reader.GetString(1);
                    add.fullname = _reader.GetString(2);
                    add.phonenum = _reader.GetString(3);
                    list.Add(add);
                }
            }
            if (list != null)
            {
                customer.addresses = list;
            }
            return customer;
        }

        public List<Addresses> GetCustomerAddress(string username)
        {
            List<Addresses> list = new List<Addresses>();
            _command.CommandText = "Select * from Delivery_Address where username= @username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            using (_reader = _command.ExecuteReader())
            {
                Addresses add = new Addresses();
                while (_reader.Read())
                {
                    add.address = _reader.GetString(1);
                    add.fullname = _reader.GetString(2);
                    add.phonenum = _reader.GetString(3);
                    list.Add(add);
                }
            }
            return list;
        }

        public void AddCustomer(Customer customer)
        {
            _command.CommandText = "INSERT INTO Customer(username, password, fullname, email, phone_num) values(@username, @password, @fullname, @email, @phone_num)  ";
            _command.Parameters.AddWithValue("@username", customer.username);
            _command.Parameters.AddWithValue("@password", customer.password);
            _command.Parameters.AddWithValue("@fullname", customer.fullname);
            _command.Parameters.AddWithValue("@email", customer.email);
            _command.Parameters.AddWithValue("@phone_num", customer.phone);
            _command.ExecuteNonQuery();
        }

        public void UpdateCustomer(Customer acc)
        {
            AccountDAO dao = new AccountDAO();
            _command.CommandText = "UPDATE Customer SET Fullname=@Fullname,Email= @Email,Phone_Num= @Phone_Num where username=@username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@Fullname", acc.fullname);
            _command.Parameters.AddWithValue("@Email", acc.email);
            _command.Parameters.AddWithValue("@Phone_Num", acc.phone);
            _command.Parameters.AddWithValue("@username", acc.username);
            _command.ExecuteNonQuery();
        }
        public void ChangePassword(string username, string pw)
        {
            AccountDAO dao = new AccountDAO();
            _command.CommandText = "UPDATE Customer SET password=@pw where username=@username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@pw", pw);
            _command.Parameters.AddWithValue("@username", username);
            _command.ExecuteNonQuery();
        }
    }
}
