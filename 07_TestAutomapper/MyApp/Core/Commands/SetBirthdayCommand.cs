using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly EmployeesMappingContext context;

        public SetBirthdayCommand(EmployeesMappingContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int id = int.Parse(args[0]);
            DateTime birthday = DateTime.Parse(args[1]);

            this.context.Employees.FirstOrDefault(e => e.Id == id).Birthday = birthday;

            this.context.SaveChanges();

            return string.Empty;
        }
    }
}
