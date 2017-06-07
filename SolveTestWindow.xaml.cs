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
    public partial class SolveTestWindow : Window
    {
        private int _userId;
        public SolveTestWindow(SolveTestViewModel solveTestViewModel, int userId)
        {
            InitializeComponent();
            _userId = userId;
            _solveTestViewModel = solveTestViewModel;
            DataContext = SolveTestViewModel;
        }

        #region ViewModelAndOnPropertyChanged

        private SolveTestViewModel _solveTestViewModel;

        public SolveTestViewModel SolveTestViewModel
        {
            get => _solveTestViewModel;
            set
            {
                _solveTestViewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

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
                if (pi == null) return null;

                // Check for DisplayName attribute and set the column header accordingly
                var attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                foreach (var t in attributes)
                {
                    var displayName = t as DisplayNameAttribute;
                    if (displayName != null && displayName != DisplayNameAttribute.Default)
                    {
                        return displayName.DisplayName;
                    }
                }
            }

            return null;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            int temp;

            if (string.IsNullOrEmpty(_solveTestViewModel.QuestionsLimit) ||
                !int.TryParse(_solveTestViewModel.QuestionsLimit, out temp)) return;

            this.DialogResult = true;
            if (_solveTestViewModel.ID != 0)
            {
                var userTest = new UserTest
                {
                    TestID = _solveTestViewModel.ID,
                    UserID = _userId,
                    Result = _solveTestViewModel.CalculateScore(),
                    WhenTestWasDone = DateTime.Now
                };
                DatabaseHelpers.SaveUserTestToDb(userTest);
                //var test = DatabaseHelpers.GetTestByID(_solveTestViewModel.ID);
                //DatabaseHelpers.RemoveTestByID(_solveTestViewModel.ID);
                //DatabaseHelpers.SaveTestToDb(test);
            }
            else
            {
                //DatabaseHelpers.SaveTestToDb(_solveTestViewModel);
            }

            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var datagrid = sender as DataGrid;

            if (datagrid == null) return;
            if (datagrid.CurrentCell.Column.Header.Equals("Poprawna"))
            {
                var item = (e.AddedItems[0] as ViewModel.Answer);
                if (item != null)
                    item.IsCorrect = !item.IsCorrect;
            }
            datagrid?.UnselectAllCells();

        }

        private void GoToTheNextQuestion_OnClick(object sender, RoutedEventArgs e)
        {
            _solveTestViewModel.GoToTheNextQuestion();
        }
        private void GoBackThePreviousQuestion_OnClick(object sender, RoutedEventArgs e)
        {
            _solveTestViewModel.GoBackToThePreviousQuestion();
        }
    }
}
