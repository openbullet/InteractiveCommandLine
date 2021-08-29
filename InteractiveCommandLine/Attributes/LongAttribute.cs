using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate an long parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class LongAttribute : Attribute
    {
        /// <summary>
        /// The minimum accepted value.
        /// </summary>
        public long Min { get; set; } = 0;

        /// <summary>
        /// The maximum accepted value.
        /// </summary>
        public long Max { get; set; } = long.MaxValue;

        /// <summary>
        /// Configures an long parameter.
        /// </summary>
        /// <param name="min">The minimum accepted value.</param>
        /// <param name="max">The maximum accepted value.</param>
        public LongAttribute(long min = 0, long max = long.MaxValue)
        {
            Min = min;
            Max = max;
        }
    }
}
