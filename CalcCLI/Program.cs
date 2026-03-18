using System;
using CoreEngine;

namespace CalcCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            MathParser parser = new MathParser();

            Console.WriteLine("Math Engine CLI Initialized.");
            Console.WriteLine("Enter a mathematical expression");
            Console.WriteLine("Press Ctrl+C to exit.\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    var tokens = parser.Tokenize(input);
                    double result = parser.Evaluate(tokens);

                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine("Result (Dec): " + result);

                    if (result % 1 == 0)
                    {
                        long intResult = (long)result;
                        Console.WriteLine("Result (Hex): 0x" + intResult.ToString("X"));
                    }

                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }
    }
}
