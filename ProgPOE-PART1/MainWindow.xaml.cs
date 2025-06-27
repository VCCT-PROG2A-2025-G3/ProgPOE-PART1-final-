using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProgPOE_PART1
{
    public partial class MainWindow : Window
    {
        private string userName;
        private HandleUserQuestions questionHandler;
        private List<CyberTask> userTasks;
        private DispatcherTimer reminderTimer;

        public MainWindow()
        {
            InitializeComponent();
            userTasks = new List<CyberTask>();
            InitializeReminderTimer();

            // Set placeholder text behaviors
            SetPlaceholderBehavior();
        }

        private void InitializeReminderTimer()
        {
            reminderTimer = new DispatcherTimer();
            reminderTimer.Interval = TimeSpan.FromMinutes(1); // Check every minute
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }

        private void SetPlaceholderBehavior()
        {
            TaskTitleTextBox.GotFocus += (s, e) =>
            {
                if (TaskTitleTextBox.Text == "Task Title")
                    TaskTitleTextBox.Text = "";
            };
            TaskTitleTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TaskTitleTextBox.Text))
                    TaskTitleTextBox.Text = "Task Title";
            };

            TaskDescriptionTextBox.GotFocus += (s, e) =>
            {
                if (TaskDescriptionTextBox.Text == "Task Description")
                    TaskDescriptionTextBox.Text = "";
            };
            TaskDescriptionTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TaskDescriptionTextBox.Text))
                    TaskDescriptionTextBox.Text = "Task Description";
            };
        }

        private void WelcomeStartButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WelcomeNameTextBox.Text))
            {
                MessageBox.Show("Please enter your name to continue.", "Name Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            userName = WelcomeNameTextBox.Text.Trim();
            UserNameLabel.Text = $"Hello, {userName}!";
            questionHandler = new HandleUserQuestions("", userName);

            WelcomeOverlay.Visibility = Visibility.Collapsed;
            UserInputTextBox.Focus();
        }

        private void WelcomeNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                WelcomeStartButton_Click(sender, e);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void UserInputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserInputTextBox.Text == "Type your message here...")
                UserInputTextBox.Text = "";
        }

        private void UserInputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserInputTextBox.Text))
                UserInputTextBox.Text = "Type your message here...";
        }

        private async void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInputTextBox.Text) ||
                UserInputTextBox.Text == "Type your message here...")
                return;

            string userMessage = UserInputTextBox.Text.Trim();
            UserInputTextBox.Text = "";

            // Add user message to chat
            AddUserMessage(userMessage);

            // Process the message and get bot response
            await ProcessUserMessage(userMessage);

            // Scroll to bottom
            ChatScrollViewer.ScrollToBottom();
        }

        private void AddUserMessage(string message)
        {
            var userBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#89b4fa")),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(50, 5, 0, 10),
                Padding = new Thickness(15),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var userText = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.SemiBold
            };

            userBorder.Child = userText;
            ChatPanel.Children.Add(userBorder);
        }

        private async Task ProcessUserMessage(string message)
        {
            // Check if it's a task-related command first
            if (message.ToLower().StartsWith("add task"))
            {
                HandleAddTaskCommand(message);
                return;
            }

            if (message.ToLower() == "show tasks" || message.ToLower() == "list tasks")
            {
                ShowTasksSummary();
                return;
            }

            // Use existing chatbot logic
            questionHandler = new HandleUserQuestions(message, userName);
            var response = await GetBotResponse(message);

            // Add bot response with typing effect
            await AddBotMessageWithTyping(response);
        }

        private async Task<string> GetBotResponse(string question)
        {
            // Simulate the existing ProcessQuestion logic
            if (string.IsNullOrWhiteSpace(question))
            {
                return "It seems you didn't ask a question. Please ask a question so I can assist you.";
            }

            question = question.ToLower().Trim();

            // Check for greetings
            if (question.Contains("how are you"))
            {
                return "I'm just a bot, but I'm fully operational and ready to help you stay safe online!";
            }

            // Check for help requests
            if (question.Contains("what can i ask") || question.Contains("help") ||
                question.Contains("topics") || question.Contains("what can you do"))
            {
                return "You can ask me about topics like:\n• Password safety\n• Phishing scams\n• Safe browsing habits\n• Privacy protection\n• Two-factor authentication\n\nYou can also use the task assistant to manage your cybersecurity goals!";
            }

            // Check for password-related questions
            if (question.Contains("password"))
            {
                var responses = new List<string>
                {
                    "Always use strong, unique passwords. Include letters, numbers, and symbols.",
                    "Avoid reusing the same password on multiple sites.",
                    "Consider using a password manager to generate and store secure passwords.",
                    "Change your passwords regularly and never share them."
                };
                var random = new Random();
                return responses[random.Next(responses.Count)] +
                       "\n\nWould you like me to add a password security task to your task list?";
            }

            // Check for phishing-related questions
            if (question.Contains("phish") || question.Contains("scam") || question.Contains("fraud"))
            {
                var responses = new List<string>
                {
                    "Phishing is a scam where attackers trick you into giving up personal info. Never click suspicious links or attachments in emails.",
                    "Always double-check the sender's email address. If something feels off, don't click!",
                    "Beware of emails urging immediate action. They're often phishing attempts!",
                    "When in doubt, contact the sender through a known, official method before responding to emails with links or attachments."
                };
                var random = new Random();
                return responses[random.Next(responses.Count)] +
                       "\n\nI can help you add an anti-phishing task to stay vigilant!";
            }

            // Check for browsing safety
            if (question.Contains("safe browsing") || question.Contains("surf") || question.Contains("browser safety"))
            {
                var responses = new List<string>
                {
                    "Always check for HTTPS in the browser URL to ensure the site is secure.",
                    "Avoid clicking on pop-ups or ads, especially on unfamiliar websites.",
                    "Keep your browser and antivirus software up to date for maximum protection.",
                    "Don't download files from untrusted or suspicious websites."
                };
                var random = new Random();
                return responses[random.Next(responses.Count)];
            }

            // Check for privacy questions
            if (question.Contains("privacy") || question.Contains("private") || question.Contains("security"))
            {
                return "Privacy protection involves using strong passwords, enabling two-factor authentication, limiting personal information on social media, and being cautious with links and downloads.\n\nWould you like me to help you create a privacy checklist?";
            }

            // Check for 2FA questions
            if (question.Contains("two factor") || question.Contains("2fa") || question.Contains("authentication"))
            {
                return "Two-factor authentication (2FA) adds an extra security layer by requiring something you know (password) and something you have (like your phone). Enable it on all important accounts - it makes your accounts 99% less likely to be compromised!\n\nI can add a 2FA setup task to help you get started.";
            }

            // Default response
            return $"Sorry {userName}, I didn't quite understand that. Could you please rephrase your question or ask about a specific topic like 'password safety', 'phishing', 'safe browsing', 'privacy', or 'two-factor authentication'?";
        }

        private async Task AddBotMessageWithTyping(string message)
        {
            var botBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#313244")),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(0, 5, 50, 10),
                Padding = new Thickness(15),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var botText = new TextBlock
            {
                Text = "",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cdd6f4")),
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            botBorder.Child = botText;
            ChatPanel.Children.Add(botBorder);

            // Typing effect
            for (int i = 0; i <= message.Length; i++)
            {
                botText.Text = message.Substring(0, i);
                ChatScrollViewer.ScrollToBottom();
                await Task.Delay(30); // Typing speed
            }
        }

        private void HandleAddTaskCommand(string message)
        {
            // Parse the add task command
            string taskInfo = message.Substring("add task".Length).Trim();
            if (string.IsNullOrEmpty(taskInfo))
            {
                AddBotMessageWithTyping("Please specify a task. For example: 'Add task - Review privacy settings'");
                return;
            }

            // Remove leading dash if present
            if (taskInfo.StartsWith("-"))
                taskInfo = taskInfo.Substring(1).Trim();

            AddQuickTask(taskInfo, $"Complete the task: {taskInfo}");
        }

        private void ShowTasksSummary()
        {
            if (userTasks.Count == 0)
            {
                AddBotMessageWithTyping("You don't have any tasks yet. Use the task assistant on the left to add some cybersecurity tasks!");
                return;
            }

            string summary = $"Here are your {userTasks.Count} cybersecurity tasks:\n\n";
            for (int i = 0; i < userTasks.Count; i++)
            {
                var task = userTasks[i];
                string status = task.IsCompleted ? "✅ Completed" : "⏳ Pending";
                summary += $"{i + 1}. {task.Title} - {status}\n";
                if (task.HasReminder)
                    summary += $"   📅 Reminder: {task.ReminderDate:MMM dd, yyyy}\n";
            }

            AddBotMessageWithTyping(summary);
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskTitleTextBox.Text == "Task Title" || string.IsNullOrWhiteSpace(TaskTitleTextBox.Text))
            {
                MessageBox.Show("Please enter a task title.", "Title Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string title = TaskTitleTextBox.Text;
            string description = TaskDescriptionTextBox.Text == "Task Description" ?
                "" : TaskDescriptionTextBox.Text;

            bool hasReminder = ReminderCheckBox.IsChecked == true;
            DateTime? reminderDate = null;

            if (hasReminder && ReminderComboBox.SelectedItem != null)
            {
                string selectedPeriod = ((ComboBoxItem)ReminderComboBox.SelectedItem).Content.ToString();
                reminderDate = CalculateReminderDate(selectedPeriod);
            }

            var task = new CyberTask
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                HasReminder = hasReminder,
                ReminderDate = reminderDate,
                CreatedDate = DateTime.Now,
                IsCompleted = false
            };

            userTasks.Add(task);
            AddTaskToUI(task);
            ClearTaskInputs();

            AddBotMessageWithTyping($"Great! I've added the task '{title}' to your list." +
                (hasReminder ? $" I'll remind you on {reminderDate:MMM dd, yyyy}." : ""));
        }

        private void AddQuickTask(string title, string description)
        {
            var task = new CyberTask
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                HasReminder = false,
                CreatedDate = DateTime.Now,
                IsCompleted = false
            };

            userTasks.Add(task);
            AddTaskToUI(task);

            AddBotMessageWithTyping($"I've added '{title}' to your task list. You can manage it using the task assistant on the left.");
        }

        private DateTime CalculateReminderDate(string period)
        {
            DateTime now = DateTime.Now;
            switch (period.ToLower())
            {
                case "1 day": return now.AddDays(1);
                case "3 days": return now.AddDays(3);
                case "1 week": return now.AddDays(7);
                case "2 weeks": return now.AddDays(14);
                case "1 month": return now.AddMonths(1);
                default: return now.AddDays(7);
            }
        }

        private void AddTaskToUI(CyberTask task)
        {
            var taskBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#313244")),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 5, 0, 5),
                Padding = new Thickness(12),
                Tag = task
            };

            var taskPanel = new StackPanel();

            // Title
            var titleText = new TextBlock
            {
                Text = task.Title,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cdd6f4")),
                FontWeight = FontWeights.SemiBold,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            // Description
            if (!string.IsNullOrEmpty(task.Description))
            {
                var descText = new TextBlock
                {
                    Text = task.Description,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a6adc8")),
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 3, 0, 0)
                };
                taskPanel.Children.Add(descText);
            }

            // Reminder info
            if (task.HasReminder)
            {
                var reminderText = new TextBlock
                {
                    Text = $"📅 Reminder: {task.ReminderDate:MMM dd, yyyy}",
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f9e2af")),
                    FontSize = 11,
                    Margin = new Thickness(0, 3, 0, 0)
                };
                taskPanel.Children.Add(reminderText);
            }

            // Action buttons
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 8, 0, 0)
            };

            var completeButton = new Button
            {
                Content = task.IsCompleted ? "✅ Completed" : "✅ Complete",
                Background = task.IsCompleted ?
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a6e3a1")) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#89b4fa")),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(8, 4),
                Margin = new Thickness(0, 0, 5, 0),
                FontSize = 11,
                Cursor = Cursors.Hand,
                IsEnabled = !task.IsCompleted
            };
            completeButton.Click += (s, e) => CompleteTask(task, taskBorder);

            var deleteButton = new Button
            {
                Content = "🗑️ Delete",
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f38ba8")),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(8, 4),
                FontSize = 11,
                Cursor = Cursors.Hand
            };
            deleteButton.Click += (s, e) => DeleteTask(task, taskBorder);

            buttonPanel.Children.Add(completeButton);
            buttonPanel.Children.Add(deleteButton);

            taskPanel.Children.Insert(0, titleText
