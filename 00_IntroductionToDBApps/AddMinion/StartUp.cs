using System;
using System.Data.SqlClient;
using System.Linq;
using IntroductionToDBApps;

namespace AddMinion
{
    class StartUp
    {

        static void Main(string[] args)
        {
            string[] MinionInput = Console.ReadLine().Split().ToArray();
            string minionName = MinionInput[1];
            int age = int.Parse(MinionInput[2]);
            string town = MinionInput[3];

            string[] VillainInput = Console.ReadLine().Split().ToArray();
            string villainName = VillainInput[1];

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                int? townid = GetTownIdByName(town, connection);

                if(townid == null) AddTown(connection, town);

                townid = GetTownIdByName(town, connection);

                int? villainId = GetVillainIdByName(villainName, connection);

                if (villainId == null) AddVillain(connection, villainName);

                villainId = GetTownIdByName(villainName, connection);

                int? minionId = GetMinionIdByName(minionName, connection);

                if (minionId == null) AddMinion(connection, minionName, age, townid, villainId);
                
            }
        }

        private static void AddVillain(SqlConnection connection, string villainName)
        {
            string insertVillainSql = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
            using (SqlCommand command = new SqlCommand(insertVillainSql, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static int? GetMinionIdByName(string minionName, SqlConnection connection)
        {
            string minionIdQuery = "SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(minionIdQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", minionName);
                return (int?)command.ExecuteScalar();
            }
        }

        private static int? GetVillainIdByName(string villainName, SqlConnection connection)
        {
            string villainIdQuery = "SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(villainIdQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);
                return (int?)command.ExecuteScalar();
            }
        }

        private static void AddMinion(SqlConnection connection, string minionName, int age, int? townId, int? villainId)
        {
            string insertMinionSql = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
            string insertMinionsVillains =
                "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";
            using (SqlCommand command = new SqlCommand(insertMinionSql, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);
                command.ExecuteNonQuery();
            }

            using (SqlCommand command = new SqlCommand(insertMinionsVillains, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", GetMinionIdByName(minionName, connection));
                command.ExecuteNonQuery();
            }
        }

        private static int? GetTownIdByName(string town, SqlConnection connection)
        {
            string townIdQuery = "SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(townIdQuery, connection))
            {
                command.Parameters.AddWithValue("@townName", town);
                return (int?) command.ExecuteScalar();
            }
        }

        private static void AddTown(SqlConnection connection, string town)
        {
            string insertTownSql = "INSERT INTO Towns (Name) VALUES (@townName)";
            using (SqlCommand command = new SqlCommand(insertTownSql, connection))
            {
                command.Parameters.AddWithValue("@townName", town);
                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Town {town} was added to the database.");
        }
    }
}