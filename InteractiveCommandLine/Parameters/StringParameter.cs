using System;
using System.Linq;

namespace InteractiveCommandLine.Parameters
{
    internal class StringParameter : Parameter
    {
        internal int MinLength { get; set; } = 0;
        internal int MaxLength { get; set; } = int.MaxValue;
        internal char[] ForbiddenCharacters { get; set; } = Array.Empty<char>();
        internal string Default { get; set; }

        internal override string DefaultString => Default;

        // TODO: Readd support for this
        internal string AutoCompleteList { get; set; }

        internal string ParseAndValidate(string value)
        {
            if (ForbiddenCharacters != null && ForbiddenCharacters.Any(c => value.Contains(c)))
            {
                throw new Exception($"Forbidden character: {ForbiddenCharacters.First(c => value.Contains(c)).ToString()}");
            }

            if (value.Length > MaxLength)
            {
                throw new Exception($"The string is too long! Max: {MaxLength} | Length: {value.Length}");
            }

            if (value.Length < MinLength)
            {
                throw new Exception($"The string is too short! Min: {MinLength} | Length: {value.Length}");
            }

            return value;
        }
    }
}
