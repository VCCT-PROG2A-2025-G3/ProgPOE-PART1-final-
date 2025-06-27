using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class NLP
    {
        // Enum representing possible user intents detected from input
        enum UserIntent
        {
            None,         // No recognized intent
            AddReminder,  // User wants to add a reminder
            AddTask,      // User wants to add a task
            StartQuiz,    // User wants to start the quiz
            ShowSummary   // User wants to see a summary of actions
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Cybersecurity Chatbot!");

            // Main input loop: continuously reads user input
            while (true)
            {
                Console.Write("\nYou: ");
                string userInput = Console.ReadLine();

                // If input is empty or whitespace, prompt user again
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter something.");
                    continue; // Restart loop to get valid input
                }

                // Detect the intent based on user's input text
                UserIntent intent = DetectIntent(userInput);

                // Handle the detected intent with a switch statement
                switch (intent)
                {
                    case UserIntent.AddReminder:
                        // Extract the reminder details from input
                        string reminder = ExtractReminderDescription(userInput);

                        // Provide feedback that reminder has been set
                        Console.WriteLine($"Chatbot: Reminder set for '{reminder}'.");
                        break;

                    case UserIntent.AddTask:
                        // Extract the task description from input
                        string task = ExtractTaskDescription(userInput);

                        // Provide feedback that task has been added and ask about reminder
                        Console.WriteLine($"Chatbot: Task added: '{task}'. Would you like to set a reminder for this task?");
                        break;

                    case UserIntent.StartQuiz:
                        // Inform user that quiz is starting (quiz logic to be added)
                        Console.WriteLine("Chatbot: Starting the cybersecurity quiz...");
                        break;

                    case UserIntent.ShowSummary:
                        // Placeholder to show recent actions summary (logic to be added)
                        Console.WriteLine("Chatbot: Here's a summary of recent actions:");
                        break;

                    default:
                        // Input could not be understood, ask user to rephrase
                        Console.WriteLine("Chatbot: Sorry, I didn't understand. Could you please rephrase?");
                        break;
                }
            }
        }

        // Method to detect the user intent by checking for keywords in the input string
        static UserIntent DetectIntent(string userInput)
        {
            // Normalize input to lowercase for case-insensitive matching
            string input = userInput.ToLower();

            // Check for keywords related to reminders
            if (input.Contains("remind") || input.Contains("reminder") || input.Contains("remember"))
                return UserIntent.AddReminder;

            // Check for keywords related to tasks
            if (input.Contains("add task") || input.Contains("create task") || input.Contains("new task") || input.Contains("task"))
                return UserIntent.AddTask;

            // Check for keywords related to quizzes or tests
            if (input.Contains("quiz") || input.Contains("test") || input.Contains("question"))
                return UserIntent.StartQuiz;

            // Check for commands to show activity summary or past actions
            if (input.Contains("what have you done") || input.Contains("summary") || input.Contains("actions"))
                return UserIntent.ShowSummary;

            // Return None if no keywords match
            return UserIntent.None;
        }

        // Extracts the reminder description by removing common reminder phrases
        static string ExtractReminderDescription(string input)
        {
            // Normalize input to lowercase
            string lowered = input.ToLower();

            // Remove common reminder phrases from input text
            lowered = lowered.Replace("remind me to", "")
                             .Replace("reminder to", "")
                             .Replace("remind me", "");

            // Trim whitespace and punctuation from the beginning and end
            return lowered.Trim(new char[] { ' ', '.', ',' });
        }

        // Extracts the task description by removing common task-related phrases
        static string ExtractTaskDescription(string input)
        {
            // Normalize input to lowercase
            string lowered = input.ToLower();

            // Remove common task-related phrases from input text
            lowered = lowered.Replace("add task", "")
                             .Replace("create task", "")
                             .Replace("new task", "")
                             .Replace("task", "");

            // Trim whitespace and punctuation from the beginning and end
            return lowered.Trim(new char[] { ' ', '.', ',' });
        }
    }
}