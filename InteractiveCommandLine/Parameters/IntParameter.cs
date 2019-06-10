using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// An int parameter.
    /// </summary>
    public class IntParameter : Parameter
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// Creates an int parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public IntParameter(string name, string description = "No description provided", string def = "0", bool positional = false, int min = 0, int max = int.MaxValue)
        {
            Name = name;
            Description = description;
            Default = def;
            Min = min;
            Max = max;
            Positional = positional;
        }

        internal override void CheckValidity(string value)
        {
            var intValue = 0;
            if (!int.TryParse(value, out intValue)) throw new FormatException("Not an integer");

            if (intValue < Min)
            {
                throw new Exception($"The number is too small! Min: {Min}");
            }

            if (intValue > Max)
            {
                throw new Exception($"The number is too small! Max: {Max}");
            }
        }
    }
}
