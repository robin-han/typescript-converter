using System;

using Microsoft.Extensions.Logging;

using TypeScript.Converter;

namespace GrapeCity.Syntax.Converter.Console
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            System.Console.WriteLine("GrapeCity (R) Source Code Syntax Converter");
            System.Console.WriteLine("Copyright (C) GrapeCity Corporation. All rights reserved.");
            System.Console.WriteLine();

            using (var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); }))
            {
                var logger = loggerFactory.CreateLogger<Program>();
                var application = new ConverterApplication();
                try
                {
                    application.Execute(args);
                }
                catch (Exception exception)
                {
                    System.Console.Error.WriteLine(exception.ToString());
                    logger.LogError(exception.Message);
                    Environment.Exit(1);
                }
            }
        }
    }
}
