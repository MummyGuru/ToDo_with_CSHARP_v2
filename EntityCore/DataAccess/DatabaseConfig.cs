namespace EntityCore.DataAccess
{
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; set; } =
            @"Server=(localhost)\SQLEXPRESS;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True";
    }
}