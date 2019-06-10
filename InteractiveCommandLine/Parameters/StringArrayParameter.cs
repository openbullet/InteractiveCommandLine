using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// A string array parameter.
    /// </summary>
    public class StringArrayParameter : Parameter
    {
        /// <summary>
        /// The minimum number of elements.
        /// </summary>
        public int MinSize { get; set; }

        /// <summary>
        /// The maximum number of elements.
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// Creates a string parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        /// <param name="minSize">The mimimum number of elements</param>
        /// <param name="maxSize">The maximum number of elements</param>
        public StringArrayParameter(string name, string description = "No description provided", string def = "", bool positional = false, int minSize = 1, int maxSize = 100)
        {
            Name = name;
            Description = description;
            Default = def;
            Positional = positional;
            MinSize = minSize;
            MaxSize = maxSize;
        }

        internal override void CheckValidity(string value)
        {
            string[] arrayValue = value.Split(',');

            if (arrayValue.Length > MaxSize)
            {
                throw new Exception($"The array has too many elements! Max: {MaxSize} | Size: {arrayValue.Length}");
            }

            if (arrayValue.Length < MinSize)
            {
                throw new Exception($"The array has too few elements! Min: {MinSize} | Size: {arrayValue.Length}");
            }
        }
    }
}
