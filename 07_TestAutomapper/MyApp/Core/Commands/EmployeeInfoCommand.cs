using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;

namespace MyApp.Core.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeesMappingContext context;

        public EmployeeInfoCommand(EmployeesMappingContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int id = int.Parse(args[0]);

            string output;

            var employee = this.context.Employees.FirstOrDefault(e => e.Id == id);

            output = $"ID:{employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}";

            return output;
        }
    }
}
