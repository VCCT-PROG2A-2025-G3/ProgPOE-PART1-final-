using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    // Manages user preferences and conversation state
    class UserPreferenceManager
    {
        private static Dictionary<string, List<string>> userPreferences = new Dictionary<string, List<string>>();
        private static int messageCount = 0;
        private static bool waitingForResponse = false;
        private static string pendingTopic = "";
        private static string lastTopic = "";
        private static bool inFollowUpMode = false;

        // Properties to access state
        public int MessageCount => messageCount;
        public bool WaitingForResponse => waitingForResponse;
        public string PendingTopic => pendingTopic;
        public string LastTopic => lastTopic;
        public bool InFollowUpMode => inFollowUpMode;

        // Increment message count
        public void IncrementMessageCount()
        {
            messageCount++;
        }

        // Initialize user preferences if needed
        public void InitializeUserIfNeeded(string userName)
        {
            if (!userPreferences.ContainsKey(userName))
            {
                userPreferences[userName] = new List<string>();
            }
        }

        // Get user's preferences
        public List<string> GetUserPreferences(string userName)
        {
            if (userPreferences.ContainsKey(userName))
            {
                return userPreferences[userName];
            }
            return new List<string>();
        }

        // Add a topic to user's preferences
        public bool AddUserPreference(string userName, string topic)
        {
            if (!userPreferences[userName].Contains(topic))
            {
                userPreferences[userName].Add(topic);
                return true; // Indicates this is a new preference
            }
            return false; // User already had this preference
        }

        // Set waiting for response state
        public void SetWaitingForResponse(bool waiting, string topic = "")
        {
            waitingForResponse = waiting;
            pendingTopic = topic;
        }

        // Set last topic
        public void SetLastTopic(string topic)
        {
            lastTopic = topic;
        }

        // Set follow-up mode
        public void SetFollowUpMode(bool mode)
        {
            inFollowUpMode = mode;
        }
    }
}
