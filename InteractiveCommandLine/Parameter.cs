using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine
{
    /// <summary>
    /// The available AutoCompletion types.
    /// </summary>
    public enum AutoCompleteType
    {
        /// <summary>
        /// List of choices that can be set by the user.
        /// </summary>
        List,

        /// <summary>
        /// Files or Folders on the Filesystem.
        /// </summary>
        FileOrFolder
    }

    /// <summary>
    /// A parameter that can be parsed from the command line
    /// </summary>
    public abstract class Parameter
    {
        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the parameter.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The default value of the parameter.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Whether the parameter MUST be specified by the user.
        /// </summary>
        public bool Essential { get; set; } = false;

        /// <summary>
        /// Whether the parameter is identified by its position in the command and not by its name.
        /// </summary>
        public bool Positional { get; set; } = false;
        
        internal AutoCompleteType AutoCompleteType { get; set; } = AutoCompleteType.List;

        /// <summary>
        /// The name of the list of choices for Auto Completion (only for String or Enum parameters).
        /// </summary>
        public string AutoCompleteList { get; set; }

        internal virtual void CheckValidity(string value)
        {
            
        }
    }
}
