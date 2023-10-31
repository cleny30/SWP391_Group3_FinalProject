using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using SWP391_Group3_FinalProject.Models;
using SWP391_Group3_FinalProject.NewFolder;

namespace SWP391_Group3_FinalProject.DAOs
{
    public class ManagerDAO
    {
        private SqlConnection conn;
        private SqlCommand _command;
        private SqlDataReader _reader;
        public ManagerDAO()
        {
            conn = DbConnection.GetConnection();
            _command = new SqlCommand();
            _command.Connection = conn;
        }
        //Get a full list of manager
        public List<Manager> GetAllManagers()
        {
            _command.CommandText = "Select * from Manager";
            _command.Parameters.Clear();
            List<Manager> list = new List<Manager>();
            using (_reader = _command.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Manager manager = new Manager();
                    manager.ID = _reader.GetInt32(0);
                    manager.username = _reader.GetString(1);
                    manager.email = _reader.GetString(2);
                    manager.password = _reader.GetString(3);
                    manager.fullname = _reader.GetString(4);
                    manager.SSN = _reader.GetString(5);
                    manager.address = _reader.GetString(6);
                    manager.phone = _reader.GetString(7);
                    manager.isAdmin = _reader.GetBoolean(8);
                    manager.isAvailable = _reader.GetBoolean(9);
                    list.Add(manager);
                }
            }
            return list;
        }

        //Get manager using username
        public Manager GetManagerByUsername(string username)
        {
            _command.CommandText = "Select * from Manager where username = @username";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", username);
            Manager manager = new Manager();
            using (_reader = _command.ExecuteReader())
            {
                if( _reader.Read())
                {
                    manager.ID = _reader.GetInt32(0);
                    manager.username = _reader.GetString(1);
                    manager.email = _reader.GetString(2);
                    manager.password = _reader.GetString(3);
                    manager.fullname = _reader.GetString(4);
                    manager.SSN = _reader.GetString(5);
                    manager.address = _reader.GetString(6);
                    manager.phone = _reader.GetString(7);
                    manager.isAdmin = _reader.GetBoolean(8);
                    manager.isAvailable = _reader.GetBoolean(9);
                    return manager;
                }
            }
                return null;
        }

        //Get manager using email
        public Manager GetManagerByEmail(string email)
        {
            _command.CommandText = "Select * from Manager where email = @email";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@email", email);
            Manager manager = new Manager();
            using (_reader = _command.ExecuteReader())
            {
                if (_reader.Read())
                {
                    manager.ID = _reader.GetInt32(0);
                    manager.username = _reader.GetString(1);
                    manager.email = _reader.GetString(2);
                    manager.password = _reader.GetString(3);
                    manager.fullname = _reader.GetString(4);
                    manager.SSN = _reader.GetString(5);
                    manager.address = _reader.GetString(6);
                    manager.phone = _reader.GetString(7);
                    manager.isAdmin = _reader.GetBoolean(8);
                    manager.isAvailable = _reader.GetBoolean(9);
                    return manager;
                }
            }
            return null;
        }

        //Disable staff
        public void DisableStaff(int staffid)
        {
            _command.CommandText = "Update Manager Set isAvailable = 'false' where ID = @ID";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@ID", staffid);
            _command.ExecuteNonQuery();
        }
        //Add manager
        public void AddManager(Manager manager)
        {
            _command.CommandText = "INSERT INTO Manager(username, Email, password, Fullname, SSN, Living_Address, Phone_Num, isAdmin, isAvailable) values(@username, @email, @password, @fullname, @SSN, @address, @phone, @isAdmin, @isAvailable)";
            _command.Parameters.Clear();
            _command.Parameters.AddWithValue("@username", manager.username);
            _command.Parameters.AddWithValue("@email", manager.email);
            _command.Parameters.AddWithValue("@password", CalculateMD5Hash(manager.password));
            _command.Parameters.AddWithValue("@fullname", manager.fullname);
            _command.Parameters.AddWithValue("@SSN", manager.SSN);
            _command.Parameters.AddWithValue("@address", manager.address);
            _command.Parameters.AddWithValue("@phone", manager.phone);
            _command.Parameters.AddWithValue("@isAdmin", manager.isAdmin);
            _command.Parameters.AddWithValue("@isAvailable", true);
            _command.ExecuteNonQuery();
        }
        static string CalculateMD5Hash(string input)
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
