using System;
using System.Collections.Generic;
using System.Text;
using MiniORM.App.Data.Entities;

namespace MiniORM.App.Data
{
    public class SoftUniDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments{ get; set; }
        public DbSet<Project> Projects{ get; set; }
        public DbSet<EmployeeProject> EmployeesProjects{ get; set; }

        public SoftUniDbContext(string connectionString) : base(connectionString)
        {
        }
    }
}
