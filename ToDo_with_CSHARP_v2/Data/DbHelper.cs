using System.Data.SqlClient;

namespace ToDo_with_CSHARP_v2.Data
{
    public static class DbHelper
    {
        private static string ConnectionString = "Server=AN515-52\\SQLEXPRESS;Database=ToDoDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection() => new SqlConnection(ConnectionString);
    }
}