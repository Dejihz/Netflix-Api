using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace project6._1Api.Entities
{
    public class db
    {
        private static string  _connectionString;

        // Constructor to inject IConfiguration
        public db(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("myDb1");
        }



      public static bool DeleteUser(int user_id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("DeleteUserById", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", user_id);

                    conn.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("User Deleted successfully.");
                    conn.Close();
                    return true;
                }
            }
        }

       private static void AddSuperuser(SqlConnection connection)
        {
            using (var command = new SqlCommand("CheckAndInsertDummyUsers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Dummy users added successfully.");
                connection.Close();
            }
        }

      

        public static void UserFaker()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                    AddSuperuser(conn);
            }
        }
    }
}