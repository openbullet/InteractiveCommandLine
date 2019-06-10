using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// An enum parameter.
    /// </summary>
    public class EnumParameter : Parameter
    {
        /// <summary>
        /// The available choices of the enum parameter.
        /// </summary>
        public string[] Choices { get; set; }

        /// <summary>
        /// Creates an enum parameter given an existing enum.
        /// </summary>
        /// <param name="name">The parameter name</param>
        /// <param name="enumType">The type of enum to automatically add options from</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        public EnumParameter(string name, Type enumType, string description = "No description provided", string def = "", bool positional = false)
        {
            Name = name;
            Description = description;
            Default = def;
            Choices = Enum.GetNames(enumType);
            Positional = positional;
        }

        /// <summary>
        /// Creates an enum parameter given a list of choices.
        /// </summary>
        /// <param name="name">The parameter name</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="choices">The list of choices</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        public EnumParameter(string name, string[] choices, string description = "No description provided", string def = "", bool positional = false)
        {
            Name = name;
            Description = description;
            Default = def;
            Choices = choices;
            Positional = positional;
        }

        internal override void CheckValidity(string value)
        {
            string stringValue = value.ToString();

            if (!Choices.Any(c => c.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception($"Enum value not allowed! Allowed values: {string.Join(", ", Choices)}");
            }
        }
    }
}
