using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;

namespace Project_II
{
    public class DatabaseConnection
    {
        private const string connectionString = "Server=localhost;Database=payments_db;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
