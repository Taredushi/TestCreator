using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCreator.Database;

namespace TestCreator.ViewModel
{
    public class TestViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

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
            Test test = new Test()
            {
                Name = this.Name
            };

            if (ID != 0)
            {
                test.TestID = ID;
            }

            List<Database.Question> questions = new List<Database.Question>();

            foreach (var question in Questions)
            {
                Database.Question tmp = new Database.Question()
                {
                    Text = question.Title
                };

                List<Database.Answer> answers = new List<Database.Answer>();

                foreach (var answer in question.Answers)
                {
                    Database.Answer newAnswer = new Database.Answer()
                    {
                        IsCorrect = answer.IsCorrect,
                        Text = answer.Text
                    };
                    answers.Add(newAnswer);
                }

                tmp.Answers = answers;
                questions.Add(tmp);
            }
            test.Questions = questions;

            return test;
        }

        public void CreateModel(Test test)
        {
            this.Name = test.Name;
            this.ID = test.TestID;
        }
    }
}
