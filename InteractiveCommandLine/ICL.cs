using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InteractiveCommandLine
{
    /// <summary>
    /// The Interactive Command Line.
    /// </summary>
    public static class ICL
    {
        /// <summary>
        /// The default action to be taken when no command is matched.
        /// </summary>
        public static Action DefaultAction { get; set; } = new Action(() => Console.WriteLine("No command matches the line."));

        /// <summary>
        /// Whether to catch all exceptions generated while invoking actions and print the message to the console.
        /// </summary>
        public static bool CatchExceptions { get; set; } = true;

        /// <summary>
        /// The command used to display the help text. By default it's 'help'.
        /// </summary>
        public static string HelpCommand { get; set; } = "help";

        internal static IEnumerable<Command> Commands => repo.Commands;

        private static readonly CommandsRepository repo = new();
        private static readonly AutoCompletionHandler ach = new();

        /// <summary>
        /// Initializes the ReadLine with History and Auto Completion.
        /// </summary>
        /// <param name="assembly">The assembly where commands are specified.</param>
        public static void Initialize(Assembly assembly)
        {
            ReadLine.HistoryEnabled = true;
            ReadLine.AutoCompletionHandler = ach;

            repo.AddFromExposedMethods(assembly);
        }

        /// <summary>
        /// Sets an AutoCompletion List by name with choices provided.
        /// </summary>
        /// <param name="name">The name of the list</param>
        /// <param name="choices">The choices that the autocomplete will suggest</param>
        public static void SetAutoCompletion(string name, List<string> choices) => ach.AddCompletion(name, choices);

        internal static Command MatchCommand(string line) => repo.Commands
            .OrderBy(b => b.Identifier.Length)
            .Reverse()
            .FirstOrDefault(b => b.MatchesLine(line));

        /// <summary>
        /// Reads the command line and executes the matched command.
        /// </summary>
        /// <param name="state">The class that holds the state.</param>
        public static void ReadAndExecute(object state)
        {
            var line = ReadLine.Read();
            var split = line.Split(' ');

            if (split.First() == HelpCommand)
            {
                if (line == HelpCommand)
                {
                    Help.PrintAvailableCommands();
                }
                else
                {
                    // Try to get an existing command
                    var c = MatchCommand(split[1]);

                    if (c == null)
                    {
                        Console.WriteLine("That command does not exist!");
                    }
                    else
                    {
                        Help.PrintCommand(c);
                    }
                }

                return;
            }

            var command = MatchCommand(line);

            if (command == null)
            {
                DefaultAction.Invoke();
            }
            else
            {
                try
                {
                    command.Execute(state, line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
