using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InteractiveCommandLine
{
    internal static class Help
    {
        internal static void PrintAvailableCommands()
        {
            Console.Clear();

            Console.WriteLine(@"AVAILABLE COMMANDS
To learn more about a command, type 'help command-name'
");

            foreach(var command in ICL.Commands)
            {
                Write(command.Identifier, ConsoleColor.Yellow);
                WriteLine($" - {command.Description}", ConsoleColor.White);
            }
        }

        internal static void PrintCommand(Command command)
        {
            Console.Clear();

            WriteLine(command.Identifier + Environment.NewLine, ConsoleColor.White);
            WriteLine("Syntax:", ConsoleColor.Cyan);
            WriteLine($"{command.Identifier} {string.Join(' ', command.Parameters.Where(p => p.Required).Select(p => p.Name))} [optional parameters]{Environment.NewLine}", ConsoleColor.White);
            WriteLine("Description:", ConsoleColor.Cyan);
            WriteLine(command.Description + Environment.NewLine, ConsoleColor.White);
            WriteLine("Compulsory Parameters:", ConsoleColor.Cyan);
            PrintParameters(command.Parameters.Where(p => p.Required));
            WriteLine("Optional Parameters:", ConsoleColor.Cyan);
            PrintParameters(command.Parameters.Where(p => !p.Required));
            WriteLine($"{Environment.NewLine}Examples:", ConsoleColor.Cyan);

            foreach (var example in command.Examples)
            {
                WriteLine(example, ConsoleColor.White);
            }

        }

        private static void Write(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void WriteLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintParameters(IEnumerable<Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Required)
                {
                    WriteLine($"  {parameter.Name}", ConsoleColor.Yellow);

                    Write($@"    Type: {parameter.GetType().Name}
    Description: {parameter.Description}", ConsoleColor.White);
                }
                else
                {
                    if (parameter.Name.Length == 1)
                    {
                        WriteLine($"  -{parameter.Name}", ConsoleColor.Yellow);
                    }
                    else
                    {
                        WriteLine($"  --{parameter.Name}", ConsoleColor.Yellow);
                    }

                    Write($@"    Type: {parameter.GetType().Name}
    Description: {parameter.Description}
    Default: {parameter.DefaultString}", ConsoleColor.White);
                }
                
                if (parameter is IntParameter intParam)
                {
                    WriteLine($@"
    Min: {intParam.Min}
    Max: {intParam.Max}", ConsoleColor.White);
                }
                else if (parameter is LongParameter longParam)
                {
                    WriteLine($@"
    Min: {longParam.Min}
    Max: {longParam.Max}", ConsoleColor.White);
                }
                else if (parameter is StringParameter stringParam)
                {
                    WriteLine($@"
    Min Length: {stringParam.MinLength}
    Max Length: {stringParam.MaxLength}
    Forbidden Characters: {new string(stringParam.ForbiddenCharacters)}", ConsoleColor.White);
                }
                else if (parameter is EnumParameter enumParam)
                {
                    WriteLine($@"
    Choices: {string.Join(", ", enumParam.Choices)}", ConsoleColor.White);
                }
                else if (parameter is DateTimeParameter dateTimeParam)
                {
                    WriteLine($@"
    Format: {dateTimeParam.Format}", ConsoleColor.White);
                }
                else
                {
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }
    }
}
