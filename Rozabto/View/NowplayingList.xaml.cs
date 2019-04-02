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
    public partial class NowplayingList : UserControl {
        private string SelectedSongName;
        public NowplayingList() {
            InitializeComponent();
            DataContext = MainViewModel.NowPlaying;
        }
        public void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            // Пускаме песента, избрана от листа.
            var playListBox = sender as ListBox;
            var song = playListBox.SelectedItem as Song;
            playListBox.UpdateLayout();
            MediaViewModel.Stop();
            MainViewModel.NowPlaying.CurrentSong = song;
            MainViewModel.NowPlaying.CurrentSongPos = playListBox.SelectedIndex;
            MediaViewModel.Play();
        }

        public void FocusOnSelectedItem(object sender, SelectionChangedEventArgs e) {
            SongsList.ScrollIntoView(SongsList.SelectedItem);
        }

        public void SelectedSong(object sender, MouseEventArgs e) {
            SelectedSongName = ((sender as StackPanel).Children[2] as TextBlock).Text;
        }

        public void AddToPlayList(object sender, RoutedEventArgs e) {
            //var objBlur = new System.Windows.Media.Effects.BlurEffect();
            //((MainWindow)Application.Current.MainWindow).Effect = objBlur;
            //var add = new AddToPlayList(SelectedSongName);
            //add.Show();
            //add.Closed += Add_Closed;
        }

        public void Add_Closed(object sender, EventArgs e) {
            ((MainWindow)Application.Current.MainWindow).Effect = null;
        }

        public void RemoveSong(object sender, RoutedEventArgs e) {
            MainViewModel.RemoveSong(SelectedSongName);
        }
    }
}
