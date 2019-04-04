using Rozabto.Model;
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
    public partial class ABPContent : UserControl {
        private bool isOpened;
        private string SelectedSongName;
        public ABPContent() {
            InitializeComponent();
            DataContext = MainViewModel.ABP;
        }

        private void RemoveSong(object sender, RoutedEventArgs e) {
            if (MainViewModel.ABP.IsPlayList)
                MainViewModel.RemoveSongFromPlayList(SelectedSongName);
            else MainViewModel.RemoveSong(SelectedSongName, true);
            isOpened = false;
        }

        private void SelectSong(object sender, MouseEventArgs e) {
            if (!isOpened)
                SelectedSongName = ((sender as StackPanel).Children[0] as Label).Content.ToString();
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e) {
            isOpened = true;
        }
    }
}
