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
            List<string> MinionsNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {

                connection.Open();

                string SelectTownsNamesQuery = @"SELECT Name FROM Minions";

                using (SqlCommand command = new SqlCommand(SelectTownsNamesQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MinionsNames.Add((string)reader["Name"]);
                        }
                    }
                }


            }
            for (int i = 0; i < MinionsNames.Count/2; i++)
            {
                Console.WriteLine(MinionsNames[i]);
                Console.WriteLine(MinionsNames[MinionsNames.Count - 1 - i]);
            }
        }
    }
}