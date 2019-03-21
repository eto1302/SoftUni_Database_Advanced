using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyApp.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Salary = table.Column<decimal>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "Birthday", "FirstName", "LastName", "Salary" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georgi", "Georgiev", 12131.44m },
                    { 2, "Neznam", new DateTime(2019, 3, 5, 21, 9, 46, 460, DateTimeKind.Local).AddTicks(1157), "Maria", "Marieva", 999.10m },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alisia", "Alisieva", 11111.11m },
                    { 4, "Neznam2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pesho", "Peshov", 431.44m },
                    { 5, null, new DateTime(2018, 3, 20, 21, 9, 46, 470, DateTimeKind.Local).AddTicks(5225), "Vyara", "Marinova", 2000.44m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
