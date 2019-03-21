using System;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Data
{
    public class EmployeesMappingContext : DbContext
    {
        public EmployeesMappingContext(DbContextOptions options)
            : base(options)
        {
        }

        public EmployeesMappingContext()
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=EmployeesDatabase;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Georgi", LastName = "Georgiev", Salary = 12131.44M },
                new Employee { Id = 2, FirstName = "Maria", LastName = "Marieva", Salary = 999.10M, Birthday = DateTime.Now.AddDays(-15), Address = "Neznam" },
                new Employee { Id = 3, FirstName = "Alisia", LastName = "Alisieva", Salary = 11111.11M },
                new Employee { Id = 4, FirstName = "Pesho", LastName = "Peshov", Salary = 431.44M, Address = "Neznam2" },
                new Employee { Id = 5, FirstName = "Vyara", LastName = "Marinova", Salary = 2000.44M, Birthday = DateTime.Now.AddDays(-365) }
            );

        }
    }
}
