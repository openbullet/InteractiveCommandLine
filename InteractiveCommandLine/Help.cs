using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            foreach(var command in ICL.commands)
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
            WriteLine($"{command.Identifier} {string.Join(' ', command.Parameters.Where(p => p.Positional).Select(p => p.Name))} [optional parameters]{Environment.NewLine}", ConsoleColor.White);
            WriteLine("Description:", ConsoleColor.Cyan);
            WriteLine(command.Description + Environment.NewLine, ConsoleColor.White);
            WriteLine("Compulsory Parameters:", ConsoleColor.Cyan);
            PrintParameters(command.Parameters.Where(p => p.Positional));
            WriteLine("Optional Parameters:", ConsoleColor.Cyan);
            PrintParameters(command.Parameters.Where(p => !p.Positional));
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
                if (parameter.Positional)
                {
                    WriteLine($"  {parameter.Name}", ConsoleColor.Yellow);

                    Write($@"    Type: {parameter.GetType().Name}
    Description: {parameter.Description}", ConsoleColor.White);
                }
                else
                {
                    if (parameter.Name.Length == 1) WriteLine($"  -{parameter.Name}", ConsoleColor.Yellow);
                    else WriteLine($"  --{parameter.Name}", ConsoleColor.Yellow);

                    Write($@"    Type: {parameter.GetType().Name}
    Description: {parameter.Description}
    Default: {parameter.Default}", ConsoleColor.White);
                }
                
                if (parameter.GetType() == typeof(IntParameter))
                {
                    WriteLine($@"
    Min: {(parameter as IntParameter).Min}
    Max: {(parameter as IntParameter).Max}", ConsoleColor.White);
                }
                else if (parameter.GetType() == typeof(LongParameter))
                {
                    WriteLine($@"
    Min: {(parameter as LongParameter).Min}
    Max: {(parameter as LongParameter).Max}", ConsoleColor.White);
                }
                else if (parameter.GetType() == typeof(StringParameter))
                {
                    WriteLine($@"
    Min Length: {(parameter as StringParameter).MinLength}
    Max Length: {(parameter as StringParameter).MaxLength}
    Forbidden Characters: {new string((parameter as StringParameter).ForbiddenCharacters)}", ConsoleColor.White);
                }
                else if (parameter.GetType() == typeof(EnumParameter))
                {
                    WriteLine($@"
    Choices: {string.Join(", ", (parameter as EnumParameter).Choices)}", ConsoleColor.White);
                }
                else if (parameter.GetType() == typeof(DateParameter))
                {
                    WriteLine($@"
    Format: {(parameter as DateParameter).Format}", ConsoleColor.White);
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
