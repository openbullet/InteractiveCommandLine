using System;
using System.Globalization;

namespace InteractiveCommandLine.Parameters
{
    internal class DateTimeParameter : Parameter
    {
        internal DateTime Min { get; set; } = DateTime.MinValue;
        internal DateTime Max { get; set; } = DateTime.MaxValue;
        internal DateTime Default { get; set; } = new DateTime(1, 1, 1970);
        internal string Format { get; set; } = "dd-MM-yyyy";

        internal override string DefaultString => Default.ToString();

        internal DateTime ParseAndValidate(string value)
            => DateTime.ParseExact(value, Format, CultureInfo.InvariantCulture);
    }
}
