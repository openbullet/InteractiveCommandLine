using System;

namespace InteractiveCommandLine.Parameters
{
    internal class StringArrayParameter : Parameter
    {
        internal int MinSize { get; set; } = 0;
        internal int MaxSize { get; set; } = int.MaxValue;
        internal string[] Default { get; set; }

        internal override string DefaultString => string.Join(',', Default);

        internal string[] ParseAndValidate(string value)
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

            return arrayValue;
        }
    }
}
