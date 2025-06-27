using System;
using System.Collections.Generic;

namespace ProgPOE_PART1
{
    // This class simulates a simple chatbot that can recognize user intents,
    // manage tasks, reminders, quizzes, and keep an activity log of actions performed.
    class ActivityLog
    {
        // List to store the activity log entries with timestamps and descriptions
        private List<string> activityLog = new List<string>();

        // The main loop of the chatbot that interacts with the user
        public void Run()
        {
            Console.WriteLine("Welcome to the Cybersecurity Chatbot!");

            // Infinite loop to continuously process user input until manually stopped
            while (true)
            {
                Console.Write("\nYou: ");
                string userInput = Console.ReadLine();

                // If the user inputs an empty string or only spaces, prompt again
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Please enter something.");
                    continue; // Skip to the next iteration to get valid input
                }

                // Detect the user's intent based on their input text
                var intent = DetectIntent(userInput);

                // Switch statement to handle each recognized intent accordingly
                switch (intent)
                {
                    case UserIntent.AddReminder:
                        // Extract reminder details from user input
                        var reminder = ExtractReminderDescription(userInput);

                        // Log the action with timestamp
                        AddLogEntry($"Reminder set: '{reminder}'");

                        // Provide feedback to the user
                        Console.WriteLine($"Chatbot: Reminder set for '{reminder}'.");
                        break;

                    case UserIntent.AddTask:
                        // Extract task description from user input
                        var task = ExtractTaskDescription(userInput);

                        // Log the task addition
                        AddLogEntry($"Task added: '{task}'");

                        // Provide feedback and offer to set a reminder
                        Console.WriteLine($"Chatbot: Task added: '{task}'. Would you like to set a reminder for this task?");
                        break;

                    case UserIntent.StartQuiz:
                        // Log that the quiz has started
                        AddLogEntry("Quiz started.");

                        // Inform the user that the quiz is starting
                        Console.WriteLine("Chatbot: Starting the cybersecurity quiz...");
                        // Placeholder: integrate actual quiz logic here
                        break;

                    case UserIntent.ShowSummary:
                        // Show the recent activity log to the user
                        ShowActivityLog();
                        break;

                    default:
                        // If the input is not understood, ask the user to rephrase
                        Console.WriteLine("Chatbot: Sorry, I didn't understand. Could you please rephrase?");
                        break;
                }
            }
        }

        // Displays the last recorded actions in the activity log to the user
        private void ShowActivityLog()
        {
            // If no actions have been logged yet, inform the user
            if (activityLog.Count == 0)
            {
                Console.WriteLine("Chatbot: No recent actions logged yet.");
                return;
            }

            // Print a header message
            Console.WriteLine("Chatbot: Here’s a summary of recent actions:");

            // Loop through all activity log entries and display them numbered
            for (int i = 0; i < activityLog.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {activityLog[i]}");
            }
        }

        // Adds a new entry to the activity log with a timestamp
        private void AddLogEntry(string actionDescription)
        {
            // Format current time as "YYYY-MM-DD HH:mm:ss"
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Add the new entry combining timestamp and action description
            activityLog.Add($"{timestamp}: {actionDescription}");

            // Maintain a maximum of 10 entries in the log for readability
            if (activityLog.Count > 10)
            {
                // Remove the oldest entry (at index 0)
                activityLog.RemoveAt(0);
            }
        }

        // Enum to represent recognized user intents for clarity in code
        private enum UserIntent
        {
            None,        // No recognized intent
            AddReminder, // User wants to add a reminder
            AddTask,     // User wants to add a task
            StartQuiz,   // User wants to start the quiz
            ShowSummary  // User wants to see the activity log summary
        }

        // Method to detect user intent by checking keywords in the input string
        private UserIntent DetectIntent(string userInput)
        {
            // Convert input to lowercase for case-insensitive matching
            string input = userInput.ToLower();

            // Check for keywords related to reminders
            if (input.Contains("remind") || input.Contains("reminder") || input.Contains("remember"))
                return UserIntent.AddReminder;

            // Check for keywords related to tasks
            if (input.Contains("add task") || input.Contains("create task") || input.Contains("new task") || input.Contains("task"))
                return UserIntent.AddTask;

            // Check for keywords related to quizzes
            if (input.Contains("quiz") || input.Contains("test") || input.Contains("question"))
                return UserIntent.StartQuiz;

            // Check for commands to show the activity log or summary of actions
            if (input.Contains("show activity log") || input.Contains("what have you done") || input.Contains("summary") || input.Contains("actions"))
                return UserIntent.ShowSummary;

            // If no keywords matched, return None
            return UserIntent.None;
        }

        // Extracts the reminder text from user input by removing common phrases
        private string ExtractReminderDescription(string input)
        {
            string lowered = input.ToLower();

            // Remove common leading phrases related to reminders
            lowered = lowered.Replace("remind me to", "")
                             .Replace("reminder to", "")
                             .Replace("remind me", "");

            // Trim spaces and punctuation from the start and end
            return lowered.Trim(new char[] { ' ', '.', ',' });
        }

        // Extracts the task description from user input by removing task command phrases
        private string ExtractTaskDescription(string input)
        {
            string lowered = input.ToLower();

            // Remove common phrases used to add tasks
            lowered = lowered.Replace("add task", "")
                             .Replace("create task", "")
                             .Replace("new task", "")
                             .Replace("task", "");

            // Trim spaces and punctuation from the start and end
            return lowered.Trim(new char[] { ' ', '.', ',' });
        }
    }
}

