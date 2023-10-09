using System.Data.SqlClient;

namespace SWP391_Group3_FinalProject.NewFolder
{
    public class DbConnection
    {
        private static string connectionString = "Data Source=UNKEPTHAROLDDPC;Initial Catalog=SWPDatabase;User ID=sa;Password=222";
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
