using Rozabto.Model;
using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Rozabto.View {
    public partial class Nowplaying : UserControl {
        private readonly DispatcherTimer SliderTimer;
        private bool SliderDragging;

        public Nowplaying() {
            InitializeComponent();

            MainViewModel.Player.MediaFailed += Player_MediaFailed;
            MainViewModel.Player.MediaOpened += Player_MediaOpened;
            MainViewModel.Player.MediaEnded += Player_MediaEnded;

            VolumeSlider.Value = 50f;
            MainViewModel.Player.Volume = 0.5f;

            SliderTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            SliderTimer.Tick += SliderTimer_Tick;
        }

        private void SliderTimer_Tick(object sender, EventArgs e) {
            if (SliderDragging) return;
            MusicSlider.Value = MainViewModel.Player.Position.TotalSeconds;
            SongTime.Content = MainViewModel.Player.Position.ToString(@"mm\:ss");
        }

        private void Player_MediaEnded(object sender, EventArgs e) {
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

        private void Player_MediaOpened(object sender, EventArgs e) {
            var player = sender as MediaPlayer;
            player.Position = new TimeSpan(0, 0, 0);
            MusicSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Minimum = 0;
            player.Volume = Math.Round(Math.Pow(VolumeSlider.Value / 100d, 1.150515), 3);
        }

        private void Player_MediaFailed(object sender, ExceptionEventArgs e) {
            MessageBox.Show(e.ErrorException.Message);
            Stop();
        }

        public void Play() {
            MainViewModel.Play();
            switch (MainViewModel.Status) {
                case SongStatus.Playing:
                    SliderTimer.Start();
                    MainViewModel.NowPlaying.PauseButton = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                    break;
                case SongStatus.Paused:
                // Fall through
                case SongStatus.Stopped:
                    SliderTimer.Stop();
                    MainViewModel.NowPlaying.PauseButton = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    break;
            }
        }

        public void Stop() {
            SliderTimer.Stop();
            MainViewModel.Status = SongStatus.Stopped;
            MainViewModel.NowPlaying.PauseButton = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            MusicSlider.Value = 0;
            MainViewModel.Player.Stop();
        }

        private Track GetSliderTrack(Slider slider) {
            var track = (Track)slider.Template.FindName("PART_Track", slider);
            return track;
        }

        private void SliderControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var track = GetSliderTrack(MusicSlider);
            double value = track.ValueFromPoint(e.GetPosition(track));
            MusicSlider.Value = value;
            MainViewModel.Player.Position = TimeSpan.FromSeconds(MusicSlider.Value);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            MainViewModel.Player.Volume = Math.Round(Math.Pow(VolumeSlider.Value / 100d, 1.150515), 3);
        }

        private void PlayPause(object sender, RoutedEventArgs e) {
            Play();
        }

        private void MusicSlider_DragCompleted(object sender, DragCompletedEventArgs e) {
            SliderDragging = false;
            MainViewModel.Player.Position = TimeSpan.FromSeconds(MusicSlider.Value);
            if (MainViewModel.Status == SongStatus.Paused)
                Play();
        }

        private void MusicSlider_DragStarted(object sender, DragStartedEventArgs e) {
            SliderDragging = true;
            if (MainViewModel.Status == SongStatus.Playing)
                Play(); // Pause
        }

        private void MoveBackSong(object sender, RoutedEventArgs e) {
            if (MainViewModel.NowPlaying.Songs.Count <= 1 || --MainViewModel.NowPlaying.CurrentSongPos == -1) {
                Stop();
                return;
            }
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[MainViewModel.NowPlaying.CurrentSongPos];
            Stop();
            Play();
        }

        private void MoveSongForward(object sender, RoutedEventArgs e) {
            if (MainViewModel.NowPlaying.Songs.Count <= 1) {
                Stop();
                return;
            }
            var pos = MainViewModel.NowPlaying.ShuffleSongs ? new Random().Next(MainViewModel.NowPlaying.Songs.Count)
                : MainViewModel.NowPlaying.CurrentSongPos + 1;
            if (pos >= MainViewModel.NowPlaying.Songs.Count)
                pos -= MainViewModel.NowPlaying.Songs.Count;
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[pos];
            MainViewModel.NowPlaying.CurrentSongPos = pos;
            Stop();
            Play();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var playListBox = sender as ListBox;
            var song = playListBox.SelectedItem as Song;
            playListBox.UpdateLayout();
            Stop();
            MainViewModel.NowPlaying.CurrentSong = song;
            Play();
        }

        private void Randomizer(object sender, RoutedEventArgs e) {
            MainViewModel.NowPlaying.ShuffleSongs = !MainViewModel.NowPlaying.ShuffleSongs;
        }

        private void Repeater(object sender, RoutedEventArgs e) {
            MainViewModel.NowPlaying.RepeatSong = !MainViewModel.NowPlaying.RepeatSong;
        }
    }
}
