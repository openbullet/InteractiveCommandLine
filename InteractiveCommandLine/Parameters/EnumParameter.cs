using System;
using System.Collections.Generic;

namespace InteractiveCommandLine.Parameters
{
    internal class EnumParameter : Parameter
    {
        internal Type EnumType { get; set; }
        internal string Default { get; set; }
        internal IEnumerable<string> Choices => Enum.GetNames(EnumType);

        internal override string DefaultString => Default;

        internal object ParseAndValidate(string value)
            => Enum.Parse(EnumType, value);
    }
}
