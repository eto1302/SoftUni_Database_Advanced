using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using IntroductionToDBApps;

namespace IncreaseAgeStoredProcedure
{
    class StartUp
    {

        static void Main(string[] args)
        {
            List<string> ResultList = new List<string>();
            string execProcedureQuery = @"EXEC usp_GetOlder @id";

            string SelectMinionQuery = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

            int id = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                
                SqlCommand command = new SqlCommand(execProcedureQuery, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();


                SqlCommand SelectMinionCommand = new SqlCommand(SelectMinionQuery, connection);
                SelectMinionCommand.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = SelectMinionCommand.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                       Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old"); 
                    }
                    
                }
            }
            
        }
    }
}

