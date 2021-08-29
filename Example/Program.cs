using System;
using InteractiveCommandLine;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the Interactive Command Line
            ICL.Initialize(typeof(Program).Assembly);

            // Begin the loop
            while (true)
            {
                Console.Write("> ");
                ICL.ReadAndExecute(null);
            }
        }
    }
}
