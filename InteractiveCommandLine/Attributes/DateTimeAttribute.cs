using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate a DateTime parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DateTimeAttribute : Attribute
    {
        /// <summary>
        /// The minimum accepted value.
        /// </summary>
        public DateTime Min { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The maximum accepted value.
        /// </summary>
        public DateTime Max { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// The default value.
        /// </summary>
        public DateTime Default { get; set; } = new DateTime(1, 1, 1970);

        /// <summary>
        /// The format to use when parsing the value from the user-supplied string.
        /// </summary>
        public string Format { get; set; } = "dd-MM-yyyy";

        /// <summary>
        /// Whether this parameter is required.
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
