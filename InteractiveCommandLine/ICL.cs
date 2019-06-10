using System;
using System.Collections.Generic;
using System.Linq;

namespace InteractiveCommandLine
{
    /// <summary>
    /// The Interactive Command Line.
    /// </summary>
    public static class ICL
    {
        internal static List<Command> commands = new List<Command>();

        /// <summary>
        /// The default action to be taken when no command is matched.
        /// </summary>
        public static Action DefaultAction = new Action(() => Console.WriteLine("No command matches the line."));

        /// <summary>
        /// Whether to catch all exceptions generated while invoking actions and print the message to the console.
        /// </summary>
        public static bool CatchExceptions = true;

        /// <summary>
        /// The command used to display the help text.
        /// </summary>
        public static string HelpCommand = "help";

        private static AutoCompletionHandler ach = new AutoCompletionHandler();

        /// <summary>
        /// Initializes the ReadLine with History and Auto Completion.
        /// </summary>
        public static void Initialize()
        {
            ReadLine.HistoryEnabled = true;
            ReadLine.AutoCompletionHandler = ach;
        }

        /// <summary>
        /// Sets an AutoCompletion List by name with choices provided.
        /// </summary>
        /// <param name="name">The name of the list</param>
        /// <param name="choices">The choices that the autocomplete will suggest</param>
        public static void SetAutoCompletion(string name, string[] choices)
        {
            ach.AddCompletion(name, choices);
        }

        /// <summary>
        /// Adds a command to the ICL.
        /// </summary>
        /// <param name="identifier">The command identifier</param>
        /// <param name="action">The action that needs to be executed</param>
        /// <param name="parameters">The accepted input parameters</param>
        /// <param name="description">A description of what the command does</param>
        /// <param name="examples">A list of examples that are shown to the user in the help message</param>
        public static void AddCommand(string identifier, Action<Parsed> action, Parameter[] parameters, string description = "No description provided", string[] examples = null)
        {
            commands.Add(new Command(identifier, action, parameters, description, examples));
        }

        internal static Command MatchCommand(string line)
        {
            return commands
                .OrderBy(b => b.Identifier.Length)
                .Reverse()
                .FirstOrDefault(b => b.MatchesLine(line));
        }

        /// <summary>
        /// Reads the command line and executes the matched command.
        /// </summary>
        public static void ReadAndExecute()
        {
            var line = ReadLine.Read();

            var split = line.Split(' ');
            if (split.First() == HelpCommand)
            {
                if (line == HelpCommand) Help.PrintAvailableCommands();
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
                    command.Execute(line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
