using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCreator.Database;

namespace TestCreator.ViewModel
{
    public class TestViewModel : IDataErrorInfo
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string QuestionsLimit { get; set; }

        public int QuestionsNumber { get; set; }

        public ObservableCollection<Question> Questions { get; set; }

        public TestViewModel(bool empty)
        {
            Questions = new ObservableCollection<Question>();
            if (empty)
            {
                AddEmptyQuestion();
            }
        }

        public void AddEmptyQuestion()
        {
            Questions.Add(new Question()
            {
                Answers = new ObservableCollection<Answer>()
                {
                    new Answer() {IsCorrect = false, Text = "Odpowiedź"},
                    new Answer() {IsCorrect = false, Text = "Odpowiedź"},
                    new Answer() {IsCorrect = false, Text = "Odpowiedź"},
                    new Answer() {IsCorrect = false, Text = "Odpowiedź"},
                },
                Number = Questions.Count + 1,
                Title = "Treść pytania"
            });
        }

        public Test GetTest()
        {
            Test test = new Test();
            test.Name = this.Name;
            test.QuestionsLimit = int.Parse(QuestionsLimit);
            test.Questions = GetQuestions();

            if (ID != 0)
            {
                test.TestID = ID;
            }
            
            return test;
        }

        public List<Database.Question> GetQuestions()
        {
            List<Database.Question> questions = new List<Database.Question>();

            foreach (var question in Questions)
            {

                Database.Question tmp = new Database.Question();
                tmp.Text = question.Title;
                if (question.ID != 0)
                {
                    tmp.QuestionID = question.ID;
                }

                List<Database.Answer> answers = new List<Database.Answer>();

                foreach (var answer in question.Answers)
                {
                    Database.Answer newAnswer = new Database.Answer();
                    newAnswer.IsCorrect = answer.IsCorrect;
                    newAnswer.Text = answer.Text;
                    if (answer.ID != 0)
                    {
                        newAnswer.AnswerID = answer.ID;
                    }

                    answers.Add(newAnswer);
                }

                tmp.Answers = answers;
                questions.Add(tmp);
            }
            return questions;
        }

        public void CreateSimpleModel(Test test)
        {
            this.Name = test.Name;
            this.ID = test.TestID;
            this.QuestionsLimit = test.QuestionsLimit.ToString();
            this.QuestionsNumber = test.Questions.Count;
        }

        public void CreateModel(Test test)
        {
            this.Name = test.Name;
            this.ID = test.TestID;
            this.QuestionsLimit = test.QuestionsLimit.ToString();

            foreach (var question in test.Questions)
            {
                var newQuestion =  new Question()
                {
                    Number = Questions.Count + 1,
                    Title = question.Text,
                    ID = question.QuestionID
                };

                newQuestion.Answers = new ObservableCollection<Answer>();

                foreach (var answer in question.Answers)
                {
                    newQuestion.Answers.Add(new Answer()
                    {
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        ID = answer.AnswerID
                    });
                }

                Questions.Add(newQuestion);
            }
        }

        #region IDataErrorInfo implementation

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "QuestionsLimit":
                        int number;
                        if (int.TryParse(QuestionsLimit, out number))
                        {
                            if (number > 100)
                            {
                                return "Limit nie może być większy od 100";
                            }
                        }
                        else
                        {
                            return "Limit musi być liczbą.";
                        }
                        break;                   
                }
                return null;
            }
        }

        #endregion
    }
}
