using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate a string[] parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StringArrayAttribute : Attribute
    {
        /// <summary>
        /// The minimum size of the array.
        /// </summary>
        public int MinSize { get; set; } = 0;

        /// <summary>
        /// The maximum size of the array.
        /// </summary>
        public int MaxSize { get; set; } = int.MaxValue;

        /// <summary>
        /// The default value.
        /// </summary>
        public string[] Default { get; set; }

        /// <summary>
        /// Whether this parameter is required.
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
