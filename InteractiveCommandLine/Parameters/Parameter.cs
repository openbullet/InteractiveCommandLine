namespace InteractiveCommandLine.Parameters
{
    internal abstract class Parameter
    {
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal bool Required { get; set; } = false;

        internal virtual string DefaultString { get; } = "Unknown";
    }
}
