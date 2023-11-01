using System.Data.SqlClient;
using System.Net;
using System.Security.Principal;
using SWP391_Group3_FinalProject.Models;
using SWP391_Group3_FinalProject.NewFolder;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

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
                _command.Parameters.AddWithValue("@password", CalculateMD5Hash(password));
                using (_reader = _command.ExecuteReader())
                {
                    Manager manager = new Manager();
                    if (_reader.Read())
                    {
                        manager.ID = _reader.GetInt32(0);
                        manager.username = _reader.GetString(1);
                        manager.password = _reader.GetString(3);
                        manager.fullname = _reader.GetString(4);
                        manager.phone = _reader.GetString(7);
                        manager.email = _reader.GetString(2);
                        manager.SSN = _reader.GetString(5);
                        manager.address = _reader.GetString(6);
                        manager.isAdmin = _reader.GetBoolean(8);
                        manager.isAvailable = _reader.GetBoolean(9);
                        return manager;
                    }
                }
                return null;
            }
        }
        public List<Customer> GetAllCustomers()
        {
            _command.CommandText = "Select * from Customer";
            _command.Parameters.Clear();
            List<Customer> list = new List<Customer>();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Customer cus = new Customer();
                    cus.username = _reader.GetString(0);
                    cus.password = _reader.GetString(1);
                    cus.fullname = _reader.GetString(2);
                    cus.phone = _reader.GetString(3);
                    cus.email = _reader.GetString(4);
                    list.Add(cus);
                }
            }
            return list;
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
                        manager.ID = _reader.GetInt32(0);
                        manager.username = _reader.GetString(1);
                        manager.password = _reader.GetString(3);
                        manager.fullname = _reader.GetString(4);
                        manager.phone = _reader.GetString(7);
                        manager.email = _reader.GetString(2);
                        manager.SSN = _reader.GetString(5);
                        manager.address = _reader.GetString(6);
                        manager.isAdmin = _reader.GetBoolean(8);
                        manager.isAvailable = _reader.GetBoolean(9);
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
            _command.Parameters.AddWithValue("@password", CalculateMD5Hash(password));
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
                while (_reader.Read())
                {
                    Addresses add = new Addresses();
                    add.address = _reader.GetString(1);
                    add.fullname = _reader.GetString(2);
                    add.phonenum = _reader.GetString(3);
                    add.ID = _reader.GetInt32(4);
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
                while (_reader.Read())
                {
                    Addresses add = new Addresses();
                    add.address = _reader.GetString(1);
                    add.fullname = _reader.GetString(2);
                    add.phonenum = _reader.GetString(3);
                    add.ID = _reader.GetInt32(4);
                    list.Add(add);
                }
            }
            return list;
        }
        public void UpdateAddress(Addresses address)
        {
            _command.CommandText = "UPDATE Delivery_Address SET Address_Information=@ad, Fullname=@fn, Phone_Num=@pn where ID=@id";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ad", address.address);
            _command.Parameters.AddWithValue("@fn", address.fullname);
            _command.Parameters.AddWithValue("@pn", address.phonenum);
            _command.Parameters.AddWithValue("@id", address.ID);
            _command.ExecuteNonQuery();
        }

        public int? AddAddress(Addresses address, string username)
        {
            _command.CommandText = "INSERT INTO Delivery_Address (username, Address_Information, fullname, Phone_Num)VALUES(@us, @ai, @fn, @pn)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@us", username);
            _command.Parameters.AddWithValue("@ai", address.address);
            _command.Parameters.AddWithValue("@fn", address.fullname);
            _command.Parameters.AddWithValue("@pn", address.phonenum);
            _command.ExecuteNonQuery();

            _command.CommandText = "SELECT TOP 1 ID FROM Delivery_Address where username = @us ORDER BY ID DESC";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@us", username);

            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    return _reader.GetInt32(0);
                }
            }
            return null;
        }
        public void DeleteAddress(int id)
        {
            _command.CommandText = "Delete from Delivery_Address where ID=@id";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@id", id);
            _command.ExecuteNonQuery();
        }
        public void AddCustomer(Customer customer)
        {
            _command.CommandText = "INSERT INTO Customer(username, password, fullname, email, phone_num) values(@username, @password, @fullname, @email, @phone_num)  ";
            _command.Parameters.AddWithValue("@username", customer.username);
            _command.Parameters.AddWithValue("@password", CalculateMD5Hash(customer.password));
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
            _command.Parameters.AddWithValue("@pw", CalculateMD5Hash(pw));
            _command.Parameters.AddWithValue("@username", username);
            _command.ExecuteNonQuery();
        }

        public void ForgetPass(string email, string pw)
        {
            AccountDAO dao = new AccountDAO();
            _command.CommandText = "UPDATE Customer SET password=@pw where email=@email";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@pw", CalculateMD5Hash(pw));
            _command.Parameters.AddWithValue("@email", email);
            _command.ExecuteNonQuery();
        }

        public int CheckEmail(string email)
        {
            _command.CommandText = "Select * from Customer where email= @email";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@email", email);
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    return 1;
                }
            }

            _command.CommandText = "Select * from Manager where email= @email";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@email", email);
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    return 1;
                }
            }

            return 0;
        }
        public string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
