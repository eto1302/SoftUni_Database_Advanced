using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly EmployeesMappingContext context;

        public SetManagerCommand(EmployeesMappingContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            int managerId = int.Parse(args[1]);

            Employee employee = this.context.Employees.FirstOrDefault(e => e.Id == employeeId);
            Employee manager = this.context.Employees.FirstOrDefault(e => e.Id == managerId);

            this.context.Employees.FirstOrDefault(e => e.Id == managerId).ManagedEmployees.Add(employee);
            this.context.Employees.FirstOrDefault(e => e.Id == employeeId).Manager = manager;
            this.context.Employees.FirstOrDefault(e => e.Id == employeeId).ManagerId = managerId;
            this.context.SaveChanges();

            return "";

        }
    }
}
