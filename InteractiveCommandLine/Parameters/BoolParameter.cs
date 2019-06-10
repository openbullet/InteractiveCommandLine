using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// A bool parameter.
    /// </summary>
    public class BoolParameter : Parameter
    {
        /// <summary>
        /// Creates a bool parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        public BoolParameter(string name, string description = "No description provided")
        {
            Name = name;
            Description = description;
            Default = "false";
        }

        internal override void CheckValidity(string value)
        {
            
        }
    }
}
