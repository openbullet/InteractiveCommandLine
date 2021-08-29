using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate a method that constitutes a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// The identifier of the command. If null, the 
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The description of the command.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A list of examples that are shown in the help message.
        /// </summary>
        public string[] Examples { get; set; }

        /// <summary>
        /// Declares a command.
        /// </summary>
        /// <param name="description">The description of the command.</param>
        public CommandAttribute(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Declares a command.
        /// </summary>
        /// <param name="identifier">The identifier of the command.</param>
        /// <param name="description">The description of the command.</param>
        /// <param name="examples">A list of examples that are shown in the help message.</param>
        public CommandAttribute(string identifier, string description, params string[] examples)
        {
            Identifier = identifier;
            Description = description;
            Examples = examples;
        }
    }
}
