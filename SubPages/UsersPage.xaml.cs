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
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : UserControl
    {
        public string Title { get; set; }

        public ObservableCollection<UserViewModel> UserCollection { get; set; }

        public UsersPage()
        {
            InitializeComponent();
            UserCollection = new ObservableCollection<UserViewModel>();
            LoadUsersList();
            UsersListView.ItemsSource = UserCollection;
            InitializeSortCombobox();
            DataContext = this;
            TopPanel.Logout += Logout;
            Title = "Użytkownicy";
        }

        public UsersPage(User user)
        {
            InitializeComponent();
            LoggedUser = user;
            UserCollection = new ObservableCollection<UserViewModel>();
            LoadUsersList();
            UsersListView.ItemsSource = UserCollection;
            InitializeSortCombobox();
            DataContext = this;
            TopPanel.Logout += Logout;
            Title = "Użytkownicy";
        }

        private ListCollectionView View
        {
            get
            {
                return (ListCollectionView)CollectionViewSource.GetDefaultView(UserCollection);
            }
        }

        private void InitializeSortCombobox()
        {
            SortCombobox.AddComboboxItem("Imię");
            SortCombobox.AddComboboxItem("Nazwisko");
            SortCombobox.AddComboboxItem("Rola");
            SortCombobox.PropertyChanged += SortComboboxOnPropertyChanged;
        }

        private void LoadUsersList()
        {
            UserCollection.Clear();
            var list = DatabaseHelpers.GetAllUsers();
            foreach (var user in list)
            {
                var userViewModel = new UserViewModel();
                userViewModel.CreateSimpleModel(user);
                UserCollection.Add(userViewModel);
            }
        }


        #region Dependency Properties

        public static readonly DependencyProperty UserPropert = DependencyProperty.Register(
            "LoggedUser", typeof(User), typeof(UsersPage), new PropertyMetadata(default(User)));

        public User LoggedUser
        {
            get { return (User)GetValue(UserPropert); }
            set
            {
                SetValue(UserPropert, value);
                LoggedUserName = LoggedUser.Name + " " + LoggedUser.Surname;
                LoggedUserRole = Enum.GetName(typeof(Role), (Role)LoggedUser.Role);
            }
        }

        public static readonly DependencyProperty UserNamePropert = DependencyProperty.Register(
            "LoggedUserName", typeof(string), typeof(UsersPage), new PropertyMetadata(default(string)));

        public string LoggedUserName
        {
            get { return (string)GetValue(UserNamePropert); }
            set
            {
                SetValue(UserNamePropert, value);
            }
        }

        public static readonly DependencyProperty UserRolePropert = DependencyProperty.Register(
            "LoggedUserRole", typeof(string), typeof(UsersPage), new PropertyMetadata(default(string)));

        public string LoggedUserRole
        {
            get { return (string)GetValue(UserRolePropert); }
            set
            {
                SetValue(UserRolePropert, value);
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
                        var test = item as UserViewModel;
                        if (test != null)
                        {
                            return test.FullName.ToLower().Contains(search);
                        }
                        return false;
                    };
                }

            }
        }

        private void SortComboboxOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (SortCombobox.SelectedItem.Equals("Imię"))
            {
                View.CustomSort = new SubPages.UsersPage.SortByName();
            }
            else if (SortCombobox.SelectedItem.Equals("Nazwisko"))
            {
                View.CustomSort = new SubPages.UsersPage.SortBySurame();
            }
            else if (SortCombobox.SelectedItem.Equals("Rola"))
            {
                View.CustomSort = new SubPages.UsersPage.SortByRole();
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
                UserViewModel user1 = (UserViewModel)x;
                UserViewModel user2 = (UserViewModel)y;
                try
                {

                    return String.Compare(user1.Name, user2.Name, StringComparison.Ordinal);
                }
                catch (Exception)
                {

                    if (string.IsNullOrEmpty(user1.Name))
                    {
                        return -1;
                    }
                    else if (string.IsNullOrEmpty(user2.Name))
                    {
                        return 1;
                    }
                    return 0;
                }

            }
        }

        private class SortBySurame : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                UserViewModel user1 = (UserViewModel)x;
                UserViewModel user2 = (UserViewModel)y;
                try
                {

                    return String.Compare(user1.Surname, user2.Surname, StringComparison.Ordinal);
                }
                catch (Exception)
                {

                    if (string.IsNullOrEmpty(user1.Name))
                    {
                        return -1;
                    }
                    else if (string.IsNullOrEmpty(user2.Name))
                    {
                        return 1;
                    }
                    return 0;
                }

            }
        }

        private class SortByRole : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                UserViewModel user1 = (UserViewModel)x;
                UserViewModel user2 = (UserViewModel)y;
                try
                {
                    return ((int) user1.Role).CompareTo((int) user2.Role);
                }
                catch (Exception)
                {

                    if (string.IsNullOrEmpty(user1.Name))
                    {
                        return -1;
                    }
                    else if (string.IsNullOrEmpty(user2.Name))
                    {
                        return 1;
                    }
                    return 0;
                }

            }
        }
        #endregion

        #region UserButtons
        private void AddUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            var  userdlg = new UserWindow();
            userdlg.WindowName = "Dodaj użytkownika";

            if (userdlg.ShowDialog() == true)
            {
                userdlg.SaveToDatabase();
                LoadUsersList();
            }
        }

        private void DeleteUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserCollection.Remove(UsersListView.SelectedItem as UserViewModel);
        }

        private void EditUserButton_OnClick(object sender, RoutedEventArgs e)
        {

            var user = DatabaseHelpers.GetUserByID((UsersListView.SelectedItem as UserViewModel).ID);

            var userdlg = new UserWindow(user);
            userdlg.WindowName = "Edycja użytkownika";

            if (userdlg.ShowDialog() == true)
            {
                userdlg.SaveToDatabase();
                LoadUsersList();
            }

        }
        #endregion

        private void Logout(object sender, EventArgs eventArgs)
        {
            OnPropertyChanged("Logout");
        }
    }
}
