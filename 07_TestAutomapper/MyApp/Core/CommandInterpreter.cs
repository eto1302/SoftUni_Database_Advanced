using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.Contracts;

namespace MyApp.Core
{
    public class CommandInterpreter : ICommandInterpreter
    {
        private const string CMD_SUFFIX = "Command";
        private readonly IServiceProvider _provider;

        public CommandInterpreter(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public string Read(string[] args)
        {
            string command = $"{args[0]}{CMD_SUFFIX}";

            Type type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(x => x.Name == command);

            if (type == null)
            {
                throw new InvalidOperationException("Command does not exist!");
            }

            string[] commandParams = args
                .Skip(1)
                .ToArray();

            ConstructorInfo constructor = type.GetConstructors()[0];

            var constructorParams = constructor
                .GetParameters()
                .Select(x => x.ParameterType)
                .Select(x => this._provider.GetService(x))
                .ToArray();

            ICommand commandInstance = (ICommand)constructor
                .Invoke(constructorParams);

            return commandInstance.Execute(commandParams);

        }
    }
}
