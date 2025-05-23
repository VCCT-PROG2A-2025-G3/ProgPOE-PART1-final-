using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    // Stores all the predefined responses
    class ResponseBank
    {
        private static Random random = new Random();

        // Empathetic responses based on sentiment
        public Dictionary<string, List<string>> EmpatheticResponses { get; } = new Dictionary<string, List<string>>
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

        // Topic-specific responses
        public Dictionary<string, List<string>> TopicResponses { get; } = new Dictionary<string, List<string>>
    {
        { "password", new List<string>
            {
                "Always use strong, unique passwords. Include letters, numbers, and symbols.",
                "Avoid reusing the same password on multiple sites.",
                "Consider using a password manager to generate and store secure passwords.",
                "Change your passwords regularly and never share them."
            }
        },
        { "phishing", new List<string>
            {
                "Phishing is a scam where attackers trick you into giving up personal info. Never click suspicious links or attachments in emails.",
                "Always double-check the sender's email address. If something feels off, don't click!",
                "Beware of emails urging immediate action. They're often phishing attempts!",
                "When in doubt, contact the sender through a known, official method before responding to emails with links or attachments."
            }
        },
        { "browsing", new List<string>
            {
                "Always check for HTTPS in the browser URL to ensure the site is secure.",
                "Avoid clicking on pop-ups or ads, especially on unfamiliar websites.",
                "Keep your browser and antivirus software up to date for maximum protection.",
                "Don't download files from untrusted or suspicious websites.",
                "Use a secure and privacy-focused browser, and consider enabling private browsing mode."
            }
        },
        { "privacy", new List<string>
            {
                "Use strong, unique passwords and enable multifactor authentication.",
                "Limit personal information shared on social media.",
                "Be cautious when clicking links or downloading attachments.",
                "Review app permissions and disable those that aren't needed."
            }
        }
    };

        // Personalized questions based on interests
        public Dictionary<string, string> PersonalizedQuestions { get; } = new Dictionary<string, string>
    {
        { "password", "Since you're interested in password security, would you like to learn all my tips about creating and managing secure passwords? (yes/no)" },
        { "phishing", "As someone concerned about phishing, would you like to know all my tips for identifying and avoiding phishing scams? (yes/no)" },
        { "browsing", "Since you're interested in safe browsing, would you like to learn all my tips for staying safe while surfing the web? (yes/no)" },
        { "privacy", "As someone concerned about privacy, would you like to know all my recommendations for protecting your personal information online? (yes/no)" }
    };

        // Helper method to select a random response
        public string GetRandomResponse(List<string> responses)
        {
            int index = random.Next(responses.Count);
            return responses[index];
        }
    }
}
