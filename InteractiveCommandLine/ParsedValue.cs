using System;
using System.Collections.Generic;
using System.Text;

namespace InteractiveCommandLine
{
    internal enum ParsedValueType
    {
        Int,
        Long,
        String,
        StringArray,
        Bool,
        Date
    }

    internal class ParsedValue
    {
        internal string Name { get; set; }
        internal dynamic Value { get; set; }
        internal ParsedValueType Type { get; set; }
    }
}
