using TypeScript.Converter.CSharp;
using TypeScript.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace TypeScript.Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new AppCommand();
            app.Execute(args);
        }
    }

}
