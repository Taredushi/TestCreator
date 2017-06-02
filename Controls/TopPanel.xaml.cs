using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TestCreator.Controls
{
    /// <summary>
    /// Interaction logic for TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {

        public EventHandler Logout;

        public TopPanel()
        {
            InitializeComponent();
        }

        #region DependencyProperties
        public static readonly DependencyProperty TitlePropert = DependencyProperty.Register(
            "Title", typeof(string), typeof(TopPanel), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string)GetValue(TitlePropert); }
            set
            {
                SetValue(TitlePropert, value);
            }
        }

        public static readonly DependencyProperty UserNamePropert = DependencyProperty.Register(
            "LoggedUserName", typeof(string), typeof(TopPanel), new PropertyMetadata(default(string)));

        public string LoggedUserName
        {
            get { return (string)GetValue(UserNamePropert); }
            set
            {
                SetValue(UserNamePropert, value);
            }
        }

        public static readonly DependencyProperty UserRolePropert = DependencyProperty.Register(
            "LoggedUserRole", typeof(string), typeof(TopPanel), new PropertyMetadata(default(string)));

        public string LoggedUserRole
        {
            get { return (string)GetValue(UserRolePropert); }
            set
            {
                SetValue(UserRolePropert, value);
            }
        }
        #endregion

        private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            Logout?.Invoke(this, new EventArgs());
        }
    }
}
