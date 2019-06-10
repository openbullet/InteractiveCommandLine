using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// A file or folder parameter.
    /// </summary>
    public class FileOrFolderParameter : Parameter
    {
        /// <summary>
        /// Creates a file or folder parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        public FileOrFolderParameter(string name, string description = "No description provided", string def = "", bool positional = false)
        {
            Name = name;
            Description = description;
            Default = def;
            AutoCompleteType = AutoCompleteType.FileOrFolder;
            Positional = positional;
        }

        internal override void CheckValidity(string value)
        {

        }
    }
}
