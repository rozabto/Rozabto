using Microsoft.Win32;
using Rozabto.ViewModel;
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

namespace Rozabto {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void NextSong(object sender, RoutedEventArgs e) {

        }

        private void PreviousSong(object sender, RoutedEventArgs e) {

        }

        private void GetNewSongs(object sender, RoutedEventArgs e) {
            var fileDialog = new OpenFileDialog {
                Filter = "mp3|*.mp3",
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Select Music Files"
            };
            var result = fileDialog.ShowDialog();
            if (result == true && string.IsNullOrWhiteSpace(fileDialog.FileName) || result == false)
                return;
            MainViewModel.AddSongs(fileDialog.FileNames);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
