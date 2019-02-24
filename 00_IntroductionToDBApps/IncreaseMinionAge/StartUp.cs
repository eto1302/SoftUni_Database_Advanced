using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using IntroductionToDBApps;

namespace VillainNames
{
    class StartUp
    {

        static void Main(string[] args)
        {
            List<string> ResultList = new List<string>();
            string UpdateMinionsQuery = @" UPDATE Minions
                                                  SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                  WHERE Id = @Id";

            string SelectMinionsQuery = @"SELECT Name, Age FROM Minions";

            int[] ids = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                foreach (var id in ids)
                {
                    using (SqlCommand command = new SqlCommand(UpdateMinionsQuery, connection))
                    {

                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteScalar();

                    }
                }

                using (SqlCommand command = new SqlCommand(SelectMinionsQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ResultList.Add($"{reader["Name"]} {reader["Age"]}");
                        }
                    }
                }

            }

            foreach (var result in ResultList)
            {
                Console.WriteLine(result);
            }
        }
    }
}