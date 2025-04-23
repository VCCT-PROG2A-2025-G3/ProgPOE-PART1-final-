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
            // Convert the question to lowercase and trim spaces for consistent matching
            question = question.ToLower().Trim();

            // Set bot response color to green
            Console.ForegroundColor = ConsoleColor.Green;

            // Match different types of questions using simple keyword checking
            if (question.Contains("how are you"))
            {
                BotAnswers.TypeText("I'm just a bot, but I'm fully operational and ready to help you stay safe online!", ConsoleColor.Green);
            }
            else if (question.Contains("purpose") || question.Contains("what do you do"))
            {
                BotAnswers.TypeText("My purpose is to help South African citizens learn how to stay safe online by avoiding common cybersecurity threats.", ConsoleColor.Green);
            }
            else if (question.Contains("what can i ask") || question.Contains("help") || question.Contains("topics") || question.Contains("what can you do"))
            {
                BotAnswers.TypeText("You can ask me about topics like:", ConsoleColor.Green);
                BotAnswers.TypeText("- Password safety", ConsoleColor.Green);
                BotAnswers.TypeText("- Phishing scams", ConsoleColor.Green);
                BotAnswers.TypeText("- Safe browsing habits", ConsoleColor.Green);
            }
            else if (question.Contains("password"))
            {
                BotAnswers.TypeText("Always use strong, unique passwords. Include letters, numbers, and symbols. Avoid using the same password across sites.", ConsoleColor.Green);
            }
            else if (question.Contains("phish") || question.Contains("scam") || question.Contains("fraud"))
            {
                BotAnswers.TypeText("Phishing is a scam where attackers trick you into giving up personal info. Never click suspicious links or attachments in emails.", ConsoleColor.Green);
            }
            else if (question.Contains("safe browsing") || question.Contains("surf") || question.Contains("browser safety"))
            {
                BotAnswers.TypeText("Always check for HTTPS in the browser URL. Don't visit sketchy websites, and keep your browser and antivirus up to date.", ConsoleColor.Green);
            }
            else
            {
                // Default response for unrecognized input
                BotAnswers.TypeText($"Sorry {userName}, I didn't quite understand that.", ConsoleColor.Green);
                BotAnswers.TypeText("Could you please rephrase your question or ask about a specific topic like 'password safety', 'phishing', or 'safe browsing'?", ConsoleColor.Green);
            }
            // Reset console color to default
            Console.ResetColor();
        }
    }
}



