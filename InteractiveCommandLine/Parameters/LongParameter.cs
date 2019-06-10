using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// A long parameter.
    /// </summary>
    public class LongParameter : Parameter
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public long Min { get; set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public long Max { get; set; }

        /// <summary>
        /// Creates a new long parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public LongParameter(string name, string description = "No description provided", string def = "0", bool positional = false, long min = 0, long max = long.MaxValue)
        {
            Name = name;
            Description = description;
            Default = def;
            Positional = positional;
            Min = min;
            Max = max;
        }

        internal override void CheckValidity(string value)
        {
            long longValue = 0;
            if (!long.TryParse(value, out longValue)) throw new Exception("Not a long integer");

            if (longValue < Min)
            {
                throw new Exception($"The number is too small! Min: {Min}");
            }

            if (longValue > Max)
            {
                throw new Exception($"The number is too small! Max: {Max}");
            }
        }
    }
}
