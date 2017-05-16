using System;
using System.Collections.Generic;
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

namespace TestCreator.SubPages
{
    /// <summary>
    /// Interaction logic for TestsPage.xaml
    /// </summary>
    public partial class TestsPage
    {

        public TestsPage()
        {
            InitializeComponent();
        }

        public TestsPage(User user)
        {
            InitializeComponent();
            LoggedUser = user;
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
            var testdlg = new TestWindow();

            if (testdlg.ShowDialog() == true)
            {
                
            }
        }
    }
}
