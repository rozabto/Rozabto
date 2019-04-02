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
using System.Windows.Shapes;

namespace Rozabto.View {
    public partial class AddToPlayList : Window {
        private readonly Song[] songs;
        public AddToPlayList(Song[] songs) {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            this.songs = songs;
            DataContext = MainViewModel.PlayList;
        }

        private void CancelPlayListCreation(object sender, RoutedEventArgs e) {
            Close();
        }

        private void ChoosePlayList(object sender, MouseButtonEventArgs e) {
            MainViewModel.AddSongsToPlayList((PlayListList.SelectedItem as PlayList).Name, songs);
            Close();
        }
    }
}
