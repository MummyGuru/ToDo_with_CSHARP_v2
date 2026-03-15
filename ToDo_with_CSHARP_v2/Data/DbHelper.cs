using System.Data.SqlClient;
using Dapper;

public static class DbHelper
{
    private static string ConnectionString = "Server=AN515-52\\SQLEXPRESS;Database=ToDoDB;Trusted_Connection=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}