using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestCreator.Annotations;
using TestCreator.Database;

namespace TestCreator.ViewModel
{
    public class SolveTestViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private string _whichQuestion;
        private Question _currentQuestion;
        public int ID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string QuestionsLimit { get; set; }

        public string WhichQuestion
        {
            get => _whichQuestion;
            set { _whichQuestion = value; OnPropertyChanged(nameof(WhichQuestion)); }
        }

        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set { _currentQuestion = value; OnPropertyChanged(nameof(CurrentQuestion)); }
        }

        public ObservableCollection<Question> QuestionsWithAnswersFromUser { get; set; }
        public List<Question> ChoosenQuestionsWithAnswers { get; set; }

        public void CreateModel(Test test)
        {
            QuestionsWithAnswersFromUser = new ObservableCollection<Question>();
            ChoosenQuestionsWithAnswers = new List<Question>();

            this.Name = test.Name;
            this.ID = test.TestID;
            this.QuestionsLimit = test.QuestionsLimit.ToString();
            this.WhichQuestion = "0";

            foreach (var question in test.Questions)
            {
                //for anwsers
                var newQuestion = new Question
                {
                    Number = QuestionsWithAnswersFromUser.Count + 1,
                    Title = question.Text,
                    ID = question.QuestionID,
                    Answers = new ObservableCollection<Answer>()
                };

                foreach (var answer in question.Answers)
                {
                    newQuestion.Answers.Add(new Answer()
                    {
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect,
                        ID = answer.AnswerID
                    });
                }
                ChoosenQuestionsWithAnswers.Add(newQuestion);
                // for users
                var newQuestionForUser = new Question
                {
                    Number = QuestionsWithAnswersFromUser.Count + 1,
                    Title = question.Text,
                    ID = question.QuestionID,
                    Answers = new ObservableCollection<Answer>()
                };

                foreach (var answer in question.Answers)
                {
                    newQuestionForUser.Answers.Add(new Answer()
                    {
                        Text = answer.Text,
                        IsCorrect = false,
                        ID = answer.AnswerID
                    });
                }
                QuestionsWithAnswersFromUser.Add(newQuestionForUser);

            }

            var rand = new Random();
            var results = Enumerable.Range(1, test.Questions.Count).OrderBy(x => rand.Next()).Take(test.QuestionsLimit).ToList();
            // select randomly
            QuestionsWithAnswersFromUser = new ObservableCollection<Question>(QuestionsWithAnswersFromUser.Where(x => results.Contains(x.Number)));
            ChoosenQuestionsWithAnswers = ChoosenQuestionsWithAnswers.Where(x => results.Contains(x.Number)).ToList();

            CurrentQuestion = QuestionsWithAnswersFromUser[0];
            WhichQuestion = CurrentQuestion.Number.ToString() + " z " + QuestionsLimit;
        }



        public List<Database.Question> GetQuestions()
        {
            var questions = new List<Database.Question>();

            foreach (var question in QuestionsWithAnswersFromUser)
            {

                var tmp = new Database.Question {Text = question.Title};
                if (question.ID != 0)
                {
                    tmp.QuestionID = question.ID;
                }

                var answers = new List<Database.Answer>();

                foreach (var answer in question.Answers)
                {
                    var newAnswer = new Database.Answer
                    {
                        IsCorrect = answer.IsCorrect,
                        Text = answer.Text
                    };
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

        public void GoToTheNextQuestion()
        {
            var nextIndex = QuestionsWithAnswersFromUser.IndexOf(CurrentQuestion) + 1;
            if (nextIndex >= QuestionsWithAnswersFromUser.Count) return;

            CurrentQuestion = QuestionsWithAnswersFromUser[nextIndex];

            WhichQuestion = (nextIndex + 1) + " z " + QuestionsLimit;
        }

        public void GoBackToThePreviousQuestion()
        {
            var prevIndex = QuestionsWithAnswersFromUser.IndexOf(CurrentQuestion) - 1;
            if (prevIndex < 0) return;

            CurrentQuestion = QuestionsWithAnswersFromUser[prevIndex];

            WhichQuestion = (prevIndex + 1) + " z " + QuestionsLimit;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CalculateScore()
        {
            var score = 0;
            var maxScore = 0;
            for (var i = 0; i < ChoosenQuestionsWithAnswers.Count; i++)
            {
                var question = ChoosenQuestionsWithAnswers[i];
                var usersAnswers = QuestionsWithAnswersFromUser[i];

                var pointsFromQuestion = 0;

                for (var index = 0; index < question.Answers.Count; index++)
                {
                    var answer = question.Answers[index];
                    var usersAnwser = usersAnswers.Answers[index];

                    if (answer.IsCorrect == usersAnwser.IsCorrect)
                    {
                        if(answer.IsCorrect)
                            pointsFromQuestion++;
                    }
                    else
                    {
                        pointsFromQuestion = 0;
                        break;
                    }
                }
                maxScore += question.Answers.Where(x => x.IsCorrect).ToList().Count;
                score += pointsFromQuestion;
            }
            MessageBox.Show("Your Score: " + score + " / " + maxScore, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return score;
        }
    }
}
