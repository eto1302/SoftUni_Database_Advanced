using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;

namespace MyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly EmployeesMappingContext context;

        public SetAddressCommand(EmployeesMappingContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int id = int.Parse(args[0]);
            string address = args[1];

            this.context.Employees.FirstOrDefault(e => e.Id == id).Address = address;

            this.context.SaveChanges();

            return string.Empty;
        }
    }
}
