using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProgPOE_PART1
{
    public partial class MainWindow : Window
    {
        private List<Question> questions;  // Declare here

        public MainWindow()
        {
            InitializeComponent();
            LoadQuestions();  // Initialize the questions here
            ShowQuestion();
        }

        private void LoadQuestions()
        {
            questions = new List<Question>
        {
            new Question("Question 1 text", new string[] {"A", "B", "C", "D"}, 2, "Explanation here"),
            // Add more questions
        };
        }
    }
}
        