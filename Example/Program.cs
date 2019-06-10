using System;
using System.Net;
using InteractiveCommandLine;
using InteractiveCommandLine.Parameters;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the Interactive Command Line
            ICL.Initialize();

            // Setup all the available commands
            ICL.AddCommand(
                "greet",
                new Action<Parsed>((parsed) => Console.WriteLine($"Hello {parsed.String("name")} and welcome to ICL!")),
                new Parameter[] { new StringParameter("name", "Your name", "", true)},
                "Greets you by name",
                new string[] {"greet John"}
            );

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

            ICL.AddCommand(
                "calc",
                new Action<Parsed>((parsed) => Calc(parsed.Int("first"), parsed.String("operation"), parsed.Int("second"))),
                new Parameter[] 
                {
                    new IntParameter("first", "The first number", "0", true),
                    new EnumParameter("operation", new string[] { "+", "-", "*", "/" }, "The operation to execute", "", true),
                    new IntParameter("second", "The second number", "0", true)
                },
                "A simple calculator",
                new string[] 
                {
                    "calc 1 + 2",
                    "calc 2 - 1",
                    "calc 2 * 3",
                    "calc 6 / 2"
                }
            );

            ICL.AddCommand(
                "geo",
                new Action<Parsed>((parsed) => Geo(parsed.String("ip"), parsed.String("type"))),
                new Parameter[]
                {
                    new StringParameter("ip", "The ip you want to geolocate", "", true),
                    new EnumParameter("type", typeof(ResponseType), "The response type", "json")
                },
                "Prints geolocation information for an IP address",
                new string[]
                {
                    "geo 1.1.1.1",
                    "geo 1.1.1.1 --type json"
                }
            );

            ICL.AddCommand(
                "quit",
                new Action<Parsed>((parsed) => Environment.Exit(0)),
                new Parameter[] { },
                "Quits the program",
                new string[] { "quit" }
            );

            // Begin the loop
            while (true)
            {
                Console.Write("> ");
                ICL.ReadAndExecute();
            }
        }

        static void Calc(int first, string operation, int second)
        {
            int result = 0;
            switch (operation)
            {
                case "+":
                    result = first + second;
                    break;

                case "-":
                    result = first - second;
                    break;

                case "*":
                    result = first * second;
                    break;

                case "/":
                    result = first / second;
                    break;
            }

            Console.WriteLine(result);
        }

        enum ResponseType
        {
            json,
            xml,
            csv,
            line
        }

        static void Geo(string ip, string type)
        {
            try
            {
                var client = new WebClient();
                var result = client.DownloadString($"http://ip-api.com/{type}/{ip}");
                Console.WriteLine(result);
            }
            catch
            {
                Console.WriteLine("Could not contact the API!");
            }
        }
    }
}
