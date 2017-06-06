using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestCreator.Database;
using TestCreator.Enumerators;
using TestCreator.ViewModel;

namespace TestCreator.SubPages
{
    /// <summary>
    /// Interaction logic for TestsPage.xaml
    /// </summary>
    public partial class TestsPage
    {
        public ObservableCollection<TestViewModel> TestCollection { get; set; }

        public string Title { get; set; }

        public TestsPage()
        {
            InitializeComponent();
            TestCollection = new ObservableCollection<TestViewModel>();
            LoadTestList();
            TestsListView.ItemsSource = TestCollection;
            InitializeSortCombobox();
            DataContext = this;
            TopPanel.Logout += Logout;
            Title = "Testy";
        }

        public TestsPage(User user)
        {
            InitializeComponent();
            LoggedUser = user;
            TestCollection = new ObservableCollection<TestViewModel>();
            LoadTestList();
            TestsListView.ItemsSource = TestCollection;
            InitializeSortCombobox();
            DataContext = this;
            TopPanel.Logout += Logout;
            Title = "Testy";
        }

        private ListCollectionView View
        {
            get
            {
                return (ListCollectionView)CollectionViewSource.GetDefaultView(TestCollection);
            }
        }

        private void InitializeSortCombobox()
        {
            SortCombobox.AddComboboxItem("Nazwa");
            SortCombobox.AddComboboxItem("Ilość pytań");
            SortCombobox.AddComboboxItem("Wszystkich pytań");
            SortCombobox.PropertyChanged += SortComboboxOnPropertyChanged;
        }

        private void LoadTestList()
        {
            TestCollection.Clear();
            var list = DatabaseHelpers.GetAllTest();
            foreach (var test in list)
            {
                var testViewModel = new TestViewModel(false);
                testViewModel.CreateSimpleModel(test);
                TestCollection.Add(testViewModel);
            }
        }


        #region Dependency Properties

        public static readonly DependencyProperty UserPropert = DependencyProperty.Register(
            "LoggedUser", typeof(User), typeof(TestsPage), new PropertyMetadata(default(User)));

        public User LoggedUser
        {
            get { return (User)GetValue(UserPropert); }
            set
            {
                SetValue(UserPropert, value);
                LoggedUserName = LoggedUser.Name + " " + LoggedUser.Surname;
                LoggedUserRole = Enum.GetName(typeof(Role), (Role) LoggedUser.Role);
                UserAvatar = LoggedUser.AvatarBitmap;
            }
        }

        public static readonly DependencyProperty UserNamePropert = DependencyProperty.Register(
            "LoggedUserName", typeof(string), typeof(TestsPage), new PropertyMetadata(default(string)));

        public string LoggedUserName
        {
            get { return (string)GetValue(UserNamePropert); }
            set
            {
                SetValue(UserNamePropert, value);
            }
        }

        public static readonly DependencyProperty UserRolePropert = DependencyProperty.Register(
            "LoggedUserRole", typeof(string), typeof(TestsPage), new PropertyMetadata(default(string)));

        public string LoggedUserRole
        {
            get { return (string)GetValue(UserRolePropert); }
            set
            {
                SetValue(UserRolePropert, value);
            }
        }

        public static readonly DependencyProperty UserAvatarPropert = DependencyProperty.Register(
            "UserAvatar", typeof(BitmapImage), typeof(TestsPage), new PropertyMetadata(default(string)));

        public BitmapImage UserAvatar
        {
            get { return (BitmapImage)GetValue(UserAvatarPropert); }
            set
            {
                SetValue(UserAvatarPropert, value);
            }
        }

        #endregion

        #region Events

        private void SearchField_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(SearchField.SearchBox.Text))
                {
                    View.Filter = null;
                }
                else
                {
                    var search = SearchField.SearchBox.Text.ToLower();
                    View.Filter = delegate (object item)
                    {
                        var test = item as TestViewModel;
                        if (test != null)
                        {
                            return test.Name.ToLower().Contains(search);
                        }
                        return false;
                    };
                }

            }
        }

        private void SortComboboxOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (SortCombobox.SelectedItem.Equals("Nazwa"))
            {
                View.CustomSort = new SortByName();
            }
            else if (SortCombobox.SelectedItem.Equals("Ilość pytań"))
            {
                View.CustomSort = new SortByQuestionsLimit();
            }
            else if (SortCombobox.SelectedItem.Equals("Wszystkich pytań"))
            {
                View.CustomSort = new SortBySortQuestionsNumber();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Sort

        private class SortByName : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                TestViewModel test1 = (TestViewModel)x;
                TestViewModel test2 = (TestViewModel)y;
                try
                {

                    return String.Compare(test1.Name, test2.Name, StringComparison.Ordinal);
                }
                catch (Exception)
                {

                    if (string.IsNullOrEmpty(test1.Name))
                    {
                        return -1;
                    }
                    else if (string.IsNullOrEmpty(test2.Name))
                    {
                        return 1;
                    }
                    return 0;
                }

            }
        }

        private class SortByQuestionsLimit : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                TestViewModel test1 = (TestViewModel)x;
                TestViewModel test2 = (TestViewModel)y;
                try
                {
                    int one = int.Parse(test1.QuestionsLimit);
                    int two = int.Parse(test2.QuestionsLimit);

                    return one.CompareTo(two);
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }

        private class SortBySortQuestionsNumber : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                TestViewModel test1 = (TestViewModel)x;
                TestViewModel test2 = (TestViewModel)y;
                try
                {
                    return test1.QuestionsNumber.CompareTo(test2.QuestionsNumber);
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        #endregion

        #region TestButtons
        private void AddTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var testdlg = new TestWindow(true);

            if (testdlg.ShowDialog() == true)
            {
                LoadTestList();
            }
        }

        private void DeleteTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            TestCollection.Remove(TestsListView.SelectedItem as TestViewModel);
        }

        private void EditTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            var test = DatabaseHelpers.GetTestByID((TestsListView.SelectedItem as TestViewModel).ID);
            var model = new TestViewModel(false);
            model.Title = "Edytuj test";
            model.CreateModel(test);

            var testdlg = new TestWindow(model);

            if (testdlg.ShowDialog() == true)
            {
                LoadTestList();
            }

        }
        #endregion

        private void Logout(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged("Logout");
        }

        private void SolveTestButton_OnClickTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var test = DatabaseHelpers.GetTestByID((TestsListView.SelectedItem as TestViewModel).ID);
            var model = new SolveTestViewModel {Title = "Rozwiąż Test"};
            model.CreateModel(test);

            var solveTestdlg = new SolveTestWindow(model,LoggedUser.UserID);

            if (solveTestdlg.ShowDialog() == true)
            {
                //LoadTestList();
            }
        }


    }
}
