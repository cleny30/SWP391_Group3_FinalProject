using System.Data.SqlClient;

namespace SWP391_Group3_FinalProject.NewFolder
{
    public class DbConnection
    {
        private static string connectionString = "Data Source=CLENY\\CLENYSQL;Initial Catalog=SWPDatabase;User ID=sa;Password=12; Max Pool Size=10000;";

        /// <summary>
        /// USED FOR TESTING
        /// Data Source=UNKEPTHAROLDDPC;Initial Catalog=SWPDatabaseV3;User ID=sa;Password=222;
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
