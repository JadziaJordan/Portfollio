using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace xact_ERP_Second.Data
{
    /// <summary>
    /// This class manages a single reusable SQL connection
    /// </summary>
    public static class Database
    {
        private static string _connectionString;

        /// <summary>
        /// Initialize the connection string from appsettings.json
        /// Call this once at app startup
        /// </summary>
        public static void Initialize()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _connectionString = config.GetConnectionString("ExactErpDemo");

            if (string.IsNullOrEmpty(_connectionString))
                throw new Exception("Connection string not found in appsettings.json");
        }

        /// <summary>
        /// Returns a new SQL connection
        /// Use with 'using' to auto-close
        /// </summary>
        public static SqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new Exception("Database not initialized. Call Database.Initialize() first.");

            return new SqlConnection(_connectionString);
        }
    }
}
