using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; } // For multiple choice
        public int CorrectOptionIndex { get; set; } // Index of correct option in Options array
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; } = false; // true if question is True/False

        public Question(string text, string[] options, int correctIndex, string explanation, bool isTrueFalse = false)
        {
            Text = text;
            Options = options;
            CorrectOptionIndex = correctIndex;
            Explanation = explanation;
            IsTrueFalse = isTrueFalse;
        }
    }
}
