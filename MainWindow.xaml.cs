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
using TestCreator.Enumerators;
using TestCreator.Events;
using TestCreator.Export;
using TestCreator.SubPages;

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private User _loggedUser;

        public MainWindow()
        {
            InitializeComponent();
            BottomToolbar.ButtonClicked += BottomToolbarOnButtonClicked;
            this.ResizeMode = ResizeMode.CanMinimize;
            var login = new LoginPage();
            login.PropertyChanged += CurrentPageOnPropertyChanged;
            ContentCtrl.Content = login;
        }

        private void BottomToolbarOnButtonClicked(object sender, EventArgs e)
        {
            if ((e as ToolBarEventArgs) == null) return;
            switch (((ToolBarEventArgs) e).Option)
            {
                case ToolbarOption.Test:
                    CreateTestPage();
                    break;
                case ToolbarOption.User:
                    CreateUserPage();
                    break;
                case ToolbarOption.Import:
                    break;
                case ToolbarOption.Export:
                    if (ContentCtrl.Content is TestsPage)
                    {
                        var saveDoc = new SaveDoc();
                        saveDoc.SaveTest(DatabaseHelpers.GetTestByID((ContentCtrl.Content as TestsPage).SelectedTestID), "test");
                    }
                    break;
                case ToolbarOption.Statistics:
                    break;
                case ToolbarOption.Settings:
                    break;
            }
        }

        private void CurrentPageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("UserLogin"))
            {
                SetupForLogged();
            }
            else if (e.PropertyName.Equals("Logout"))
            {
                SetupLoginPage();
            }
        }

        private void SetupForLogged()
        {
            this.ResizeMode = ResizeMode.CanResize;
            this.Width = 1024;
            this.Height = 768;

            _loggedUser = (ContentCtrl.Content as LoginPage).GetUser();
            CreateTestPage();
        }

        private void SetupLoginPage()
        {
            this.ResizeMode = ResizeMode.CanMinimize;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Width = 550;
            this.Height = 450;

            var page = new LoginPage();
            page.PropertyChanged += CurrentPageOnPropertyChanged;
            ContentCtrl.Content = page;
            ToolBarGrid.Visibility = Visibility.Collapsed;
        }

        private void EnableToolbar(Role role)
        {
            ToolBarGrid.Visibility = Visibility.Visible;
            switch (role)
            {
                case Role.Admin:
                    BottomToolbar.SetupForAdmin();
                    break;
                case Role.User:
                    BottomToolbar.SetupForUser();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        #region Creating pages
        private void CreateTestPage()
        {
            var page = new TestsPage(_loggedUser);
            page.PropertyChanged += CurrentPageOnPropertyChanged;
            ContentCtrl.Content = page;
            EnableToolbar((Role)_loggedUser.Role);
        }
        private void CreateUserPage()
        {
            var page = new UsersPage(_loggedUser);
            page.PropertyChanged += CurrentPageOnPropertyChanged;
            ContentCtrl.Content = page;
            EnableToolbar((Role)_loggedUser.Role);
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
