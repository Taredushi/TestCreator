using System;
using System.Collections.Generic;
using System.IO;
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
using TestCreator.Database;
using TestCreator.Export;
using TestCreator.Helpers;

namespace TestCreator
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        private Test _test;
        private readonly string _appPath;
        private readonly string _exportPath;
        public string WindowName { get; set; }

        public ExportWindow(Test test)
        {
            InitializeComponent();
            WindowName = "Export";
            DataContext = this;
            this._test = test;
            _appPath = AppDomain.CurrentDomain.BaseDirectory;
            _exportPath = _appPath + "\\Export\\";

        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void XmlExport_OnClick(object sender, RoutedEventArgs e)
        {
            var xml = XmlGenerator.ConvertTestToTestXml(_test);
            XmlGenerator.SaveXml(xml, GetFilePath());

            MessageBox.Show("Exportowanie zakończone sukcesem.", "", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void DocxExport_OnClick(object sender, RoutedEventArgs e)
        {
            CreateFolderIfNotExists(_exportPath);
            string path = GetFilePath();
            var saveDoc = new SaveDoc();
            saveDoc.SaveTestToWord(_test, path);

            MessageBox.Show("Exportowanie zakończone sukcesem.", "", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void PdfExport_OnClick(object sender, RoutedEventArgs e)
        {
            CreateFolderIfNotExists(_exportPath);
            string path = GetFilePath();
            var saveDoc = new SaveDoc();
            saveDoc.SaveTestToPdf(_test, path);

            MessageBox.Show("Exportowanie zakończone sukcesem.", "", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void CreateFolderIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string GetFilePath()
        {
            if (_test.Name.Length > 6)
            {
                return _exportPath + _test.Name.Remove(6);
            }
            return _exportPath + _test.Name;
        }
    }
}
