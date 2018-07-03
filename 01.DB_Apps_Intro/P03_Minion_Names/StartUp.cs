using P01_Initial_Setup;
using System;
using System.Data.SqlClient;

namespace P03_Minion_Names
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string villainName = GetVillainName(villainId, connection);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villainName}");
                    PrintNames(villainId, connection);

                }

                connection.Close();
            }
        }

        private static void PrintNames(int villainId, SqlConnection connection)
        {
            string minionsSql = "SELECT Name, Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @id";

            using (SqlCommand command = new SqlCommand(minionsSql, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No Minions !");
                    }
                    else
                    {
                        int row = 1;

                        while (reader.Read())
                        {
                            Console.WriteLine($"1. {reader[0]} -> {reader[1]} years old");
                            row++;
                        }
                    }
                }
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            string nameSql = "SELECT Name FROM Villains WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(nameSql, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);
                return (string)command.ExecuteScalar();
            }
        }
    }
}
