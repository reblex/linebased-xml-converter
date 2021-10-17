﻿using System;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;

#nullable enable

namespace Converter
{
    /// <summary>
    /// Command Line Interface for Converter app.
    /// </summary>
    class CLI
    {
        public static int Main(string[] args)
        {
            var cmd = new RootCommand
            {
            new Argument<string>("input", "Input file."),
            new Option<string>(new[] {"--output", "-o"}, "Output file name. Defaults to input name with xml extension."),
            new Option(new[] {"--verbose", "-v"}, "Detailied ouput."),
            new Option(new[] {"--force", "-f"}, "Write output even if files already exists.")
            };

            cmd.Handler = CommandHandler.Create<string, string, bool, bool>(HandleConversion);

            return cmd.Invoke(args);
        }

        // CLI Handlers
        private static void HandleConversion(string input, string output, bool verbose, bool force)
        {
            // Can't find input file
            if (!File.Exists(input))
            {
                Console.WriteLine("No such input file: " + input);
                return;
            }

            output = output != "" ? output : Path.ChangeExtension(input, ".xml");         

            // Already existing output file
            if (File.Exists(output) && !force)
            {
                Console.WriteLine("Output name " + output + " already exists. Please enter alternative output name using the -o option.");
                return;
            }

            Converter.LineBasedToXML(input, output);
        }
    }
}
