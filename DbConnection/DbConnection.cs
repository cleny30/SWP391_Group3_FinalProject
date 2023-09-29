using System.Data.SqlClient;

namespace SWP391_Group3_FinalProject.NewFolder
{
    public class DbConnection
    {
        private static string connectionString = "Data Source=CLENY\\CLENYSQL;Initial Catalog=SWPDatabase;Persist Security Info=True;User ID=sa;Password=12";
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
