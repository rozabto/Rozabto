using Rozabto.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View
{
    public partial class MiniNowplaying : UserControl
    {
        public MiniNowplaying()
        {
            InitializeComponent();
            DataContext = MainViewModel.NowPlaying;
        }

        public void PlayPause(object sender, RoutedEventArgs e)
        {
            MediaViewModel.TimerPlay();
        }

        public void MoveBackSong(object sender, RoutedEventArgs e)
        {
            // Преместваме с една песен назад.
            if (MainViewModel.NowPlaying.Songs.Count <= 1 || --MainViewModel.NowPlaying.CurrentSongPos == -1)
            {
                MediaViewModel.Stop();
                return;
            }
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[MainViewModel.NowPlaying.CurrentSongPos];
            MediaViewModel.Stop();
            MediaViewModel.TimerPlay();
        }

        public void MoveSongForward(object sender, RoutedEventArgs e)
        {
            // Преместваме с една песен напред.
            if (MainViewModel.NowPlaying.Songs.Count <= 1)
            {
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
            MediaViewModel.TimerPlay();
        }
    }
}
