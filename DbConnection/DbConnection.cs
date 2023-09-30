using System.Data.SqlClient;

namespace SWP391_Group3_FinalProject.NewFolder
{
    public class DbConnection
    {
        private static string connectionString = "Data Source=MSI;Initial Catalog=SWPDatabase;User ID=sa;Password=baso0939105522";
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
