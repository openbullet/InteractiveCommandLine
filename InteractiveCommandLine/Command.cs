using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InteractiveCommandLine
{
    /// <summary>
    /// A command that the user can call.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// The identifier of the command.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The description of the command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The action that will be executed when the command is called.
        /// </summary>
        public Action<Parsed> Action { get; set; }

        /// <summary>
        /// The list of parameters that the command supports.
        /// </summary>
        public Parameter[] Parameters { get; set; }

        /// <summary>
        /// A list of examples that are shown in the help message.
        /// </summary>
        public string[] Examples { get; set; }

        /// <summary>
        /// Makes a new command
        /// </summary>
        /// <param name="identifier">The name of the command</param>
        /// <param name="action">The action to take in case of a match</param>
        /// <param name="parameters">The accepted parameters</param>
        /// <param name="description">A description of what the command does</param>
        /// <param name="examples">A list of examples that are shown to the user in the help message</param>
        public Command(string identifier, Action<Parsed> action, Parameter[] parameters = null, string description = "No description provided", string[] examples = null)
        {
            Identifier = identifier;
            Description = description;
            Action = action;

            if (parameters == null) Parameters = new Parameter[] { };
            else Parameters = parameters;

            if (examples == null) Examples = new string[] { };
            else Examples = examples;

            CheckParameterValidity();
        }

        /// <summary>
        /// Adds an input parameter to the command.
        /// </summary>
        /// <param name="parameter">The input parameter</param>
        public void AddParameter(Parameter parameter)
        {
            var list = Parameters.ToList();
            list.Add(parameter);
            Parameters = list.ToArray();

            CheckParameterValidity();
        }

        private void CheckParameterValidity()
        {
            // Check if the parameters provided are valid (normal params MUST follow positional params)
            var normalIndices = Parameters.Where(p => !p.Positional).Select(p => Parameters.ToList().IndexOf(p)).OrderBy(i => i);
            var positionalIndices = Parameters.Where(p => p.Positional).Select(p => Parameters.ToList().IndexOf(p)).OrderBy(i => i);
            if (normalIndices.Count() > 0 && positionalIndices.Count() > 0 && positionalIndices.Last() > normalIndices.First())
            {
                throw new Exception("Positional parameters cannot follow normal parameters!");
            }

            // Check if the name has spaces
            if (Parameters.Any(p => p.Name.Contains(' ')))
            {
                throw new Exception("Parameter names cannot have spaces!");
            }
        }

        /// <summary>
        /// Checks whether a command matches a predefined command.
        /// </summary>
        /// <param name="line">The line that needs to be parsed</param>
        /// <returns></returns>
        internal bool MatchesLine(string line)
        {
            return line.StartsWith(Identifier);
        }

        internal void Execute(string line)
        {
            // Parse all the parameters, if some are invalid return false and the error
            Parsed parsed = new Parsed();
            CultureInfo provider = CultureInfo.InvariantCulture;

            for (var i = 0; i < Parameters.Count(); i++)
            {
                var parameter = Parameters[i];

                // If not bool, not positional and not present
                if (parameter.GetType() != typeof(BoolParameter) && !parameter.Positional && !Utils.IsParameterPresent(line, parameter.Name))
                {
                    // If not essential, add the default value
                    if (!parameter.Essential)
                    {
                        if (parameter.GetType() == typeof(IntParameter))
                        {
                            parsed.AddInt(parameter.Name, int.Parse(parameter.Default));
                        }
                        else if (parameter.GetType() == typeof(LongParameter))
                        {
                            parsed.AddLong(parameter.Name, long.Parse(parameter.Default));
                        }
                        else if (parameter.GetType() == typeof(DateParameter))
                        {
                            parsed.AddDate(parameter.Name, DateTime.ParseExact(parameter.Default, (parameter as DateParameter).Format, provider));
                        }
                        else if (Utils.IsStringParameter(parameter.GetType()))
                        {
                            parsed.AddString(parameter.Name, parameter.Default);
                        }
                        else if (parameter.GetType() == typeof(StringArrayParameter))
                        {
                            parsed.AddStringArray(parameter.Name, parameter.Default == "" ? new string[] { } : parameter.Default.Split(','));
                        }
                    }

                    // If essential (and not present) throw error
                    else
                    {
                        throw new Exception($"Parameter {parameter.Name} is required");
                    }

                    continue;
                }

                string p = ""; // The parsed value

                // The bool parameter does not need a value, it just needs to be presentx  
                if (parameter.GetType() == typeof(BoolParameter))
                {
                    parsed.AddBool(parameter.Name, Utils.IsParameterPresent(line, parameter.Name));
                }

                // Other parameters need a value
                else
                {
                    // Positional parameter, just parse the value not the name
                    if (parameter.Positional)
                    {
                        try
                        {
                            p = Utils.ParsePositionalParameter(line, i, Identifier);
                        }
                        catch
                        {
                            throw new Exception($"Parameter {parameter.Name} is required");
                        }
                    }

                    // Normal parameter e.g. -a 10 or --min-amount 15
                    else
                    {
                        try
                        {
                            p = Utils.ParseValueParameter(line, parameter.Name);
                        }
                        catch
                        {
                            if (parameter.Essential) throw new Exception($"Parameter {parameter.Name} is required");
                            else continue;
                        }
                    }

                    parameter.CheckValidity(p);

                    if (parameter.GetType() == typeof(IntParameter))
                    {
                        parsed.AddInt(parameter.Name, int.Parse(p));
                    }
                    else if (parameter.GetType() == typeof(LongParameter))
                    {
                        parsed.AddLong(parameter.Name, long.Parse(p));
                    }
                    else if (parameter.GetType() == typeof(DateParameter))
                    {
                        parsed.AddDate(parameter.Name, DateTime.ParseExact(p, (parameter as DateParameter).Format, provider));
                    }
                    else if (Utils.IsStringParameter(parameter.GetType()))
                    {
                        parsed.AddString(parameter.Name, p);
                    }
                    else if (parameter.GetType() == typeof(StringArrayParameter))
                    {
                        parsed.AddStringArray(parameter.Name, p == "" ? new string[] { } : p.Split(','));
                    }
                }
            }

            // Execute the action
            if (ICL.CatchExceptions)
            {
                try
                {
                    Action.Invoke(parsed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed! Reason: {ex.Message}");
                }
            }
            else
            {
                Action.Invoke(parsed);
            }
        }
    }
}
