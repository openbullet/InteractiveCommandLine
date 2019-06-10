# InteractiveCommandLine
A library to make shell-based programs, based on the amazing [ReadLine](https://github.com/tonerdo/readline) library.

# Usage
You can see the [Example](https://github.com/openbullet/InteractiveCommandLine/blob/master/Example/Program.cs) console program or read below.

First of all initialize ICL
```csharp
ICL.Initialize();
```
Then add all the commands you need, for example
```csharp
ICL.AddCommand(
    "count",
    new Action<Parsed>((parsed) => 
    {
        for (int i = 1; i <= parsed.Int("number"); i++)
        {
            Console.Write($"{i}, ");
        }
        Console.WriteLine();
    }),
    new Parameter[] { new IntParameter("number", "The number to count to", "0", true, 0, 100) },
    "Counts from 1 to your number",
    new string[] { "count 10" }
);
```

And finally make an endless loop
```csharp
while (true)
{
    Console.Write("> ");
    ICL.ReadAndExecute();
}
```

To display the auto generated documentation, type `help` in the console.

# Parameters
Parameters can be positional (no need to specify their name) or not.
Non-positional parameters must be specified with `--name value` or `-n value` if their name is a single character.
Bool parameters do not need a value, they just need to be present like `--bool`.

# Auto Completion
You can set Auto Completion lists using
```csharp
ICL.SetAutoCompletion("numbers", new string[] { "one", "two", "three" });
```
and then in the parameter that needs to be auto completed you can specify 
```csharp
new StringParameter("name", "Description") { AutoCompleteList = "numbers" };
```

To autocomplete folder names and file names from the filesystem you can use the `FileOrFolderParameter` type.

# TODO
- autocompletion of double quotes in files/folders with spaces
- multiple aliases for command names and parameter names
- parameter aggregation e.g. -xvcf
- mutually exclusive parameters
