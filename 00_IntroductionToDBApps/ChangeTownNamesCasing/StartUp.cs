using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using IntroductionToDBApps;

namespace VillainNames
{
    class StartUp
    {

        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {

                connection.Open();

                string updateTownNamesQuery = @"UPDATE Towns
                                                SET Name = UPPER(Name)
                                                WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                string selectTownNamesQuery = @" SELECT t.Name 
                                                 FROM Towns as t
                                                 JOIN Countries AS c ON c.Id = t.CountryCode
                                                 WHERE c.Name = @countryName";


                using (SqlCommand command = new SqlCommand(updateTownNamesQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);
                    int rows = command.ExecuteNonQuery();
                    if (rows == 0) Console.WriteLine("No town names were affected.");
                    else
                    {
                        Console.WriteLine($"{rows} town names were affected.");
                        List<string> names = new List<string>();
                        using (SqlCommand getNamesCommand = new SqlCommand(selectTownNamesQuery, connection))
                        {
                            getNamesCommand.Parameters.AddWithValue("@countryName", countryName);

                            using (SqlDataReader reader = getNamesCommand.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    names.Add((string)reader["Name"]);
                                }
                            }

                        }

                        Console.WriteLine($"[{string.Join(", ", names)}]");
                    }
                }
            }
        }
    }
}