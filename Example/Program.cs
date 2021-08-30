using System;
using System.Net.Http;
using InteractiveCommandLine;
using InteractiveCommandLine.Attributes;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var state = new State();

            // Initialize the Interactive Command Line
            ICL.Initialize(typeof(Program).Assembly);

            // Begin the loop
            while (true)
            {
                Console.Write("> ");
                ICL.ReadAndExecute(state);
            }
        }
    }

    // This is the class that holds your state
    // It will be passed to methods that accept it as a first parameter
    public class State
    {
        public int Counter { get; set; } = 1;
    }

    // Define all your commands as static methods
    public static class Commands
    {
        // Add the Command attribute to all your command methods, and the Param attribute to all their parameters
        [Command("greet", "Greets you by name", "greet John")]
        public static void Greet([Param("Your name")] string name)
        {
            Console.WriteLine($"Hello {name} and welcome to ICL!");
        }

        // Commands can optionally accept a state as the first parameter
        // Do not use the Param attribute for this parameter
        [Command("stateful", "Prints the stateful counter's value and increases it by 1", "state")]
        public static void StatefulCommand(State state)
        {
            Console.WriteLine($"The counter inside the state is {state.Counter++}");
        }

        [Command("count", "Counts from 1 to your number", "count 10")]
        public static void Count([Param("The number to count to"), Int(1, 100)] int number)
        {
            for (int i = 1; i < number; i++)
            {
                Console.Write($"{i}, ");
            }

            Console.WriteLine(number);
        }

        [Command("calc", "A simple calculator", "calc 1 + 2", "calc 2 - 1", "calc 2 * 3", "calc 6 / 2")]
        public static void Calc(
            [Param("The left term")] int left,
            [Param("The operation")] string operation,
            [Param("The right term")] int right)
        {
            var result = operation switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => left / right,
                "%" => left % right,
                _ => throw new NotSupportedException("Operation not supported")
            };

            Console.WriteLine(result);
        }

        [Command("geo", "Prints geolocation information for an IP address", "geo 1.1.1.1", "geo 1.1.1.1 --type json")]
        public static void Geo(
            [Param("The ip you want to geolocate")] string ip,
            [Param("The response type")] ResponseType type)
        {
            try
            {
                using var client = new HttpClient();
                using var response = client.GetAsync($"{"http"}://ip-api.com/{type}/{ip}").Result;
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                Console.WriteLine("Could not contact the API!");
            }
        }

        [Command("quit", "Quits the program", "quit")]
        public static void Quit()
        {
            Environment.Exit(0);
        }
    }

    public enum ResponseType
    {
        json,
        xml,
        csv,
        line
    }
}
