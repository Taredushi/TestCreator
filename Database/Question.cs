using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Database
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }

        public int TestID { get; set; }
        public Test Test { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        [NotMapped]
        public int MaxPoints { get; }
    }
}
