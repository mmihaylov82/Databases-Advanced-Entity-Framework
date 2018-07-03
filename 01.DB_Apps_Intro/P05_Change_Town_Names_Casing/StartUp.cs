using P01_Initial_Setup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace P05_Change_Town_Names_Casing
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                int countryId = GetCountryId(countryName, connection);

                if (countryId == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    int affectedRows = UpdateName(countryId, connection);
                    List<string> names = GetNames(countryId, connection);
                    Console.WriteLine($"{affectedRows} town names were affected. ");
                    Console.WriteLine($"[{String.Join(", ", names)}]");
                }

                connection.Close();
            }
        }

        private static List<string> GetNames(int countryId, SqlConnection connection)
        {
            List<string> names = new List<string>();
            string namesSql = "SELECT Name FROM Towns WHERE CountryCode = @countryId";

            using (SqlCommand command = new SqlCommand(namesSql, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        names.Add((string)reader[0]);
                    }
                }
            }

            return names;
        }

        private static int UpdateName(int countryId, SqlConnection connection)
        {
            string updateStatements = "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @CountryId";

            using (SqlCommand command = new SqlCommand(updateStatements, connection))
            {
                command.Parameters.AddWithValue("@CountryId", countryId);
                return command.ExecuteNonQuery();
            }
        }

        private static int GetCountryId(string countryName, SqlConnection connection)
        {
            string countryInfo = "SELECT TOP(1) c.Id FROM Towns AS t JOIN Countries AS c ON c.Id = t.CountryCode WHERE c.Name = @name";

            using (SqlCommand command = new SqlCommand(countryInfo, connection))
            {
                command.Parameters.AddWithValue("@name", countryName);

                if (command.ExecuteScalar() == null)
                {
                    return 0;
                }

                return (int)command.ExecuteScalar();
            }
        }
    }
}
