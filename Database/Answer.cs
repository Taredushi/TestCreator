using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Database
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionID { get; set; }
        public Question Question { get; set; }

    }
}
