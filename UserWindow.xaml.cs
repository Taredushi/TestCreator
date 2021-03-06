﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using TestCreator.Database;
using TestCreator.Enumerators;
using TestCreator.Helpers;

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public string WindowName { get; set; }
        public User User { get; set; }

        private List<string> _listOfRoles;

        public UserWindow()
        {
            InitializeComponent();
            User = new User()
            {
                Role = (int)Role.User
            };
            SetupRoleComboboxSource();
            DataContext = this;
            PasswordTextBlock.Text = "Hasło";
        }

        public UserWindow(User user)
        {
            InitializeComponent();
            this.User = user;
            SetupRoleComboboxSource();
            DataContext = this;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            //validation
            if (Validate_User())
            {
                CheckNewPassword();
                this.DialogResult = true;
                this.Close();
            }
           
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SetupRoleComboboxSource()
        {
            _listOfRoles = new List<string>();

            for (int i = 0; i <= EnumMaxValue.GetMaxValue(typeof(Role)); i++)
            {
                var name = Enum.GetName(typeof(Role), i);
                _listOfRoles.Add(name);
            }

            RoleCombobox.ItemsSource = _listOfRoles;

            if (User != null)
            {
                RoleCombobox.SelectedIndex = User.Role;
            }
        }

        public void SaveToDatabase()
        {
            User.Avatar = BitmapConverter.ConvertBitmapToBytes(AvatarImage.Source as BitmapImage);
            DatabaseHelpers.SaveUser(User);
        }

        public void CheckNewPassword()
        {
            if (!string.IsNullOrEmpty(Passwordbox.Password))
            {
                User.Password = Encryptor.MD5Hash(Passwordbox.Password);
            }
        }

        private void RoleCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User.Role = RoleCombobox.SelectedIndex;
        }

        private void AvatarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Pliki obrazu | *.jpg;*.jpeg;*.png;*.bmp";
            fileDialog.Multiselect = false;
            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                AvatarImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

      
        /// Returns true if form is valid, else returns false.
        private bool Validate_User()
        {
            if (!EmailTb.Text.IsValidEmailAddress())
            {
                MessageBox.Show("Nieprawidłowy adres e-mail!", "Email error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if(string.IsNullOrEmpty(LoginTb.Text))
            {
                MessageBox.Show("Nieprawidłowy Login!", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if(AvatarImage.Source == null)
            {
                if(MessageBox.Show("Avatar jest pusty! Chcesz kontynuować mimo to?", "Avatar error", 
                    MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    AvatarImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/add_user.png"));
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
