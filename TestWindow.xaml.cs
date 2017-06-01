using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestCreator.Database;
using TestCreator.ViewModel;

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {

        private TestViewModel _testViewModel;

        public TestViewModel TestViewModel
        {
            get { return _testViewModel; }
            set
            {
                _testViewModel = value;
                OnPropertyChanged();
            }
        }

        public TestWindow(TestViewModel _testViewModel)
        {
            InitializeComponent();
            this._testViewModel = _testViewModel;
            DataContext = TestViewModel;
        }

        public TestWindow(bool empty)
        {
            InitializeComponent();
            TestViewModel = new TestViewModel(empty){Title = "Dodaj Test"};
            DataContext = TestViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void AnswersDataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        private static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }

        private void AddQuestionButton_OnClick(object sender, RoutedEventArgs e)
        {
            TestViewModel.AddEmptyQuestion();
        }

        private void DeleteQuestionButton_OnClick(object sender, RoutedEventArgs e)
        {
            int index = QuestionsListBox.SelectedIndex;
            if (index == -1) return;

            TestViewModel.Questions.RemoveAt(index);

            for (int i = index; i < TestViewModel.Questions.Count; i++)
            {
                TestViewModel.Questions[i].Number = TestViewModel.Questions[i].Number - 1;
            }
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (_testViewModel.ID != 0)
            {
                DatabaseHelpers.RemoveTestByID(_testViewModel.ID);
            }
            DatabaseHelpers.SaveTestToDb(_testViewModel);
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void LimitTb_OnTextChanged(object sender, TextChangedEventArgs e)
        {
           if (LimitTb.Text.Length > 3)
           {
               LimitTb.Text = LimitTb.Text.Remove(3);
               LimitTb.CaretIndex = LimitTb.Text.Length;
           }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var datagrid = sender as DataGrid;

            if (datagrid == null) return;
            if (datagrid.CurrentCell.Column.Header.Equals("Poprawna"))
            {
                var item = (e.AddedItems[0] as TestCreator.ViewModel.Answer);
                if (item != null)
                    item.IsCorrect = !item.IsCorrect;
            }
            datagrid?.UnselectAllCells();

        }
    }
}
