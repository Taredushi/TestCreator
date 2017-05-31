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
    /// Interaction logic for Combobox.xaml
    /// </summary>
    public partial class Combobox : UserControl
    {

        private string _selectedItem;

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public Combobox()
        {
            InitializeComponent();
        }

        public void AddComboboxItem(string name)
        {
            Cbox.Items.Add(name);
        }

        #region Events
        private void Cbox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = Cbox.SelectedItem.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        
    }
}
