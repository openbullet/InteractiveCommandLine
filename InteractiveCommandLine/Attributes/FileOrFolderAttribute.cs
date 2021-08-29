using System;

namespace InteractiveCommandLine.Attributes
{
    /// <summary>
    /// Specifies that a string parameter supports files and folders autocompletion.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FileOrFolderAttribute : Attribute
    {

    }
}
