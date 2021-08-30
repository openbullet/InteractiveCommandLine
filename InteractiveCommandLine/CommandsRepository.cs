using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InteractiveCommandLine
{
    /// <summary>
    /// The repository of commands that are available to the end user.
    /// </summary>
    internal class CommandsRepository
    {
        private readonly List<Command> commands = new();

        /// <summary>
        /// All the commands currently available in the repository.
        /// </summary>
        internal IEnumerable<Command> Commands => commands;

        /// <summary>
        /// Adds commands to the repository by finding exposed methods in the given
        /// <paramref name="assembly"/>.
        /// </summary>
        internal void AddFromExposedMethods(Assembly assembly)
        {
            // Get all types of the assembly
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                // Get the methods in the type
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    // Check if the methods has a Command attribute
                    var attribute = method.GetCustomAttribute<Attributes.CommandAttribute>();
                    
                    if (attribute == null)
                    {
                        continue;
                    }

                    // If the identifier was not set manually, use the lowercase method's name
                    var id = attribute.Identifier ?? method.Name.ToLower();

                    // Throw if a command with the same id already exists
                    if (commands.Any(c => c.Identifier == id))
                    {
                        throw new Exception($"Duplicate command id: {id}");
                    }

                    var parameters = method.GetParameters();
                    var stateful = parameters.Length > 0 && parameters[0].Name == "state";

                    if (stateful)
                    {
                        parameters = parameters.Skip(1).ToArray();
                    }

                    // Build and add the command
                    var command = new Command(id, method, parameters.Select(p => BuildParameter(p)).ToArray(),
                        attribute.Description, attribute.Examples)
                    {
                        Stateful = stateful
                    };

                    commands.Add(command);
                }
            }
        }

        private static Parameter BuildParameter(ParameterInfo info)
        {
            var dict = new Dictionary<Type, Func<Parameter>>
            {
                { typeof(string), () => BuildFromString(info) },
                { typeof(bool), () => BuildFromBool(info) },
                { typeof(DateTime), () => BuildFromDateTime(info) },
                { typeof(int), () => BuildFromInt(info) },
                { typeof(long), () => BuildFromLong(info) },
                { typeof(string[]), () => BuildFromStringArray(info) },
            };

            // If it's one of the standard types
            if (dict.ContainsKey(info.ParameterType))
            {
                var param = dict[info.ParameterType].Invoke();
                param.Name = info.Name;
                return param;
            }

            // If it's an enum type
            if (info.ParameterType.IsEnum)
            {
                return BuildFromEnum(info);
            }

            throw new NotSupportedException($"Parameter {info.Name} has an unsupported type ({info.ParameterType})");
        }

        private static Parameter BuildFromString(ParameterInfo info)
        {
            // If it has the FileOrFolder attribute
            if (info.GetCustomAttribute<Attributes.FileOrFolderAttribute>() is not null)
            {
                var param = new FileOrFolderParameter();
                SetParameterInfo(info, param);

                if (info.HasDefaultValue)
                {
                    param.Default = (string)info.DefaultValue;
                }

                return param;
            }
            // Otherwise it's a normal string parameter
            else
            {
                var param = new StringParameter();
                SetParameterInfo(info, param);

                if (info.HasDefaultValue)
                {
                    param.Default = (string)info.DefaultValue;
                }

                var stringAttribute = info.GetCustomAttribute<Attributes.StringAttribute>();

                if (stringAttribute is not null)
                {
                    param.MinLength = stringAttribute.MinLength;
                    param.MaxLength = stringAttribute.MaxLength;
                    param.ForbiddenCharacters = stringAttribute.ForbiddenCharacters.ToCharArray();
                    param.AutoCompleteList = stringAttribute.AutoCompleteList;
                }

                return param;
            }
        }

        private static Parameter BuildFromBool(ParameterInfo info)
        {
            var param = new BoolParameter();
            SetParameterInfo(info, param);

            if (info.HasDefaultValue)
            {
                param.Default = (bool)info.DefaultValue;
            }

            return param;
        }

        private static Parameter BuildFromInt(ParameterInfo info)
        {
            var param = new IntParameter();
            SetParameterInfo(info, param);

            if (info.HasDefaultValue)
            {
                param.Default = (int)info.DefaultValue;
            }

            var intAttribute = info.GetCustomAttribute<Attributes.IntAttribute>();

            if (intAttribute is not null)
            {
                param.Min = intAttribute.Min;
                param.Max = intAttribute.Max;
            }

            return param;
        }

        private static Parameter BuildFromLong(ParameterInfo info)
        {
            var param = new LongParameter();
            SetParameterInfo(info, param);

            if (info.HasDefaultValue)
            {
                param.Default = (long)info.DefaultValue;
            }

            var longAttribute = info.GetCustomAttribute<Attributes.LongAttribute>();

            if (longAttribute is not null)
            {
                param.Min = longAttribute.Min;
                param.Max = longAttribute.Max;
            }

            return param;
        }

        private static Parameter BuildFromDateTime(ParameterInfo info)
        {
            var param = new DateTimeParameter();
            SetParameterInfo(info, param);

            var dateTimeAttribute = info.GetCustomAttribute<Attributes.DateTimeAttribute>();

            if (dateTimeAttribute is not null)
            {
                param.Min = dateTimeAttribute.Min;
                param.Max = dateTimeAttribute.Max;
                param.Default = dateTimeAttribute.Default;
                param.Format = dateTimeAttribute.Format;
                param.Required = dateTimeAttribute.Required;
            }

            return param;
        }

        private static Parameter BuildFromStringArray(ParameterInfo info)
        {
            var param = new StringArrayParameter();
            SetParameterInfo(info, param);

            var stringArrayAttribute = info.GetCustomAttribute<Attributes.StringArrayAttribute>();

            if (stringArrayAttribute is not null)
            {
                param.MinSize = stringArrayAttribute.MinSize;
                param.MaxSize = stringArrayAttribute.MaxSize;
                param.Default = stringArrayAttribute.Default;
                param.Required = stringArrayAttribute.Required;
            }

            return param;
        }

        private static Parameter BuildFromEnum(ParameterInfo info)
        {
            var param = new EnumParameter();
            SetParameterInfo(info, param);

            param.Default = Enum.GetNames(info.ParameterType).First();
            param.EnumType = info.ParameterType;

            return param;
        }

        private static void SetParameterInfo(ParameterInfo info, Parameter param)
        {
            var paramAttribute = info.GetCustomAttribute<Attributes.ParamAttribute>();
            param.Name = paramAttribute?.Name ?? info.Name;
            param.Description = paramAttribute?.Description ?? string.Empty;
            param.Required = !info.HasDefaultValue;
        }
    }
}
