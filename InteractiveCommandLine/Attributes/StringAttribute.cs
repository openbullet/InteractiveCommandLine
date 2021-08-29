using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate a string parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StringAttribute : Attribute
    {
        /// <summary>
        /// The minimum length of the string.
        /// </summary>
        public int MinLength { get; set; } = 0;

        /// <summary>
        /// The maximum length of the string.
        /// </summary>
        public int MaxLength { get; set; } = int.MaxValue;

        /// <summary>
        /// The characters that must not be in the string.
        /// </summary>
        public string ForbiddenCharacters { get; set; }

        /// <summary>
        /// Configures a string parameter.
        /// </summary>
        /// <param name="minLength">The minimum length of the string.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <param name="forbiddenCharacters">The characters that must not be in the string.</param>
        public StringAttribute(int minLength = 0, int maxLength = int.MaxValue, string forbiddenCharacters = "")
        {
            MinLength = minLength;
            MaxLength = maxLength;
            ForbiddenCharacters = forbiddenCharacters;
        }
    }
}
