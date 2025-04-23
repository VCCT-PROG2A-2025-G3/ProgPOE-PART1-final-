using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class HandleUserQuestions
    {
        // Private fields to store the user's question and name
        private string question;
        private string userName;

        // Constructor to initialize the question and username
        public HandleUserQuestions(string question, string userName)
        {
            this.question = question;
            this.userName = userName;
        }

        // Method to process the user's question and generate a response
        public void ProcessQuestion()
        {
            // Add a blank line before the bot's response for better spacing
            Console.WriteLine();

            // Check if the question is empty or just whitespace
            if (string.IsNullOrWhiteSpace(question))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                BotAnswers.TypeText("It seems you didn't ask a question. Please ask a question so I can assist you.", ConsoleColor.Green);
                Console.ResetColor();
                return; // Exit the method early if no valid input
            }
        }
    }
}
