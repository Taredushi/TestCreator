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
using TestCreator.Enumerators;
using TestCreator.Events;

namespace TestCreator.Controls
{
    /// <summary>
    /// Interaction logic for ToolBar.xaml
    /// </summary>
    public partial class ToolBar : UserControl
    {
        public ToolBar()
        {
            InitializeComponent();
        }

        public void SetupForAdmin()
        {
            UsersButton.Visibility = Visibility.Visible;
            TestsButton.Visibility = Visibility.Visible;
            ImportButton.Visibility = Visibility.Visible;
            ExportButton.Visibility = Visibility.Visible;
            StatisticsButton.Visibility = Visibility.Visible;

        }

        public void SetupForUser()
        {
            UsersButton.Visibility = Visibility.Collapsed;
            ImportButton.Visibility = Visibility.Collapsed;
            ExportButton.Visibility = Visibility.Collapsed;
            TestsButton.Visibility = Visibility.Visible;
            StatisticsButton.Visibility = Visibility.Visible;
        }

        public event EventHandler ButtonClicked;

        private void UsersButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, new ToolBarEventArgs(){Option = ToolbarOption.User});
        }

        private void TestsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, new ToolBarEventArgs() { Option = ToolbarOption.Test });
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, new ToolBarEventArgs() { Option = ToolbarOption.Import });
        }

        private void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, new ToolBarEventArgs() { Option = ToolbarOption.Export });
        }

        private void StatisticsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, new ToolBarEventArgs() { Option = ToolbarOption.Statistics });
        }

    }
}
