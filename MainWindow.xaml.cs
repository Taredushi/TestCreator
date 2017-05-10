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
using TestCreator.Interfaces;
using TestCreator.SubPages;

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPage _currentPage;

        public IPage CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                if (!(_currentPage is LoginPage))
                {
                    this.ResizeMode = ResizeMode.CanResize;
                    this.Width = 1024;
                    this.Height = 768;
                    ToolBarGrid.Visibility = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.CanMinimize;
            //CurrentPage = new LoginPage();
            CurrentPage = new TestsPage();
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
