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
        // Fields to store the user's question and name
        private string question;
        private string userName;

        // Component instances for processing
        private SentimentAnalyzer sentimentAnalyzer;
        private ResponseBank responseBank;
        private UserPreferenceManager preferenceManager;
        private TopicFollowUpHandler followUpHandler;

        // Constructor that initializes fields and components
        public HandleUserQuestions(string question, string userName)
        {
            this.question = question;
            this.userName = userName;

            // Initialize all supporting components
            sentimentAnalyzer = new SentimentAnalyzer();
            responseBank = new ResponseBank();
            preferenceManager = new UserPreferenceManager();
            followUpHandler = new TopicFollowUpHandler();

            // Ensure user preferences are initialized and track message count
            preferenceManager.InitializeUserIfNeeded(userName);
            preferenceManager.IncrementMessageCount();
        }

        // Main method to process the user's question
        public void ProcessQuestion()
        {
            Console.WriteLine();

            // Handle empty input
            if (string.IsNullOrWhiteSpace(question))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                BotAnswers.TypeText("It seems you didn't ask a question. Please ask a question so I can assist you.", ConsoleColor.Green);
                Console.ResetColor();
                return;
            }

            // Normalize question input
            question = question.ToLower().Trim();
            Console.ForegroundColor = ConsoleColor.Green;

            // Handle yes/no response to previously asked personalized question
            if (preferenceManager.WaitingForResponse)
            {
                HandleYesNoResponse();
                Console.ResetColor();
                return;
            }

            // Check for a follow-up answer based on previous topic
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

            // Ask a personalized question every 4 messages if the user has preferences
            if (preferenceManager.MessageCount % 4 == 0 &&
                preferenceManager.GetUserPreferences(userName).Count > 0 &&
                !preferenceManager.WaitingForResponse)
            {
                AskPersonalizedQuestion();
                Console.ResetColor();
                return;
            }

            // Respond to generic follow-up prompts like "tell me more"
            if ((question == "tell me more" ||
                 question.Contains("tell me more") ||
                 question == "more" ||
                 question.Contains("more information")) &&
                !string.IsNullOrEmpty(preferenceManager.LastTopic))
            {
                ProvideAllInformationOnTopic(preferenceManager.LastTopic);
                Console.ResetColor();
                return;
            }

            // Detect sentiment in the input
            string detectedSentiment = sentimentAnalyzer.DetectSentiment(question);

            // Process the user's input as a regular question
            ProcessRegularQuestion(detectedSentiment);
            Console.ResetColor();
        }

        // Handle responses to yes/no questions for follow-ups
        private void HandleYesNoResponse()
        {
            if (question.Contains("yes") || question == "y")
            {
                // User is interested in learning more
                ProvideAllInformationOnTopic(preferenceManager.PendingTopic);
                preferenceManager.SetWaitingForResponse(false);
            }
            else if (question.Contains("no") || question == "n")
            {
                // User declined more info
                BotAnswers.TypeText("No problem! Let me know if you have any other questions about cybersecurity.", ConsoleColor.Green);
                preferenceManager.SetWaitingForResponse(false);
            }
            else
            {
                // Prompt user to answer yes/no
                BotAnswers.TypeText("I'm waiting for a yes or no response. Would you like to learn more about this topic?", ConsoleColor.Green);
            }
        }

        // Handle standard question logic with sentiment analysis
        private void ProcessRegularQuestion(string detectedSentiment)
        {
            if (question.Contains("how are you"))
            {
                BotAnswers.TypeText("I'm just a bot, but I'm fully operational and ready to help you stay safe online!", ConsoleColor.Green);
                preferenceManager.SetLastTopic("greeting");
            }
            else if (question.Contains("purpose") || question.Contains("what do you do"))
            {
                BotAnswers.TypeText("My purpose is to help South African citizens learn how to stay safe online by avoiding common cybersecurity threats.", ConsoleColor.Green);
                preferenceManager.SetLastTopic("purpose");
            }
            else if (question.Contains("what can i ask") || question.Contains("help") || question.Contains("topics") || question.Contains("what can you do"))
            {
                BotAnswers.TypeText("You can ask me about topics like:", ConsoleColor.Green);
                BotAnswers.TypeText("- Password safety", ConsoleColor.Green);
                BotAnswers.TypeText("- Phishing scams", ConsoleColor.Green);
                BotAnswers.TypeText("- Safe browsing habits", ConsoleColor.Green);
                BotAnswers.TypeText("- Privacy", ConsoleColor.Green);
                preferenceManager.SetLastTopic("help");
            }
            else if (question.Contains("password"))
            {
                // Handle questions about passwords
                if (followUpHandler.HasSpecificAnswer("password", question))
                {
                    string specificAnswer = followUpHandler.GetSpecificAnswer("password", question);
                    BotAnswers.TypeText(specificAnswer, ConsoleColor.Green);
                }
                else
                {
                    if (!string.IsNullOrEmpty(detectedSentiment))
                        BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);

                    BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.TopicResponses["password"]), ConsoleColor.Green);
                }

                preferenceManager.SetLastTopic("password");

                if (preferenceManager.AddUserPreference(userName, "password"))
                {
                    BotAnswers.TypeText($"I'll remember that you're interested in password security, {userName}. It's a crucial part of staying safe online.", ConsoleColor.Green);
                }
            }
            else if (question.Contains("phish") || question.Contains("scam") || question.Contains("fraud"))
            {
                // Handle phishing/scam-related questions
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
            else if (question.Contains("safe browsing") || question.Contains("surf") || question.Contains("browser safety"))
            {
                // Handle safe browsing-related questions
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
            else if (question.Contains("privacy") || question.Contains("private") || question.Contains("security"))
            {
                // Handle privacy/security-related questions
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
            else if (question.Contains("what do i like") || question.Contains("what am i interested in") || question.Contains("my interests"))
            {
                // Respond with the user's saved interests
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
            else if (!string.IsNullOrEmpty(detectedSentiment) &&
                    (question.Contains("feel") || question.Contains("am") || question.StartsWith("i'm") || question.StartsWith("im")))
            {
                // Handle emotional expressions
                BotAnswers.TypeText(responseBank.GetRandomResponse(responseBank.EmpatheticResponses[detectedSentiment]), ConsoleColor.Green);
                BotAnswers.TypeText("Would you like to learn about a specific cybersecurity topic like passwords, phishing, safe browsing, or privacy?", ConsoleColor.Green);
            }
            else
            {
                // Unknown input
                BotAnswers.TypeText($"Sorry {userName}, I didn't quite understand that.", ConsoleColor.Green);
                BotAnswers.TypeText("Could you please rephrase your question or ask about a specific topic like 'password safety', 'phishing', 'safe browsing' or 'privacy'?", ConsoleColor.Green);
            }
        }

        // Ask a personalized question based on known user interests
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

        // Provide full information on a topic the user showed interest in
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



