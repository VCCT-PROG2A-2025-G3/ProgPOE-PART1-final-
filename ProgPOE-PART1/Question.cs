using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class Question
    {
        // The question text shown to the user
        public string Text { get; set; }

        // The array of possible answer options (e.g., A, B, C, D)
        public string[] Options { get; set; }

        // The index of the correct answer within the Options array
        public int CorrectOptionIndex { get; set; }

        // Explanation text to show after the user answers (correct or incorrect)
        public string Explanation { get; set; }

        // Flag indicating if the question is a True/False type question
        // True means options will likely be "True" and "False"
        public bool IsTrueFalse { get; set; } = false;

        // Constructor to initialize all properties of a Question instance
        // Parameters:
        //   text - The question prompt
        //   options - The answer options array
        //   correctIndex - The index of the correct option in the options array
        //   explanation - Explanation shown after answer
        //   isTrueFalse - Optional flag to mark as True/False question (default false)
        public Question(string text, string[] options, int correctIndex, string explanation, bool isTrueFalse = false)
        {
            Text = text;
            Options = options;
            CorrectOptionIndex = correctIndex;
            Explanation = explanation;
            IsTrueFalse = isTrueFalse;
        }
    }
