using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Attribute used to decorate an int parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class IntAttribute : Attribute
    {
        /// <summary>
        /// The minimum accepted value.
        /// </summary>
        public int Min { get; set; } = 0;

        /// <summary>
        /// The maximum accepted value.
        /// </summary>
        public int Max { get; set; } = int.MaxValue;

        /// <summary>
        /// Configures an int parameter.
        /// </summary>
        /// <param name="min">The minimum accepted value.</param>
        /// <param name="max">The maximum accepted value.</param>
        public IntAttribute(int min = 0, int max = int.MaxValue)
        {
            Min = min;
            Max = max;
        }
    }
}
