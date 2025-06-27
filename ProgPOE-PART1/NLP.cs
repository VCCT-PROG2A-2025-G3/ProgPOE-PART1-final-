using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class NLP
    {
        enum UserIntent
        {
            None,
            AddReminder,
            AddTask,
            StartQuiz,
            ShowSummary
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Cybersecurity Chatbot!");
            while (true)
            {
                Console.Write("\nYou: ");
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter something.");
                    continue;
                }

                UserIntent intent = DetectIntent(userInput);

                switch (intent)
                {
                    case UserIntent.AddReminder:
                        string reminder = ExtractReminderDescription(userInput);
                        Console.WriteLine($"Chatbot: Reminder set for '{reminder}'.");
                        break;

                    case UserIntent.AddTask:
                        string task = ExtractTaskDescription(userInput);
                        Console.WriteLine($"Chatbot: Task added: '{task}'. Would you like to set a reminder for this task?");
                        break;

                    case UserIntent.StartQuiz:
                        Console.WriteLine("Chatbot: Starting the cybersecurity quiz...");
                        // Call your quiz logic here
                        break;

                    case UserIntent.ShowSummary:
                        Console.WriteLine("Chatbot: Here's a summary of recent actions:");
                        // Print your actions summary here
                        break;

                    default:
                        Console.WriteLine("Chatbot: Sorry, I didn't understand. Could you please rephrase?");
                        break;
                }
            }
        }

        static UserIntent DetectIntent(string userInput)
        {
            string input = userInput.ToLower();

            if (input.Contains("remind") || input.Contains("reminder") || input.Contains("remember"))
                return UserIntent.AddReminder;

            if (input.Contains("add task") || input.Contains("create task") || input.Contains("new task") || input.Contains("task"))
                return UserIntent.AddTask;

            if (input.Contains("quiz") || input.Contains("test") || input.Contains("question"))
                return UserIntent.StartQuiz;

            if (input.Contains("what have you done") || input.Contains("summary") || input.Contains("actions"))
                return UserIntent.ShowSummary;

            return UserIntent.None;
        }

        static string ExtractReminderDescription(string input)
        {
            string lowered = input.ToLower();

            lowered = lowered.Replace("remind me to", "")
                             .Replace("reminder to", "")
                             .Replace("remind me", "");

            return lowered.Trim(new char[] { ' ', '.', ',' });
        }

        static string ExtractTaskDescription(string input)
        {
            string lowered = input.ToLower();

            lowered = lowered.Replace("add task", "")
                             .Replace("create task", "")
                             .Replace("new task", "")
                             .Replace("task", "");

            return lowered.Trim(new char[] { ' ', '.', ',' });
        }
    }
}