using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class BotAnswers
    {
        public static void TypeText(string text, ConsoleColor color)
        {
            // Set the text color in the console to the specified colour
            Console.ForegroundColor = color;

            // Print the prefix "Bot: " before displaying the actual message
            Console.Write("Bot: ");

            // Loop through each character in the input text
            foreach (char c in text)
            {
                Console.Write(c);       // Print each character one at a time
                Thread.Sleep(25);       // Add a small delay (25 milliseconds) between characters to create a typewriter effect
            }

            // Move to a new line after the full message is printed
            Console.WriteLine();

            // Reset the console text color to the default to avoid affecting other outputs
            Console.ResetColor();
        }
    }
}
