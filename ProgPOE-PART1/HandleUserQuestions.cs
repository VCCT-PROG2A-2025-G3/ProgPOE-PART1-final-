using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class HandleUserQuestions
    {
        private string question;
        private string userName;
        private static Random random = new Random(); // Shared random instance

        // Static fields to maintain state across instances
        private static string lastTopic = "";
        private static Dictionary<string, List<string>> userPreferences = new Dictionary<string, List<string>>();
        private static int messageCount = 0; // To track conversation progress
        private static bool waitingForResponse = false; // Flag to track if we're waiting for yes/no
        private static string pendingTopic = ""; // Topic waiting for yes/no response

        // Sentiment keywords
        private Dictionary<string, List<string>> sentiments = new Dictionary<string, List<string>>
    {
        { "worried", new List<string> { "worried", "concerned", "afraid", "scared", "anxious", "nervous", "fear", "stress", "stressed" } },
        { "curious", new List<string> { "curious", "interested", "want to learn", "tell me", "how do", "how can", "how does", "what is" } },
        { "frustrated", new List<string> { "frustrated", "annoyed", "confused", "difficult", "hard", "complicated", "don't understand", "can't figure", "struggling" } },
        { "overwhelmed", new List<string> { "overwhelmed", "too much", "complex", "complicated", "difficult", "challenging", "lost", "helpless" } }
    };

        // Empathetic responses based on sentiment
        private Dictionary<string, List<string>> empatheticResponses = new Dictionary<string, List<string>>
    {
        { "worried", new List<string>
            {
                "It's completely understandable to feel worried. Cybersecurity can seem intimidating, but with a few simple habits, you can greatly reduce your risk.",
                "I understand your concern. Many people feel the same way. Let me help you with some practical steps to stay safe.",
                "Your concerns are valid. The digital world can be scary, but I'm here to help you navigate it safely."
            }
        },
        { "curious", new List<string>
            {
                "I'm glad you're curious about this! Learning about cybersecurity is the first step to staying safe online.",
                "That's a great question! I'm happy to share what I know about this topic.",
                "Your curiosity will serve you well! Let me explain this in a way that's easy to understand."
            }
        },
        { "frustrated", new List<string>
            {
                "I understand this can be frustrating. Let me try to explain it more clearly.",
                "It's okay to feel frustrated. Cybersecurity can be complex, but we can break it down into simpler steps.",
                "I hear your frustration. Let's take a step back and approach this differently."
            }
        },
        { "overwhelmed", new List<string>
            {
                "It's normal to feel overwhelmed. Let's focus on just one simple step you can take today.",
                "I understand this might seem like a lot. We can take it one small step at a time.",
                "Many people feel overwhelmed by cybersecurity. Let's start with the basics and build from there."
            }
        }
    };

        // Predefined response banks
        private List<string> phishingResponses = new List<string>
    {
        "Phishing is a scam where attackers trick you into giving up personal info. Never click suspicious links or attachments in emails.",
        "Always double-check the sender's email address. If something feels off, don't click!",
        "Beware of emails urging immediate action. They're often phishing attempts!",
        "When in doubt, contact the sender through a known, official method before responding to emails with links or attachments."
    };

        private List<string> passwordResponses = new List<string>
    {
        "Always use strong, unique passwords. Include letters, numbers, and symbols.",
        "Avoid reusing the same password on multiple sites.",
        "Consider using a password manager to generate and store secure passwords.",
        "Change your passwords regularly and never share them."
    };

        private List<string> safeBrowsingResponses = new List<string>
    {
        "Always check for HTTPS in the browser URL to ensure the site is secure.",
        "Avoid clicking on pop-ups or ads, especially on unfamiliar websites.",
        "Keep your browser and antivirus software up to date for maximum protection.",
        "Don't download files from untrusted or suspicious websites.",
        "Use a secure and privacy-focused browser, and consider enabling private browsing mode."
    };

        private List<string> privacyResponses = new List<string>
    {
        "Use strong, unique passwords and enable multifactor authentication.",
        "Limit personal information shared on social media.",
        "Be cautious when clicking links or downloading attachments.",
        "Review app permissions and disable those that aren't needed."
    };

        // Personalized questions based on interests
        private Dictionary<string, string> personalizedQuestions = new Dictionary<string, string>
    {
        { "password", "Since you're interested in password security, would you like to learn all my tips about creating and managing secure passwords? (yes/no)" },
        { "phishing", "As someone concerned about phishing, would you like to know all my tips for identifying and avoiding phishing scams? (yes/no)" },
        { "browsing", "Since you're interested in safe browsing, would you like to learn all my tips for staying safe while surfing the web? (yes/no)" },
        { "privacy", "As someone concerned about privacy, would you like to know all my recommendations for protecting your personal information online? (yes/no)" }
    };

        public HandleUserQuestions(string question, string userName)
        {
            this.question = question;
            this.userName = userName;

            // Initialize user preferences if this is the first time
            if (!userPreferences.ContainsKey(userName))
            {
                userPreferences[userName] = new List<string>();
            }

            messageCount++;
        }

        public void ProcessQuestion()
        {
            Console.WriteLine();
            if (string.IsNullOrWhiteSpace(question))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                BotAnswers.TypeText("It seems you didn't ask a question. Please ask a question so I can assist you.", ConsoleColor.Green);
                Console.ResetColor();
                return;
            }

            question = question.ToLower().Trim();
            Console.ForegroundColor = ConsoleColor.Green;

            // Check if we're waiting for a yes/no response to a personalized question
            if (waitingForResponse)
            {
                if (question.Contains("yes") || question == "y")
                {
                    // User said yes, provide all information on the pending topic
                    ProvideAllInformationOnTopic(pendingTopic);
                    waitingForResponse = false;
                    pendingTopic = "";
                }
                else if (question.Contains("no") || question == "n")
                {
                    // User said no
                    BotAnswers.TypeText("No problem! Let me know if you have any other questions about cybersecurity.", ConsoleColor.Green);
                    waitingForResponse = false;
                    pendingTopic = "";
                }
                else
                {
                    // User didn't say yes or no
                    BotAnswers.TypeText("I'm waiting for a yes or no response. Would you like to learn more about this topic?", ConsoleColor.Green);
                }

                Console.ResetColor();
                return;
            }

            // Check if it's time to offer a personalized question (every 3-5 messages)
            if (messageCount % 4 == 0 && userPreferences[userName].Count > 0 && !waitingForResponse)
            {
                AskPersonalizedQuestion();
                Console.ResetColor();
                return;
            }

            // Check for follow-up requests like "tell me more"
            if ((question == "tell me more" ||
                 question.Contains("tell me more") ||
                 question == "more" ||
                 question.Contains("more information")) &&
                !string.IsNullOrEmpty(lastTopic))
            {
                // Provide all information about the last topic
                ProvideAllInformationOnTopic(lastTopic);
                Console.ResetColor();
                return;
            }

            // Detect sentiment in the user's question
            string detectedSentiment = DetectSentiment(question);

            // Process regular questions with sentiment awareness
            if (question.Contains("how are you"))
            {
                BotAnswers.TypeText("I'm just a bot, but I'm fully operational and ready to help you stay safe online!", ConsoleColor.Green);
                lastTopic = "greeting";
            }
            else if (question.Contains("purpose") || question.Contains("what do you do"))
            {
                BotAnswers.TypeText("My purpose is to help South African citizens learn how to stay safe online by avoiding common cybersecurity threats.", ConsoleColor.Green);
                lastTopic = "purpose";
            }
            else if (question.Contains("what can i ask") || question.Contains("help") || question.Contains("topics") || question.Contains("what can you do"))
            {
                BotAnswers.TypeText("You can ask me about topics like:", ConsoleColor.Green);
                BotAnswers.TypeText("- Password safety", ConsoleColor.Green);
                BotAnswers.TypeText("- Phishing scams", ConsoleColor.Green);
                BotAnswers.TypeText("- Safe browsing habits", ConsoleColor.Green);
                BotAnswers.TypeText("- Privacy", ConsoleColor.Green);
                lastTopic = "help";
            }
            else if (question.Contains("password"))
            {
                // Respond with empathy if sentiment is detected
                if (!string.IsNullOrEmpty(detectedSentiment))
                {
                    BotAnswers.TypeText(GetRandomResponse(empatheticResponses[detectedSentiment]), ConsoleColor.Green);
                }

                BotAnswers.TypeText(GetRandomResponse(passwordResponses), ConsoleColor.Green);
                lastTopic = "password";

                // Remember user interest if not already stored
                if (!userPreferences[userName].Contains("password"))
                {
                    userPreferences[userName].Add("password");
                    BotAnswers.TypeText($"I'll remember that you're interested in password security, {userName}. It's a crucial part of staying safe online.", ConsoleColor.Green);
                }
            }
            else if (question.Contains("phish") || question.Contains("scam") || question.Contains("fraud"))
            {
                // Respond with empathy if sentiment is detected
                if (!string.IsNullOrEmpty(detectedSentiment))
                {
                    BotAnswers.TypeText(GetRandomResponse(empatheticResponses[detectedSentiment]), ConsoleColor.Green);
                }

                BotAnswers.TypeText(GetRandomResponse(phishingResponses), ConsoleColor.Green);
                lastTopic = "phishing";

                // Remember user interest if not already stored
                if (!userPreferences[userName].Contains("phishing"))
                {
                    userPreferences[userName].Add("phishing");
                    BotAnswers.TypeText($"I'll remember that you're interested in phishing prevention, {userName}. Staying alert to these scams is essential!", ConsoleColor.Green);
                }
            }
            else if (question.Contains("safe browsing") || question.Contains("surf") || question.Contains("browser safety"))
            {
                // Respond with empathy if sentiment is detected
                if (!string.IsNullOrEmpty(detectedSentiment))
                {
                    BotAnswers.TypeText(GetRandomResponse(empatheticResponses[detectedSentiment]), ConsoleColor.Green);
                }

                BotAnswers.TypeText(GetRandomResponse(safeBrowsingResponses), ConsoleColor.Green);
                lastTopic = "browsing";

                // Remember user interest if not already stored
                if (!userPreferences[userName].Contains("browsing"))
                {
                    userPreferences[userName].Add("browsing");
                    BotAnswers.TypeText($"I'll remember that you're interested in safe browsing, {userName}. Smart browsing habits are key to online security!", ConsoleColor.Green);
                }
            }
            else if (question.Contains("privacy") || question.Contains("private") || question.Contains("security"))
            {
                // Respond with empathy if sentiment is detected
                if (!string.IsNullOrEmpty(detectedSentiment))
                {
                    BotAnswers.TypeText(GetRandomResponse(empatheticResponses[detectedSentiment]), ConsoleColor.Green);
                }

                BotAnswers.TypeText(GetRandomResponse(privacyResponses), ConsoleColor.Green);
                lastTopic = "privacy";

                // Remember user interest if not already stored
                if (!userPreferences[userName].Contains("privacy"))
                {
                    userPreferences[userName].Add("privacy");
                    BotAnswers.TypeText($"I'll remember that you're interested in privacy, {userName}. It's a crucial part of staying safe online.", ConsoleColor.Green);
                }
            }
            else if (question.Contains("what do i like") || question.Contains("what am i interested in") || question.Contains("my interests"))
            {
                // Report back user's interests
                if (userPreferences[userName].Count > 0)
                {
                    BotAnswers.TypeText($"Based on our conversation, you've shown interest in:", ConsoleColor.Green);
                    foreach (string interest in userPreferences[userName])
                    {
                        BotAnswers.TypeText($"- {interest}", ConsoleColor.Green);
                    }
                }
                else
                {
                    BotAnswers.TypeText($"We haven't discussed your specific interests yet, {userName}. Feel free to ask about password safety, phishing, safe browsing, or privacy!", ConsoleColor.Green);
                }
            }
            // Handle pure sentiment expressions without specific topics
            else if (!string.IsNullOrEmpty(detectedSentiment) &&
                    (question.Contains("feel") || question.Contains("am") || question.StartsWith("i'm") || question.StartsWith("im")))
            {
                BotAnswers.TypeText(GetRandomResponse(empatheticResponses[detectedSentiment]), ConsoleColor.Green);
                BotAnswers.TypeText("Would you like to learn about a specific cybersecurity topic like passwords, phishing, safe browsing, or privacy?", ConsoleColor.Green);
            }
            else
            {
                BotAnswers.TypeText($"Sorry {userName}, I didn't quite understand that.", ConsoleColor.Green);
                BotAnswers.TypeText("Could you please rephrase your question or ask about a specific topic like 'password safety', 'phishing', 'safe browsing' or 'privacy'?", ConsoleColor.Green);
                // Don't update lastTopic here as we didn't understand the question
            }

            Console.ResetColor();
        }

        // Helper method to select a random response
        private string GetRandomResponse(List<string> responses)
        {
            int index = random.Next(responses.Count);
            return responses[index];
        }

        // Detect sentiment in the user's question
        private string DetectSentiment(string input)
        {
            foreach (var sentiment in sentiments)
            {
                foreach (var keyword in sentiment.Value)
                {
                    if (input.Contains(keyword))
                    {
                        return sentiment.Key;
                    }
                }
            }

            return ""; // No sentiment detected
        }
        // Ask a personalized question based on user interests
        private void AskPersonalizedQuestion()
        {
            if (userPreferences[userName].Count == 0)
                return;

            // Select a random interest from the user's preferences
            string interest = userPreferences[userName][random.Next(userPreferences[userName].Count)];

            // Get the question for this interest
            if (personalizedQuestions.ContainsKey(interest))
            {
                string question = personalizedQuestions[interest];
                BotAnswers.TypeText(question, ConsoleColor.Green);

                // Set the waiting flag and pending topic
                waitingForResponse = true;
                pendingTopic = interest;
            }
        }

        // Provide all information on a specific topic
        private void ProvideAllInformationOnTopic(string topic)
        {
            BotAnswers.TypeText($"Here's everything I know about {topic}:", ConsoleColor.Green);

            List<string> allResponses = new List<string>();

            switch (topic)
            {
                case "password":
                    allResponses = passwordResponses;
                    break;
                case "phishing":
                    allResponses = phishingResponses;
                    break;
                case "browsing":
                    allResponses = safeBrowsingResponses;
                    break;
                case "privacy":
                    allResponses = privacyResponses;
                    break;
                default:
                    BotAnswers.TypeText("I don't have additional information on that topic.", ConsoleColor.Green);
                    return;
            }

            foreach (string response in allResponses)
            {
                BotAnswers.TypeText($"• {response}", ConsoleColor.Green);
            }

            BotAnswers.TypeText("Is there anything specific about this topic you'd like to know more about?", ConsoleColor.Green);
        }
    }
}



