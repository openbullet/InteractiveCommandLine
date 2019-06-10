using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// A string parameter.
    /// </summary>
    public class StringParameter : Parameter
    {
        /// <summary>
        /// The minimum length of the string.
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// The maximum length of the string.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// The characters that must not be in the string.
        /// </summary>
        public char[] ForbiddenCharacters { get; set; }

        /// <summary>
        /// Creates a string parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        /// <param name="minLength">The mimimum length</param>
        /// <param name="maxLength">The maximum length</param>
        /// <param name="forbiddenCharacters">The characters that must not be present</param>
        public StringParameter(string name, string description = "No description provided", string def = "", bool positional = false, int minLength = 1, int maxLength = 1024, char[] forbiddenCharacters = null)
        {
            Name = name;
            Description = description;
            Default = def;
            Positional = positional;
            MinLength = minLength;
            MaxLength = maxLength;
            ForbiddenCharacters = forbiddenCharacters;
        }

        internal override void CheckValidity(string value)
        {
            string stringValue = value.ToString();

            if (ForbiddenCharacters != null && ForbiddenCharacters.Any(c => stringValue.Contains(c)))
            {
                throw new Exception($"Forbidden character: {ForbiddenCharacters.First(c => stringValue.Contains(c)).ToString()}");
            }

            if (stringValue.Length > MaxLength)
            {
                throw new Exception($"The string is too long! Max: {MaxLength} | Length: {stringValue.Length}");
            }

            if (stringValue.Length < MinLength)
            {
                throw new Exception($"The string is too short! Min: {MinLength} | Length: {stringValue.Length}");
            }
        }
    }
}
