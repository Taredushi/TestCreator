using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.ViewModel
{
    public struct Question
    {
        public int Numer { get; set; }
        public string Title { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }
    }

    public struct Answer
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
