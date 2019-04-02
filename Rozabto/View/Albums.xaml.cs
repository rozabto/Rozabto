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
    public partial class Album : UserControl {
        private string SelectedAlbumName;
        public Album() {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }
         
        public void SelectAlbum(object sender, SelectionChangedEventArgs e) {
            var listbox = sender as ListBox;
            // Взимаме grid който показва страницата от MainWindow.
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            // Активираме ABP с избрания албум от листа.
            MainViewModel.ActivateABP(MainViewModel.Collection.Albums[listbox.SelectedIndex]);
            grid.Children.Add(new ABPContent());
        }

        public void SelectedAlbum(object sender, MouseEventArgs e) {
            SelectedAlbumName = ((sender as DockPanel).Children[1] as Label).Content.ToString();
        }

        public void AddToPlayList(object sender, RoutedEventArgs e) {
            var add = new AddToPlayList(MainViewModel.GetSongsFromAlbum(SelectedAlbumName)) {
                Owner = (MainWindow)Application.Current.MainWindow
            };
            add.Show();
        }

        public void RemoveAlbum(object sender, RoutedEventArgs e) {
            MainViewModel.RemoveAlbum(SelectedAlbumName);
        }
    }
}
