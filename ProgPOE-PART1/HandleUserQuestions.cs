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
        "Review app permissions and disable those that aren’t needed."
    };

        public HandleUserQuestions(string question, string userName)
        {
            this.question = question;
            this.userName = userName;
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
                BotAnswers.TypeText("- Privacy", ConsoleColor.Green);
            }
            else if (question.Contains("password"))
            {
                BotAnswers.TypeText(GetRandomResponse(passwordResponses), ConsoleColor.Green);
            }
            else if (question.Contains("phish") || question.Contains("scam") || question.Contains("fraud"))
            {
                BotAnswers.TypeText(GetRandomResponse(phishingResponses), ConsoleColor.Green);
            }
            else if (question.Contains("safe browsing") || question.Contains("surf") || question.Contains("browser safety"))
            {
                BotAnswers.TypeText(GetRandomResponse(safeBrowsingResponses), ConsoleColor.Green);
            }
            else if (question.Contains("privacy") || question.Contains("private") || question.Contains("security"))
            {
                BotAnswers.TypeText(GetRandomResponse(privacyResponses), ConsoleColor.Green);
            }
            else
            {
                BotAnswers.TypeText($"Sorry {userName}, I didn't quite understand that.", ConsoleColor.Green);
                BotAnswers.TypeText("Could you please rephrase your question or ask about a specific topic like 'password safety', 'phishing', 'safe browsing' or 'privacy'?", ConsoleColor.Green);
            }

            Console.ResetColor();
        }

        // Helper method to select a random response
        private string GetRandomResponse(List<string> responses)
        {
            int index = random.Next(responses.Count);
            return responses[index];
        }
    }
}



