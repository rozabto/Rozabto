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
    public partial class MiniNowplaying : UserControl {
        public MiniNowplaying() {
            InitializeComponent();
            DataContext = MainViewModel.NowPlaying;
        }

        private void PlayPause(object sender, RoutedEventArgs e) {
            MediaViewModel.Play();
        }

        private void MoveBackSong(object sender, RoutedEventArgs e) {
            // Преместваме с една песен назад.
            if (MainViewModel.NowPlaying.Songs.Count <= 1 || --MainViewModel.NowPlaying.CurrentSongPos == -1) {
                MediaViewModel.Stop();
                return;
            }
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[MainViewModel.NowPlaying.CurrentSongPos];
            MediaViewModel.Stop();
            MediaViewModel.Play();
        }

        private void MoveSongForward(object sender, RoutedEventArgs e) {
            // Преместваме с една песен напред.
            if (MainViewModel.NowPlaying.Songs.Count <= 1) {
                MediaViewModel.Stop();
                return;
            }
            var pos = MainViewModel.NowPlaying.ShuffleSongs ? new Random().Next(MainViewModel.NowPlaying.Songs.Count)
                : MainViewModel.NowPlaying.CurrentSongPos + 1;
            if (pos >= MainViewModel.NowPlaying.Songs.Count)
                pos -= MainViewModel.NowPlaying.Songs.Count;
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[pos];
            MainViewModel.NowPlaying.CurrentSongPos = pos;
            MediaViewModel.Stop();
            MediaViewModel.Play();
        }
    }
}
