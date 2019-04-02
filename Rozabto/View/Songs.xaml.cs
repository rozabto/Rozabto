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

namespace Rozabto.View {
    public partial class Songs : UserControl {
        private string SelectedSongName;
        public Songs() {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }

        public void SelectedSong(object sender, MouseEventArgs e) {
            SelectedSongName = ((sender as StackPanel).Children[1] as Label).Content.ToString();
        }

        public void AddToPlayList(object sender, RoutedEventArgs e) {
            var add = new AddToPlayList(MainViewModel.GetSongFromName(SelectedSongName)) {
                Owner = (MainWindow)Application.Current.MainWindow
            };
            add.Show();
        }

        public void RemoveSong(object sender, RoutedEventArgs e) {
            MainViewModel.RemoveSong(SelectedSongName);
        }
    }
}
