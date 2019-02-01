using GrapeCity.CodeAnalysis.TypeScript.Converter.CSharp;
using GrapeCity.CodeAnalysis.TypeScript.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace GrapeCity.CodeAnalysis.TypeScript.Converter
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
