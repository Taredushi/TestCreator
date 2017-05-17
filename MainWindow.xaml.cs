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
using TestCreator.Enumerators;
using TestCreator.SubPages;

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.CanMinimize;
            var login = new LoginPage();
            login.PropertyChanged += CurrentPageOnPropertyChanged;
            ContentCtrl.Content = login;

            //do testów okna testów
            //var testdlg = new TestWindow();

            //if (testdlg.ShowDialog() == true)
            //{

            //}
        }

        private void CurrentPageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("UserLogin"))
            {
                SetupForLogged();
            }
        }

        private void SetupForLogged()
        {
            this.ResizeMode = ResizeMode.CanResize;
            this.Width = 1024;
            this.Height = 768;

            var loggedUser = (ContentCtrl.Content as LoginPage).GetUser();

            var page = new TestsPage(loggedUser);
            ContentCtrl.Content = page;
            EnableToolbar((Enums.Role)loggedUser.Role);
        }

        private void EnableToolbar(Enums.Role role)
        {
            ToolBarGrid.Visibility = Visibility.Visible;
            switch (role)
            {
                case Enums.Role.Admin:
                    BottomToolbar.SetupForAdmin();
                    break;
                case Enums.Role.User:
                    BottomToolbar.SetupForUser();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
