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
        private string SelectedSongName;
        public ABPContent() {
            InitializeComponent();
            DataContext = MainViewModel.ABP;
        }

        public void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void RemoveSong(object sender, RoutedEventArgs e) {
            if (MainViewModel.ABP.IsPlayList)
                MainViewModel.RemoveSongFromPlayList(SelectedSongName);
            else MainViewModel.RemoveSong(SelectedSongName, true);
        }

        private void RemoveSong(object sender, MouseEventArgs e) {
            SelectedSongName = ((sender as StackPanel).Children[1] as Label).Content.ToString();
        }
    }
}
