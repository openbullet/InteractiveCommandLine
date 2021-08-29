using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParamAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the parameter.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Declares a parameter of a command.
        /// </summary>
        /// <param name="description">The description of the parameter.</param>
        public ParamAttribute(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Declares a parameter of a command.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="description">The description of the parameter.</param>
        public ParamAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
