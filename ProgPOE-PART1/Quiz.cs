using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class Quiz
    {

    private void LoadQuestions()
    {
            questions = new List<Question>
        {
            new Question("What should you do if you receive an email asking for your password?",
                         new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                         2,
                         "Correct! Reporting phishing emails helps prevent scams."),
            new Question("True or False: Using the same password for multiple accounts is safe.",
                         new[] { "True", "False" },
                         1,
                         "False! Using the same password makes all your accounts vulnerable.",
                         true),
            // Add the rest of your questions here
        };
    }

    private void ShowQuestion()
    {
        FeedbackText.Text = "";
        NextButton.IsEnabled = false;
        selectedOptionIndex = null;

        var q = questions[currentQuestionIndex];
        QuestionText.Text = q.Text;

        OptionsPanel.Children.Clear();

        for (int i = 0; i < q.Options.Length; i++)
        {
            RadioButton rb = new RadioButton
            {
                Content = q.Options[i],
                Tag = i,
                GroupName = "OptionsGroup",
                Margin = new Thickness(5)
            };
            rb.Checked += Option_Checked;
            OptionsPanel.Children.Add(rb);
        }
    }

    private void Option_Checked(object sender, RoutedEventArgs e)
    {
        var rb = sender as RadioButton;
        selectedOptionIndex = (int)rb.Tag;
        NextButton.IsEnabled = true;
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        if (selectedOptionIndex == null) return;

        var q = questions[currentQuestionIndex];

        if (selectedOptionIndex == q.CorrectOptionIndex)
        {
            score++;
            FeedbackText.Foreground = Brushes.Green;
            FeedbackText.Text = $"Correct! {q.Explanation}";
        }
        else
        {
            FeedbackText.Foreground = Brushes.Red;
            FeedbackText.Text = $"Incorrect! {q.Explanation}";
        }

        NextButton.IsEnabled = false;

        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Count)
        {
            // Quiz finished
            string finalMessage = score >= questions.Count * 0.8
                ? "Great job! You’re a cybersecurity pro!"
                : "Keep learning to stay safe online!";

            MessageBox.Show($"Quiz complete!\nYour score: {score} out of {questions.Count}\n{finalMessage}", "Quiz Result");

            // Optionally reset quiz
            currentQuestionIndex = 0;
            score = 0;
            ShowQuestion();
        }
        else
        {
            // Wait 2 seconds then show next question
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                ShowQuestion();
            };
            timer.Start();
        }
    }
}
    }
}
