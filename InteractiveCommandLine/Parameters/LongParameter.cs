using System;

namespace InteractiveCommandLine.Parameters
{
    internal class LongParameter : Parameter
    {
        internal long Min { get; set; } = 0;
        internal long Max { get; set; } = long.MaxValue;
        internal long Default { get; set; } = 0;

        internal override string DefaultString => Default.ToString();

        internal long ParseAndValidate(string value)
        {
            if (!long.TryParse(value, out long longValue))
            {
                throw new Exception("Not a long integer");
            }

            if (longValue < Min)
            {
                throw new Exception($"The number is too small! Min: {Min}");
            }

            if (longValue > Max)
            {
                throw new Exception($"The number is too big! Max: {Max}");
            }

            return longValue;
        }
    }
}
