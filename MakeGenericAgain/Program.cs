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
            try
            {
                if (string.IsNullOrEmpty(options.FileName) || !File.Exists(options.FileName))
                {
                    return Return(ExitCode.InvalidFilename, $"Error Filename '{options.FileName}' is invalid or not existing");
                }

                var lines = File.ReadAllLines(options.FileName);
                for (var index = 0; index < lines.Length; index++)
                {
                    lines[index] = NameReplacer.ReplaceToGeneric(lines[index]);
                }
                File.WriteAllLines(options.FileName, lines);
                return Return(ExitCode.Success, $"Filename {options.FileName} has successfully been updated");
            }
            catch (Exception e)
            {
                return Return(ExitCode.UnknownError, e.Message);
            }
        }

        static ExitCode Return (ExitCode code, string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = code == ExitCode.Success ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
            return code;
        }

    }

    enum ExitCode : int
    {
        Success = 0,
        InvalidFilename = 1,
        UnknownError = 2
    }
}
