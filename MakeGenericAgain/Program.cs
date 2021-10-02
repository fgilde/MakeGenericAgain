using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MakeGenericAgain
{
    class Program
    {
        private static Options options;
        static int Main(string[] args)
        {
            options = CommandLineArgs.Parse<Options>(args);
            return (int)Handle();
        }

        static ExitCode Handle()
        {
            if (string.IsNullOrEmpty(options.FileName) || !File.Exists(options.FileName))
            {
                Console.WriteLine($"Error Filename {options.FileName} is invalid or not existing");
                return ExitCode.InvalidFilename;
            }

            var lines = File.ReadAllLines(options.FileName);
            for (var index = 0; index < lines.Length; index++)
            {
                lines[index] = NameReplacer.ReplaceToGeneric(lines[index]);
            }
            File.WriteAllLines(options.FileName, lines);
            return ExitCode.Success;
        }

    }

    enum ExitCode : int
    {
        Success = 0,
        InvalidFilename = 1
    }
}
