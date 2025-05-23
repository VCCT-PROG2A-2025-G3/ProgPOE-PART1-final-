using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    // Responsible for detecting user sentiment
    class SentimentAnalyzer
    {
        private Dictionary<string, List<string>> sentiments = new Dictionary<string, List<string>>
    {
        { "worried", new List<string> { "worried", "concerned", "afraid", "scared", "anxious", "nervous", "fear", "stress", "stressed" } },
        { "curious", new List<string> { "curious", "interested", "want to learn", "tell me", "how do", "how can", "how does", "what is" } },
        { "frustrated", new List<string> { "frustrated", "annoyed", "confused", "difficult", "hard", "complicated", "don't understand", "can't figure", "struggling" } },
        { "overwhelmed", new List<string> { "overwhelmed", "too much", "complex", "complicated", "difficult", "challenging", "lost", "helpless" } }
    };

        // Detect sentiment in the user's question
        public string DetectSentiment(string input)
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
    }
}
