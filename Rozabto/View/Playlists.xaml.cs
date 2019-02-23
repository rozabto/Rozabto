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
    public partial class Playlists : UserControl {
        public Playlists() {
            InitializeComponent();
            DataContext = MainViewModel.PlayList;
        }

        private void AddPlayList(object sender, RoutedEventArgs e) {
            var text = Playlist.Text;
            if (string.IsNullOrWhiteSpace(text) || text.Length < 5 || text.Length > 20)
                return;
            MainViewModel.AddPlayList(text);
            Playlistdialog.IsOpen = false;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var view = sender as Viewbox;

        }

        private void GoToPlayList(object sender, MouseButtonEventArgs e) {
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            var name = ((sender as DockPanel).Children[1] as Label).Content.ToString();
            MainViewModel.ActivateABP("playlist", name);
            grid.Children.Add(new ABPContent());
        }
    }
}
