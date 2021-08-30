# InteractiveCommandLine
A library to make shell-based programs, based on the amazing [ReadLine](https://github.com/tonerdo/readline) library.

# Usage
Check out the [Example](https://github.com/openbullet/InteractiveCommandLine/blob/master/Example/Program.cs) console program.

# Parameters
Parameters can be positional (no need to specify their name) or not.
Non-positional parameters must be specified with `--name value` or `-n value` if their name is a single character.
Bool parameters do not need a value, they just need to be present like `--bool`.

# TODO
- autocompletion of double quotes in files/folders with spaces
- multiple aliases for command names and parameter names
- parameter aggregation e.g. -xvcf
- mutually exclusive parameters
- catch Ctrl+C to quit
