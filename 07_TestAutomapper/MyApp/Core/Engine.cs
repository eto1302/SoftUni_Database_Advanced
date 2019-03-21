using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Contracts;

namespace MyApp.Core
{
    public class Engine : IEngine
    {
        private readonly IServiceProvider _provider;

        public Engine(IServiceProvider services)
        {
            this._provider = services;
        }

        public void Run()
        {
            while (true)
            {
                var input = Console.ReadLine()
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var interpreter = this._provider.GetService<ICommandInterpreter>();

                var result = interpreter.Read(input);
                Console.WriteLine(result);
            }
        }
    }
}
