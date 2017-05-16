using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.ViewModel
{
    public class TestViewModel
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public ObservableCollection<Question> Questions { get; set; }

        public TestViewModel()
        {
            Name = "Nazwa testu";
            Title = "Dodaj Test";
            Questions = new ObservableCollection<Question>()
            {
                new Question()
                {
                    Answers = new ObservableCollection<Answer>()
                    {
                        new Answer(){IsCorrect = false, Text = "Odpowiedź"},
                        new Answer(){IsCorrect = false, Text = "Odpowiedź"},
                        new Answer(){IsCorrect = false, Text = "Odpowiedź"},
                        new Answer(){IsCorrect = false, Text = "Odpowiedź"},
                    },
                    Numer = 1,
                    Title = "Treść pytania"
                }
            };
        }

    }
}
