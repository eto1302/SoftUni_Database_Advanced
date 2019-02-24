using System;
using System.Data.SqlClient;
using IntroductionToDBApps;

namespace VillainNames
{
    class StartUp
    {

        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                
                connection.Open();
                string villainNameQuery = "SELECT Name FROM Villains WHERE Id = @Id";
                


                using (SqlCommand command = new SqlCommand(villainNameQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", villainId);
                    string villainName = (string) command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villainName}");
                }

                string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                using (SqlCommand command = new SqlCommand(minionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", villainId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long rowNumber = (long) reader["RowNum"];
                            string name = (string)reader["Name"];
                            int age = (int) reader["Age"];

                            Console.WriteLine($"{rowNumber}. {name} {age}");
                        }
                    }
                }

            }


        }


    }
}