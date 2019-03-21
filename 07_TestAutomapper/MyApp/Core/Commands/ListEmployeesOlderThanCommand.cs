using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;

namespace MyApp.Core.Commands
{
    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeesMappingContext context;

        public ListEmployeesOlderThanCommand(EmployeesMappingContext context)
        {
            this.context = context;
        }


        public string Execute(string[] args)
        {
            int age = int.Parse(args[0]);
            StringBuilder output = new StringBuilder();
            this.context.Employees
                .Where(e => e.Birthday.Year < DateTime.Now.Year - age)
                .OrderBy(e => e.Salary)
                .Select(a =>
                    $"{a.FirstName} {a.LastName} - ${a.Salary:F2} - Manager: {(a.Manager == null ? string.Concat(a.Manager.FirstName, a.Manager.LastName) : "[no manager]")}")
                .ToList()
                .ForEach(e => output.AppendLine(e));

            return output.ToString().TrimEnd();

        }
    }
}
