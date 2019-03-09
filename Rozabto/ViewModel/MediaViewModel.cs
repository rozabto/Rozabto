using MaterialDesignThemes.Wpf;
using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Rozabto.ViewModel {
    public static class MediaViewModel {
        private static readonly DispatcherTimer SliderTimer;
        private static Slider MusicSlider;
        private static Slider VolumeSlider;
        private static Label SongTime;
        private static int VolumeValue;
        public static bool SliderDragging { get; set; }

        static MediaViewModel() {
            MainViewModel.Player.MediaFailed += Player_MediaFailed;
            MainViewModel.Player.MediaOpened += Player_MediaOpened;
            MainViewModel.Player.MediaEnded += Player_MediaEnded;

            MainViewModel.Player.Volume = 0.5f;
            VolumeValue = 50;

            SliderTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            SliderTimer.Tick += SliderTimer_Tick;
        }

        public static void ConnectViewToViewModel(Slider musicSlider, Slider volumeSlider, Label songTime) {
            MusicSlider = musicSlider;
            VolumeSlider = volumeSlider;
            SongTime = songTime;
            VolumeSlider.Value = VolumeValue;
        }

        private static void Player_MediaFailed(object sender, ExceptionEventArgs e) {
            MessageBox.Show(e.ErrorException.Message);
            Stop();
        }

        private static void SliderTimer_Tick(object sender, EventArgs e) {
            if (SliderDragging || MusicSlider is null) return;
            MusicSlider.Value = MainViewModel.Player.Position.TotalSeconds;
            SongTime.Content = MainViewModel.Player.Position.ToString(@"mm\:ss");
        }

        private static void Player_MediaEnded(object sender, EventArgs e) {
            MusicSlider.Value = 0;
            SongTime.Content = "";
            SliderTimer.Stop();
            MainViewModel.Status = SongStatus.Stopped;
            if (MainViewModel.NowPlaying.RepeatSong) {
                Play();
                return;
            }
            if (MainViewModel.NowPlaying.Songs.Count <= 0)
                return;
            var pos = MainViewModel.NowPlaying.ShuffleSongs ? new Random().Next(MainViewModel.NowPlaying.Songs.Count)
                : MainViewModel.NowPlaying.CurrentSongPos + 1;
            if (pos >= MainViewModel.NowPlaying.Songs.Count)
                pos -= MainViewModel.NowPlaying.Songs.Count;
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[pos];
            MainViewModel.NowPlaying.CurrentSongPos = pos;
            Play();
        }

        private static void Player_MediaOpened(object sender, EventArgs e) {
            var player = sender as MediaPlayer;
            player.Position = new TimeSpan(0, 0, 0);
            MusicSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Minimum = 0;
            player.Volume = MainViewModel.Volume == VolumeState.Zero ? 0 
                : Math.Round(Math.Pow(VolumeSlider.Value / 100d, 1.150515), 3);
            MainViewModel.NowPlaying.OnPropertyChanged("SongBand");
        }

        public static void Play() {
            MainViewModel.Play();
            switch (MainViewModel.Status) {
                case SongStatus.Playing:
                    SliderTimer.Start();
                    MainViewModel.NowPlaying.PauseButton = PackIconKind.Pause;
                    break;
                case SongStatus.Paused:
                // Fall through
                case SongStatus.Stopped:
                    SliderTimer.Stop();
                    MainViewModel.NowPlaying.PauseButton = PackIconKind.Play;
                    break;
            }
        }

        public static void Stop() {
            SliderTimer.Stop();
            MainViewModel.Status = SongStatus.Stopped;
            MainViewModel.NowPlaying.PauseButton = PackIconKind.Pause;
            MusicSlider.Value = 0;
            MainViewModel.Player.Stop();
        }

        public static void SaveVolume() {
            VolumeValue = (int)VolumeSlider.Value;
        }
    }
}
