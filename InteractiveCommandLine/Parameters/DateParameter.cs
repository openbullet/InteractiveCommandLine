using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace InteractiveCommandLine.Parameters
{
    /// <summary>
    /// A date parameter.
    /// </summary>
    public class DateParameter : Parameter
    {
        /// <summary>
        /// The DateTime date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The format of date to parse.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Creates a date parameter.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="description">The description of what the parameter changes</param>
        /// <param name="def">The default value</param>
        /// <param name="positional">Whether the parameter is positional</param>
        /// <param name="format">The parsing format</param>
        public DateParameter(string name, string description = "No description provided", string def = "01-01-1970", bool positional = false, string format = "dd-MM-yyyy")
        {
            Name = name;
            Description = description;
            Default = def;
            Format = format;
            Positional = positional;
        }

        internal override void CheckValidity(string value)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime.ParseExact(value, Format, provider);
        }
    }
}
