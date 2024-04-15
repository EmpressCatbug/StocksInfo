using MySql.Data.MySqlClient;
using System.Data;

namespace StocksInfo.Repository
{
    public class DatabaseManager
    {
        private readonly string connectionString;

        public DatabaseManager()
        {
            DotNetEnv.Env.Load();

            // Build the connection string from environment variables
            connectionString = $"Server={System.Environment.GetEnvironmentVariable("MYSQLHOST")}; " +
                               $"Port={System.Environment.GetEnvironmentVariable("MYSQLPORT")}; " +
                               $"Database={System.Environment.GetEnvironmentVariable("MYSQLDATABASE")}; " +
                               $"Uid={System.Environment.GetEnvironmentVariable("MYSQLUSER")}; " +
                               $"Pwd={System.Environment.GetEnvironmentVariable("MYSQLPASSWORD")};";
        }

        // Method to execute a query and return the result as a DataTable
        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                conn.Close();
            }
            return dt;
        }
    }
}