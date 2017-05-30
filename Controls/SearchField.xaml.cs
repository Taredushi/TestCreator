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

namespace TestCreator.Controls
{
    /// <summary>
    /// Interaction logic for SearchField.xaml
    /// </summary>
    public partial class SearchField
    {
        private string _searchText;
        private Brush _foreColor;
        private int _fSize;

        public Brush ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                OnPropertyChanged();
            }
        }

        public int FSize
        {
            get { return _fSize; }
            set
            {
                _fSize = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public SearchField()
        {
            InitializeComponent();
            ForeColor = Brushes.DimGray;
            FSize = 14;
            SearchText = "wyszukaj";
            DataContext = this;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.ToLower().Equals("wyszukaj"))
            {
                SearchBox.Text = "";
            }
        }

        private void SearchBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                SearchBox.Text = "wyszukaj";
            }
        }
    }
}
