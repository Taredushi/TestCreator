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

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage
    {
        #region Private Fields
        private string _errorString = "Podany Login lub/oraz Hasło są niepoprawne.";
        private string _loginString = "login";
        private string _passwordString = "hasło";
        #endregion

        #region Constructor
        public LoginPage()
        {
            InitializeComponent();
            LoginTb.Text = _loginString;
            PasswordTb.Text = _passwordString;
            LoginForeColor = Brushes.LightGray;
            PasswordForeColor = Brushes.LightGray;
            DataContext = this;
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty UserPropert = DependencyProperty.Register(
            "User", typeof(User), typeof(LoginPage), new PropertyMetadata(default(User)));

        public User User
        {
            get { return (User) GetValue(UserPropert); }
            set
            {
                SetValue(UserPropert, value);
                this.Visibility = Visibility.Collapsed;
                OnPropertyChanged("UserLogin");
            }
        }

        public static readonly DependencyProperty LoginForeColorPropert = DependencyProperty.Register(
            "LoginForeColor", typeof(Brush), typeof(LoginPage), new PropertyMetadata(default(Brush)));

        public Brush LoginForeColor
        {
            get { return (Brush)GetValue(LoginForeColorPropert); }
            set { SetValue(LoginForeColorPropert, value); }
        }

        public static readonly DependencyProperty PasswordForeColorPropert = DependencyProperty.Register(
            "PasswordForeColor", typeof(Brush), typeof(LoginPage), new PropertyMetadata(default(Brush)));

        public Brush PasswordForeColor
        {
            get { return (Brush)GetValue(PasswordForeColorPropert); }
            set { SetValue(PasswordForeColorPropert, value); }
        }

        #endregion

        #region Events
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            MyDbContext db = new MyDbContext();

            if (!string.IsNullOrEmpty(LoginTb.Text) && !string.IsNullOrEmpty(PasswordTb.Text))
            {
                var pass = Encryptor.MD5Hash(PasswordTb.Text);

                try
                {
                    User = db.Users.Single(u => u.Login.Equals(LoginTb.Text) &&
                                                    u.Password.Equals(pass));
                }
                catch(Exception ex)
                {
                    ShowLoginErrorInfo();
                }
            }
            else
            {
                ShowLoginErrorInfo();
            }
        }

        private void LoginTb_OnGotFocus(object sender, RoutedEventArgs e)
        {
            LoginForeColor = Brushes.Black;
            LoginTb.Text = LoginTb.Text.ToLower().Equals(_loginString) ? "" : LoginTb.Text;
            LoginTb.CaretIndex = LoginTb.Text.Length;
        }

        private void PasswordTb_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PasswordForeColor = Brushes.Black;
            PasswordTb.Text = PasswordTb.Text.ToLower().Equals(_passwordString) ? "" : PasswordTb.Text;
            PasswordTb.CaretIndex = PasswordTb.Text.Length;
        }

        private void LoginTb_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginTb.Text)) return;

            LoginForeColor = Brushes.LightGray;
            LoginTb.Text = _loginString;
        }

        private void PasswordTb_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordTb.Text)) return;

            PasswordForeColor = Brushes.LightGray;
            PasswordTb.Text = _passwordString;
        }

        private void LoginPage_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_OnClick(this, null);
            }
        }

        #endregion

        #region Methods
        private void ShowLoginErrorInfo()
        {
            ErrorTb.Text = _errorString;
            ErrorTb.Visibility = Visibility.Visible;
        }

        public User GetUser()
        {
            return User;
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        
    }
}
