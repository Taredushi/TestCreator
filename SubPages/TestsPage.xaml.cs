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
using TestCreator.ViewModel;

namespace TestCreator.SubPages
{
    /// <summary>
    /// Interaction logic for TestsPage.xaml
    /// </summary>
    public partial class TestsPage
    {
        public ObservableCollection<TestViewModel> TestCollection { get; set; }

        public TestsPage()
        {
            InitializeComponent();
            TestCollection = new ObservableCollection<TestViewModel>();
            LoadTestList();
            TestsListView.ItemsSource = TestCollection;
        }

        public TestsPage(User user)
        {
            InitializeComponent();
            LoggedUser = user;
            TestCollection = new ObservableCollection<TestViewModel>();
            LoadTestList();
            TestsListView.ItemsSource = TestCollection;
        }

        private ListCollectionView View
        {
            get
            {
                return (ListCollectionView)CollectionViewSource.GetDefaultView(TestCollection);
            }
        }


        #region Dependency Properties

        public static readonly DependencyProperty UserPropert = DependencyProperty.Register(
            "LoggedUser", typeof(User), typeof(LoginPage), new PropertyMetadata(default(User)));

        public User LoggedUser
        {
            get { return (User)GetValue(UserPropert); }
            set
            {
                SetValue(UserPropert, value);
            }
        }

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void AddTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var testdlg = new TestWindow(true);

            if (testdlg.ShowDialog() == true)
            {
                MessageBox.Show("true");
            }
            else
            {
                MessageBox.Show("false");
            }
        }

        private void LoadTestList()
        {
            var list = DatabaseHelpers.GetAllTest();
            foreach (var test in list)
            {
                var testViewModel = new TestViewModel(false);
                testViewModel.CreateModel(test);
                TestCollection.Add(testViewModel);
            }
        }

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
                    View.Filter = delegate(object item)
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
    }
}
