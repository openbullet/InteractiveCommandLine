using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InteractiveCommandLine
{
    internal class Command
    {
        internal bool Stateful { get; set; }
        internal string Identifier { get; set; }
        internal string Description { get; set; }
        internal MethodInfo Method { get; set; }
        internal Parameter[] Parameters { get; set; }
        internal string[] Examples { get; set; }

        internal Command(string identifier, MethodInfo method, Parameter[] parameters = null,
            string description = "No description provided", string[] examples = null)
        {
            Identifier = identifier;
            Description = description;
            Method = method;

            Parameters = parameters ?? Array.Empty<Parameter>();
            Examples = examples ?? Array.Empty<string>();

            CheckParameterValidity();
        }

        private void CheckParameterValidity()
        {
            // Check if the parameters provided are valid (normal params MUST follow positional params)
            var normalIndices = Parameters.Where(p => !p.Required).Select(p => Parameters.ToList().IndexOf(p)).OrderBy(i => i);
            var positionalIndices = Parameters.Where(p => p.Required).Select(p => Parameters.ToList().IndexOf(p)).OrderBy(i => i);
            
            if (normalIndices.Any() && positionalIndices.Any() && positionalIndices.Last() > normalIndices.First())
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

        internal void Execute(object state, string line)
        {
            List<object> parsedValues = new();

            if (Stateful)
            {
                parsedValues.Add(state);
            }

            // Parse all the parameters
            for (var i = 0; i < Parameters.Length; i++)
            {
                var parameter = Parameters[i];

                // A bool parameter does not need a value, it just needs to be present
                if (parameter is BoolParameter)
                {
                    parsedValues.Add(Utils.IsParameterPresent(line, parameter.Name));
                    continue;
                }

                // If required, try to parse it from the string
                if (parameter.Required)
                {
                    try
                    {
                        parsedValues.Add(ParseString(parameter, Utils.ParsePositionalParameter(line, i, Identifier)));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new Exception($"Parameter '{parameter.Name}' is required");
                    }
                }

                // If not required
                else
                {
                    // If not present, use the default value
                    if (!Utils.IsParameterPresent(line, parameter.Name))
                    {
                        parsedValues.Add(parameter switch
                        {
                            BoolParameter boolParam => boolParam.Default,
                            DateTimeParameter dateTimeParam => dateTimeParam.Default,
                            EnumParameter enumParam => enumParam.Default,
                            FileOrFolderParameter fofParam => fofParam.Default,
                            IntParameter intParam => intParam.Default,
                            LongParameter longParam => longParam.Default,
                            StringArrayParameter stringArrayParam => stringArrayParam.Default,
                            StringParameter stringParam => stringParam.Default,
                            _ => throw new NotImplementedException()
                        });
                    }

                    // Otherwise parse its value specified by the user
                    else
                    {
                        parsedValues.Add(ParseString(parameter, Utils.ParseValueParameter(line, parameter.Name)));
                    }
                }
            }

            // Try to execute the method
            try
            {
                Method.Invoke(null, parsedValues.ToArray());
            }
            catch (Exception ex)
            {
                if (ICL.CatchExceptions)
                {
                    var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                    Console.WriteLine($"Failed! Reason: {message}");
                }
                else
                {
                    throw;
                }
            }
        }

        private static object ParseString(Parameter parameter, string value)
            => parameter switch
            {
                BoolParameter boolParam => boolParam.ParseAndValidate(value),
                DateTimeParameter dateTimeParam => dateTimeParam.ParseAndValidate(value),
                EnumParameter enumParam => enumParam.ParseAndValidate(value),
                FileOrFolderParameter fofParam => fofParam.ParseAndValidate(value),
                IntParameter intParam => intParam.ParseAndValidate(value),
                LongParameter longParam => longParam.ParseAndValidate(value),
                StringArrayParameter stringArrayParam => stringArrayParam.ParseAndValidate(value),
                StringParameter stringParam => stringParam.ParseAndValidate(value),
                _ => throw new NotImplementedException()
            };
    }
}
