using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    // Main class that processes user questions
    class HandleUserQuestions
    {
        // Fields to store the user's input question and username
        private string question;
        private string userName;

        // Supporting components used for processing the question
        private SentimentAnalyzer sentimentAnalyzer;        // For analyzing sentiment in the user's question
        private ResponseBank responseBank;                  // Contains predefined responses for various topics
        private UserPreferenceManager preferenceManager;    // Manages user's preferences and interaction history
        private TopicFollowUpHandler followUpHandler;       // Handles follow-up questions on previously discussed topics

        // Constructor to initialize fields and supporting components
        public HandleUserQuestions(string question, string userName)
        {
            this.question = question;       // Store the user's question
            this.userName = userName;       // Store the user's name

            // Create instances of each component
            sentimentAnalyzer = new SentimentAnalyzer();
            responseBank = new ResponseBank();
            preferenceManager = new UserPreferenceManager();
            followUpHandler = new TopicFollowUpHandler();

            // Ensure the user's preferences are initialized in memory
            preferenceManager.InitializeUserIfNeeded(userName);

            // Increment the user's message count for tracking engagement
            preferenceManager.IncrementMessageCount();
        }

        // Main method to process the user's question and generate a response
        public void ProcessQuestion()
        {
            Console.WriteLine(); // Add a blank line before the bot's response

            // If the question is empty or whitespace, prompt the user to ask something
            if (string.IsNullOrWhiteSpace(question))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                BotAnswers.TypeText("It seems you didn't ask a question. Please ask a question so I can assist you.", ConsoleColor.Green);
                Console.ResetColor();
                return;
            }

            // Normalize the question input (convert to lowercase and remove extra spaces)
            question = question.ToLower().Trim();
            Console.ForegroundColor = ConsoleColor.Green;

            // If the bot is waiting for a yes/no answer from the user (based on a previous question)
            if (preferenceManager.WaitingForResponse)
            {
                HandleYesNoResponse();  // Process the yes/no reply
                Console.ResetColor();
                return;
            }

            // Check if there's a relevant follow-up response for the user's current question
            if (!string.IsNullOrEmpty(preferenceManager.LastTopic))
            {
                if (followUpHandler.HasSpecificAnswer(preferenceManager.LastTopic, question))
                {
                    string specificAnswer = followUpHandler.GetSpecificAnswer(preferenceManager.LastTopic, question);
                    BotAnswers.TypeText(specificAnswer, ConsoleColor.Green);
                    Console.ResetColor();
                    return;
                }
            }

            // Ask a personalized question every 4 messages if the user has preferences and no pending follow-up
            if (preferenceManager.MessageCount % 4 == 0 &&
                preferenceManager.GetUserPreferences(userName).Count > 0 &&
                !preferenceManager.WaitingForResponse)
            {
                AskPersonalizedQuestion();
                Console.ResetColor();
                return;
            }

            // If the question is a generic follow-up prompt like "tell me more"
            if ((question == "tell me more" ||
                 question.Contains("tell me more") ||
                 question == "more" ||
                 question.Contains("more information")) &&
                !string.IsNullOrEmpty(preferenceManager.LastTopic))
            {
                ProvideAllInformationOnTopic(preferenceManager.LastTopic); // Provide detailed info
                Console.ResetColor();
                return;
            }

            // Detect the sentiment (e.g., positive, negative, neutral) from the input
            string detectedSentiment = sentimentAnalyzer.DetectSentiment(question);

            // Process the input as a standard cybersecurity-related question
            ProcessRegularQuestion(detectedSentiment);
            Console.ResetColor();
        }

        // Handle yes/no responses to personalized or follow-up questions
        private void HandleYesNoResponse()
        {
            // If the user says yes
            if (question.Contains("yes") || question == "y")
            {
                ProvideAllInformationOnTopic(preferenceManager.PendingTopic); // Provide full info
                preferenceManager.SetWaitingForResponse(false); // Reset waiting state
            }
            // If the user says no
            else if (question.Contains("no") || question == "n")
            {
                BotAnswers.TypeText("No problem! Let me know if you have any other questions about cybersecurity.", ConsoleColor.Green);
                preferenceManager.SetWaitingForResponse(false);
            }
            // If unclear response, prompt again
            else
            {
                BotAnswers.TypeText("I'm waiting for a yes or no response. Would you like to learn more about this topic?", ConsoleColor.Green);
            }
        }

        // Process general user questions and sentiment-based replies
        private void ProcessRegularQuestion(string detectedSentiment)
        {
            // Greeting check
            if (question.Contains("how are you"))
            {
                BotAnswers.TypeText("I'm just a bot, but I'm fully operational and ready to help you stay safe online!", ConsoleColor.Green);
                preferenceManager.SetLastTopic("greeting");
            }
            // Asking about bot's purpose
            else if (question.Contains("purpose") || question.Contains("what do you do"))
            {
                BotAnswers.TypeText("My purpose is to help South African citizens learn how to stay safe online by avoiding common cybersecurity threats.", ConsoleColor.Green);
                preferenceManager.SetLastTopic("purpose");
            }
            // Asking for help or available topics
            else if (question.Contains("what can i ask") || question.Contains("help") || question.Contains("topics") || question.Contains("what can you do"))
            {
                BotAnswers.TypeText("You can ask me about topics like:", ConsoleColor.Green);
                BotAnswers.TypeText("- Password safety", ConsoleColor.Green);
                BotAnswers.TypeText("- Phishing scams", ConsoleColor.Green);
                BotAnswers.TypeText("- Safe browsing habits", ConsoleColor.Green);
                BotAnswers.TypeText("- Privacy", ConsoleColor.Green);
                preferenceManager.SetLastTopic("help");
            }
            // Question about passwords
            else if (question.Contains("password"))
            {
                // Check for specific predefined answers
                if (followUpHandler.HasSpecificAnswer("password", question))
                {
                    string specificAnswer = followUpHandler.GetSpecificAnswer("password", question);
                    BotAnswers.TypeText(specificAnswer, ConsoleColor.Green);
                }
                else
                {
                    // Respond empathetically based on sentiment
                    if (!string.IsNullOrEmpty(detectedSentiment))
                        BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);

                    // Provide a general password-related answer
                    BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.TopicResponses["password"]), ConsoleColor.Green);
                }

                preferenceManager.SetLastTopic("password");

                // Save user's interest in password security
                if (preferenceManager.AddUserPreference(userName, "password"))
                {
                    BotAnswers.TypeText($"I'll remember that you're interested in password security, {userName}. It's a crucial part of staying safe online.", ConsoleColor.Green);
                }
            }
            // Question about phishing or scams
            else if (question.Contains("phish") || question.Contains("scam") || question.Contains("fraud"))
            {
                if (followUpHandler.HasSpecificAnswer("phishing", question))
                {
                    string specificAnswer = followUpHandler.GetSpecificAnswer("phishing", question);
                    BotAnswers.TypeText(specificAnswer, ConsoleColor.Green);
                }
                else
                {
                    if (!string.IsNullOrEmpty(detectedSentiment))
                        BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);

                    BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.TopicResponses["phishing"]), ConsoleColor.Green);
                }

                preferenceManager.SetLastTopic("phishing");

                if (preferenceManager.AddUserPreference(userName, "phishing"))
                {
                    BotAnswers.TypeText($"I'll remember that you're interested in phishing prevention, {userName}. Staying alert to these scams is essential!", ConsoleColor.Green);
                }
            }
            // Question about safe browsing
            else if (question.Contains("safe browsing") || question.Contains("surf") || question.Contains("browser safety"))
            {
                if (followUpHandler.HasSpecificAnswer("browsing", question))
                {
                    string specificAnswer = followUpHandler.GetSpecificAnswer("browsing", question);
                    BotAnswers.TypeText(specificAnswer, ConsoleColor.Green);
                }
                else
                {
                    if (!string.IsNullOrEmpty(detectedSentiment))
                        BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);

                    BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.TopicResponses["browsing"]), ConsoleColor.Green);
                }

                preferenceManager.SetLastTopic("browsing");

                if (preferenceManager.AddUserPreference(userName, "browsing"))
                {
                    BotAnswers.TypeText($"I'll remember that you're interested in safe browsing, {userName}. Smart browsing habits are key to online security!", ConsoleColor.Green);
                }
            }
            // Question about privacy/security
            else if (question.Contains("privacy") || question.Contains("private") || question.Contains("security"))
            {
                if (followUpHandler.HasSpecificAnswer("privacy", question))
                {
                    string specificAnswer = followUpHandler.GetSpecificAnswer("privacy", question);
                    BotAnswers.TypeText(specificAnswer, ConsoleColor.Green);
                }
                else
                {
                    if (!string.IsNullOrEmpty(detectedSentiment))
                        BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);

                    BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.TopicResponses["privacy"]), ConsoleColor.Green);
                }

                preferenceManager.SetLastTopic("privacy");

                if (preferenceManager.AddUserPreference(userName, "privacy"))
                {
                    BotAnswers.TypeText($"I'll remember that you're interested in privacy, {userName}. It's a crucial part of staying safe online.", ConsoleColor.Green);
                }
            }
            // Asking about their interests
            else if (question.Contains("what do i like") || question.Contains("what am i interested in") || question.Contains("my interests"))
            {
                List<string> interests = preferenceManager.GetUserPreferences(userName);
                if (interests.Count > 0)
                {
                    BotAnswers.TypeText($"Based on our conversation, you've shown interest in:", ConsoleColor.Green);
                    foreach (string interest in interests)
                    {
                        BotAnswers.TypeText($"- {interest}", ConsoleColor.Green);
                    }
                }
                else
                {
                    BotAnswers.TypeText($"We haven't discussed your specific interests yet, {userName}. Feel free to ask about password safety, phishing, safe browsing, or privacy!", ConsoleColor.Green);
                }
            }
            // If user expresses an emotion
            else if (!string.IsNullOrEmpty(detectedSentiment) &&
                    (question.Contains("feel") || question.Contains("am") || question.StartsWith("i'm") || question.StartsWith("im")))
            {
                BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);
                BotAnswers.TypeText("Would you like to learn about a specific cybersecurity topic like passwords, phishing, safe browsing, or privacy?", ConsoleColor.Green);
            }
            // Fallback if input is not understood
            else
            {
                BotAnswers.TypeText($"Sorry {userName}, I didn't quite understand that.", ConsoleColor.Green);
                BotAnswers.TypeText("Could you please rephrase your question or ask about a specific topic like 'password safety', 'phishing', 'safe browsing' or 'privacy'?", ConsoleColor.Green);
            }
        }

        // Asks a personalized question related to a known user interest
        private void AskPersonalizedQuestion()
        {
            List<string> userInterests = preferenceManager.GetUserPreferences(userName);
            if (userInterests.Count == 0)
                return;

            Random random = new Random();
            string interest = userInterests[random.Next(userInterests.Count)];

            if (responseBank.PersonalizedQuestions.ContainsKey(interest))
            {
                string question = responseBank.PersonalizedQuestions[interest];
                BotAnswers.TypeText(question, ConsoleColor.Green);
                preferenceManager.SetWaitingForResponse(true, interest);
            }
        }

        // Provides all stored information on a given topic
        private void ProvideAllInformationOnTopic(string topic)
        {
            BotAnswers.TypeText($"Here's everything I know about {topic}:", ConsoleColor.Green);

            if (responseBank.TopicResponses.ContainsKey(topic))
            {
                List<string> allResponses = responseBank.TopicResponses[topic];
                foreach (string response in allResponses)
                {
                    BotAnswers.TypeText($"• {response}", ConsoleColor.Green);
                }

                BotAnswers.TypeText("Is there anything specific about this topic you'd like to know more about?", ConsoleColor.Green);
            }
            else
            {
                BotAnswers.TypeText("Sorry, I don't have detailed information about that topic yet.", ConsoleColor.Green);
            }
        }
    }
}



