using System;

namespace InteractiveCommandLine.Parameters
{
    internal class IntParameter : Parameter
    {
        internal int Min { get; set; } = 0;
        internal int Max { get; set; } = int.MaxValue;
        internal int Default { get; set; } = 0;

        internal override string DefaultString => Default.ToString();

        internal int ParseAndValidate(string value)
        {
            if (!int.TryParse(value, out int intValue))
            {
                throw new FormatException("Not an integer");
            }

            if (intValue < Min)
            {
                throw new Exception($"The number is too small! Min: {Min}");
            }

            if (intValue > Max)
            {
                throw new Exception($"The number is too big! Max: {Max}");
            }

            return intValue;
        }
    }
}
