using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteractiveCommandLine
{
    /// <summary>
    /// The collection of values parsed from the user's input.
    /// </summary>
    public class Parsed
    {
        private List<ParsedValue> Values = new List<ParsedValue>();

        /// <summary>
        /// Gets an int value.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The int value</returns>
        public int Int(string name)
        {
            return (int)GetValue(name, ParsedValueType.Int);
        }

        /// <summary>
        /// Gets a long value.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The long value</returns>
        public long Long(string name)
        {
            return (long)GetValue(name, ParsedValueType.Long);
        }

        /// <summary>
        /// Gets a string value.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The string value</returns>
        public string String(string name)
        {
            return (string)GetValue(name, ParsedValueType.String);
        }

        /// <summary>
        /// Gets a string array value.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The string array value</returns>
        public string[] StringArray(string name)
        {
            return (string[])GetValue(name, ParsedValueType.StringArray);
        }

        /// <summary>
        /// Gets a bool value.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The bool value</returns>
        public bool Bool(string name)
        {
            return (bool)GetValue(name, ParsedValueType.Bool);
        }

        /// <summary>
        /// Gets a DateTime value.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The DateTime value</returns>
        public DateTime Date(string name)
        {
            return (DateTime)GetValue(name, ParsedValueType.Date);
        }

        #region Internal assignments
        internal void AddInt(string name, int value)
        {
            Values.Add(new ParsedValue()
            {
                Name = name,
                Type = ParsedValueType.Int,
                Value = value
            });
        }

        internal void AddLong(string name, long value)
        {
            Values.Add(new ParsedValue()
            {
                Name = name,
                Type = ParsedValueType.Long,
                Value = value
            });
        }

        internal void AddString(string name, string value)
        {
            Values.Add(new ParsedValue()
            {
                Name = name,
                Type = ParsedValueType.String,
                Value = value
            });
        }

        internal void AddStringArray(string name, string[] value)
        {
            Values.Add(new ParsedValue()
            {
                Name = name,
                Type = ParsedValueType.StringArray,
                Value = value
            });
        }

        internal void AddBool(string name, bool value)
        {
            Values.Add(new ParsedValue()
            {
                Name = name,
                Type = ParsedValueType.Bool,
                Value = value
            });
        }

        internal void AddDate(string name, DateTime value)
        {
            Values.Add(new ParsedValue()
            {
                Name = name,
                Type = ParsedValueType.Date,
                Value = value
            });
        }
        #endregion

        private dynamic GetValue(string name, ParsedValueType type)
        {
            return Values.First(v => v.Name == name && v.Type == type).Value;
        }

        internal void Add(ParsedValue value)
        {
            Values.Add(value);
        }
    }
}
