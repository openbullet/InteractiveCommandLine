using System;

namespace InteractiveCommandLine.Parameters
{
    internal class BoolParameter : Parameter
    {
        internal bool Default { get; set; } = false;
        internal override string DefaultString => Default.ToString();

        // TODO: Move these out of parameter classes, logic shouldn't be in here!!
        internal bool ParseAndValidate(string value)
        {
            if (!bool.TryParse(value, out bool boolValue))
            {
                throw new FormatException("Not a boolean");
            }

            return boolValue;
        }
    }
}
