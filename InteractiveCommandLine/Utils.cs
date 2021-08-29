using InteractiveCommandLine.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InteractiveCommandLine
{
    /// <summary>
    /// Provides various utilities to transform values.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Parses the size in number of bytes from a string
        /// </summary>
        /// <param name="size">The size string to parse from e.g. 10KB or 220MB</param>
        /// <param name="max">Whether to use the max in case the size is empty</param>
        /// <returns>The number of bytes</returns>
        public static long ParseSize(string size, bool max)
        {
            if (string.IsNullOrWhiteSpace(size))
            {
                if (max) return long.MaxValue;
                else return 0;
            }

            var value = long.Parse(Regex.Match(size, "[0-9]*").Value);
            var unit = size.Replace(value.ToString(), "").ToUpper();
            if (unit == "KB") value *= 1000;
            else if (unit == "MB") value *= 1000 * 1000;
            else if (unit == "GB") value *= 1000 * 1000 * 1000;
            return value;
        }

        private static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// Gets the size as a string from a number of bytes.
        /// </summary>
        /// <param name="value">The number of bytes</param>
        /// <param name="decimalPlaces">The decimal places in the returned number</param>
        /// <returns>The string value e.g. 10KB or 220MB</returns>
        public static string GetSizeFromBytes(long value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + GetSizeFromBytes(-value); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        // INTERNAL METHODS

        internal static string ParseValueParameter(string line, string name)
        {
            var split = SplitLine(line);
            var parameterName = name.Length > 1 ? $"--{name}" : $"-{name}";
            var index = split.IndexOf(parameterName);
            if (index == -1) throw new Exception($"The parameter {name} is not present");
            else return split[index + 1];
        }

        internal static string ParsePositionalParameter(string line, int pos, string identifier)
        {
            var split = SplitLine(line.Substring(identifier.Length + 1));
            return split[pos];
        }

        internal static bool IsParameterPresent(string line, string name)
        {
            try
            {
                var split = SplitLine(line);
                var parameterName = name.Length > 1 ? $"--{name}" : $"-{name}";
                return split.Contains(parameterName);
            }
            catch { return false; }
        }

        internal static List<string> SplitLine(string line)
        {
            var pattern = "(^| |,)\"([^\"]*)\"($| |,)";
            var sequence = "_-_-_-_-_-_-_"; // Shitty way but I cannot figure out a decent regex

            foreach (Match match in Regex.Matches(line, pattern))
            {
                string val = match.Groups[2].Value;

                if (val.Length == 0) continue;
                line = line.Replace($"\"{val}\"", val.Replace(" ", sequence));
            }

            return line
                .Split(' ')
                .Select(s => s.Replace(sequence, " "))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }

        internal static bool IsStringParameter(Type type)
        {
            var types = new Type[] { typeof(StringParameter), typeof(EnumParameter), typeof(FileOrFolderParameter) };
            return types.Any(t => t == type);
        }

        internal static bool IsParameter (string text)
        {
            return (text.StartsWith('-') && text.Length == 2) || (text.StartsWith("--"));
        }

        internal static string ParseParameterName (string text)
        {
            if (text.StartsWith('-') && text.Length == 2) return text.Substring(1);
            else return text.Substring(2);
        }
    }
}
