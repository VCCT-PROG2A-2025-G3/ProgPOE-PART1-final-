using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    // Handles specific follow-up questions about topics
    class TopicFollowUpHandler
    {
        // Changed from private to public to allow access in HandleUserQuestions
        public Dictionary<string, Dictionary<string, string>> topicFollowUps = new Dictionary<string, Dictionary<string, string>>
    {
        { "password", new Dictionary<string, string>
            {
                { "how often should i change", "For important accounts, change passwords every 3-6 months. For less critical accounts, once a year is sufficient. However, it's more important to use unique, strong passwords than to change them frequently." },
                { "what makes a strong password", "A strong password should be at least 12 characters long, include uppercase and lowercase letters, numbers, and special characters. Avoid using personal information, dictionary words, or common patterns." },
                { "password manager", "Password managers like LastPass, Dashlane, or Bitwarden securely store all your passwords in an encrypted vault. They can generate strong, unique passwords for each site and auto-fill them, so you only need to remember one master password." },
                { "two factor", "Two-factor authentication (2FA) adds an extra layer of security by requiring something you know (password) and something you have (like your phone). Enable it on all important accounts - it makes your accounts 99% less likely to be compromised." },
                { "remember", "Don't write passwords on sticky notes or store them in plain text files. Instead, use a password manager. If you must memorize them, try creating a passphrase - a sentence that's meaningful to you but hard for others to guess." }
            }
        },
        { "phishing", new Dictionary<string, string>
            {
                { "how to identify", "Look for these red flags: unexpected emails requesting urgent action, generic greetings, spelling/grammar errors, suspicious email addresses, unusual requests for personal information, and suspicious links or attachments." },
                { "what to do if clicked", "If you clicked a phishing link: disconnect from the internet, scan your device with antivirus software, change passwords for any accounts you accessed, monitor your accounts for suspicious activity, and report the incident to your IT department if at work." },
                { "report", "Report phishing emails to your email provider (most have a 'report phishing' option). You can also forward them to the Anti-Phishing Working Group at reportphishing@apwg.org or report to your country's cybersecurity authority." },
                { "spear phishing", "Spear phishing targets specific individuals with personalized messages using information gathered about you. Be extra cautious with emails that seem to know details about you, especially if they're asking for sensitive information or contain links/attachments." },
                { "smishing", "Smishing is phishing via SMS text messages. Never click links in unexpected text messages, even if they claim to be from your bank or a service you use. Instead, contact the organization directly through their official website or phone number." }
            }
        },
        { "browsing", new Dictionary<string, string>
            {
                { "https", "HTTPS (indicated by a padlock icon in your browser) means the connection between your browser and the website is encrypted. Always check for HTTPS before entering sensitive information on a website." },
                { "vpn", "A VPN (Virtual Private Network) encrypts your internet connection, protecting your data on public Wi-Fi and hiding your browsing activity. Consider using one when on public networks or if you want additional privacy." },
                { "incognito", "Incognito or private browsing mode prevents your browser from saving your browsing history, cookies, and site data. However, it doesn't make you anonymous online - your ISP, employer, or the websites you visit can still track you." },
                { "cookies", "Cookies are small files websites store on your device to remember your preferences. While most are harmless, some track your browsing habits. Regularly clear cookies or use browser settings to block third-party cookies for better privacy." },
                { "extensions", "Browser extensions can enhance security (ad blockers, password managers), but they can also pose risks. Only install extensions from trusted sources, regularly review and remove unused ones, and check their permissions." }
            }
        },
        { "privacy", new Dictionary<string, string>
            {
                { "social media", "On social media: review privacy settings regularly, be selective about friend requests, limit personal information in your profile, disable location sharing, and think twice before posting sensitive information or photos." },
                { "data breach", "If your data was in a breach: change affected passwords immediately, enable two-factor authentication, monitor your accounts for suspicious activity, check your credit report, and consider freezing your credit if sensitive information was exposed." },
                { "children", "To protect children's privacy online: use parental controls, teach them about sharing personal information, monitor their online activities, use privacy-focused services, and have open conversations about online safety." },
                { "tracking", "Minimize tracking by using privacy-focused browsers (like Firefox or Brave), installing ad blockers, adjusting privacy settings in your browser, using search engines that don't track you (like DuckDuckGo), and regularly clearing your browsing data." },
                { "delete data", "To remove your data online: use privacy tools like DeleteMe or PrivacyDuck, contact companies directly to request data deletion, use your legal rights (like GDPR in Europe or CCPA in California), and regularly Google yourself to find what information is public." }
            }
        }
    };

        // Check if we have a specific answer for this follow-up question
        public bool HasSpecificAnswer(string topic, string question)
        {
            if (!topicFollowUps.ContainsKey(topic))
                return false;
            question = question.ToLower();
            foreach (var key in topicFollowUps[topic].Keys)
            {
                if (question.Contains(key))
                    return true;
            }
            return false;
        }

        // Get the specific answer for a follow-up question
        public string GetSpecificAnswer(string topic, string question)
        {
            if (!topicFollowUps.ContainsKey(topic))
                return null;
            question = question.ToLower();
            foreach (var key in topicFollowUps[topic].Keys)
            {
                if (question.Contains(key))
                    return topicFollowUps[topic][key];
            }
            return null;
        }
    }
}
