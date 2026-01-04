namespace ToDo_with_CSHARP_v2.EntityCore.DataAccess
{
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; set; } =
            @"Server=AN515 - 52\\SQLEXPRESS;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True";
    }
}