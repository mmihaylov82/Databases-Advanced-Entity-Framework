using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P09_Increase_Age_Stored_Procedure
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(Configuration.ConnectionString);

            connection.Open();

            int id = int.Parse(Console.ReadLine());

            using (connection)
            {
                SqlCommand command = new SqlCommand("EXEC usp_GetOlder @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT * FROM Minions WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                using (reader)
                {
                    reader.Read();

                    Console.WriteLine($"{(string)reader["Name"]} - {(int)reader["Age"]} years old");
                }
            }
        }
    }
}
