using System;
using System.IO;

namespace InteractiveCommandLine.Parameters
{
    internal class FileOrFolderParameter : Parameter
    {
        internal string Default { get; set; }

        internal string ParseAndValidate(string value)
        {
            if (!File.Exists(value) && !Directory.Exists(value))
            {
                throw new Exception($"No file or directory found with path {value}");
            }

            return value;
        }
    }
}
