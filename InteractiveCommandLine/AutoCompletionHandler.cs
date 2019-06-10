using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InteractiveCommandLine
{
    class AutoCompletionHandler : IAutoCompleteHandler
    {
        // characters to start completion from
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/' };

        internal Dictionary<string, string[]> Lists = new Dictionary<string, string[]>();

        internal void AddCompletion(string name, string[] choices)
        {
            if (Lists.ContainsKey(name)) Lists[name] = choices;
            else Lists.Add(name, choices);
        }

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
            try
            {
                // Match the correct command
                var command = ICL.MatchCommand(text);

                // If no command matches, I'm typing the command name so return those
                if (command == null)
                {
                    var identifiers = ICL.commands.Select(b => b.Identifier).Concat(new string[] { ICL.HelpCommand }).ToArray();

                    // If we didn't type anything yet we return all the possible options
                    if (text == "") return identifiers;

                    // If it's the help command, we return the options for it
                    var tempSplit = text.Split(' ');
                    if (tempSplit.Count() == 2 && tempSplit[0] == ICL.HelpCommand)
                    {
                        return ICL.commands.Select(b => b.Identifier)
                            .Where(i => i.StartsWith(tempSplit[1], StringComparison.InvariantCultureIgnoreCase)).ToArray();
                    }

                    // Otherwise we only return the options that start with the correct text
                    return identifiers.Where(i => i.StartsWith(text, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                }

                // For this you don't need to get the Parameter object, just check how many positional params are in the Command
                // Check if the parameter is positional or not (just count the positional parameters and the currently typed ones)
                var split = Utils.SplitLine(text.Substring(command.Identifier.Length + 1));
                var positional = command.Parameters[split.Count - 1].Positional;

                Parameter parameter = null;

                // If positional ...
                if (positional)
                {
                    parameter = command.Parameters[split.Count - 1];
                }

                // If not positional ...
                else
                {
                    // If we're actually writing the parameter name (more than 1 char), suggest the possible parameter names
                    var last = split.Last();
                    if (last.StartsWith("--"))
                    {
                        return command.Parameters
                            .Where(p => p.Name.Length > 1 && p.Name.StartsWith(last.Replace("--", "")))
                            .Select(p => $"--{p.Name}")
                            .ToArray();
                    }

                    // Get the name of the current parameter value that is being set (if it exists)
                    var paramName = Utils.ParseParameterName(split.Last(s => Utils.IsParameter(s)));

                    // Once you have identified the Parameter object:
                    parameter = command.Parameters.First(p => p.Name == paramName);
                }
                
                // Check if the parameter is EnumParameter, StringParameter (with a specified list name), File/Folder/FileOrFolder Parameter
                if (!Utils.IsStringParameter(parameter.GetType())) throw new Exception(); // Cannot autocomplete int, long etc.

                // If autocomplete = list -> search for the list with that name and return those
                if (parameter.GetType() == typeof(StringParameter))
                {
                    if (!string.IsNullOrEmpty(parameter.AutoCompleteList))
                    {
                        // If we didn't type anything yet (the last is still the parameter name) we return all the possible options
                        if (Utils.IsParameter(split.Last())) return Lists.First(l => l.Key == parameter.AutoCompleteList).Value;

                        // Otherwise we return only the options that start with the correct text
                        else return Lists.First(l => l.Key == parameter.AutoCompleteList).Value
                                .Where(c => c.StartsWith(split.Last(), StringComparison.InvariantCultureIgnoreCase)).ToArray();
                    }
                }
                else if (parameter.GetType() == typeof(EnumParameter))
                {
                    // If we didn't type anything yet (the last is still the parameter name) we return all the possible options
                    if (Utils.IsParameter(split.Last())) return (parameter as EnumParameter).Choices;

                    // Otherwise we return only the options that start with the correct text
                    else return (parameter as EnumParameter).Choices
                            .Where(c => c.StartsWith(split.Last(), StringComparison.InvariantCultureIgnoreCase)).ToArray();
                }
                else if (parameter.GetType() == typeof(FileOrFolderParameter))
                {
                    var last = split.Last();
                    var dir = Path.GetDirectoryName(last);
                    var lastChunk = last.Contains('/') ? last.Split('/').Last() : last;
                    if (dir == "") dir = "./";
                    if (lastChunk == "") return GetFilesAndFolders(dir);
                    else return GetFilesAndFolders(dir).Where(f => f.StartsWith(lastChunk)).ToArray();
                }
            }
            catch { }

            return new string[] { };
        }

        private string[] GetFiles(string path)
        {
            return Directory.EnumerateFiles(path)
                //.Select(f => Path.Combine(path, f.Replace('\\', '/'))).ToArray();
                .Select(f => Path.GetFileName(f.Replace('\\', '/'))).ToArray();
        }

        private string[] GetFolders(string path)
        {
            return Directory.EnumerateDirectories(path)
                //.Select(d => Path.Combine(path, d.Replace('\\', '/'))).ToArray();
                .Select(d => Path.GetFileName(d.Replace('\\', '/')) + "/").ToArray();
        }

        private string[] GetFilesAndFolders(string path)
        {
            return GetFolders(path).Concat(GetFiles(path)).ToArray();
        }
    }
}
