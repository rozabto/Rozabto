﻿using MaterialDesignThemes.Wpf;
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
        public Nowplaying() {
            InitializeComponent();
            DataContext = MainViewModel.NowPlaying;
            MusicSlider.Maximum = !MainViewModel.Player.NaturalDuration.HasTimeSpan ? 1d :
                MainViewModel.Player.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Minimum = 0;
            MediaViewModel.ConnectViewToViewModel(MusicSlider, VolumeSlider, SongTime);
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
            if (MainViewModel.Volume == VolumeState.Mute) return;
            MainViewModel.Player.Volume = Math.Round(Math.Pow(VolumeSlider.Value / 100d, 1.150515), 3);
            VolumeLabel.Changed(VolumeGrid, VolumeSlider.Value);
            SetVolumeToPlayer();
        }

        private void SetVolumeToPlayer() {
            var volume = MainViewModel.Player.Volume;
            if (volume == 0 && MainViewModel.Volume != VolumeState.Zero) {
                MainViewModel.Volume = VolumeState.Zero;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeMute;
            }
            else if (volume > 0 && volume <= 0.298 && MainViewModel.Volume != VolumeState.Low) {
                MainViewModel.Volume = VolumeState.Low;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeLow;
            }
            else if (volume > 0.298 && volume <= 0.663 && MainViewModel.Volume != VolumeState.Medium) {
                MainViewModel.Volume = VolumeState.Medium;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeMedium;
            }
            else if (volume > 0.663 && MainViewModel.Volume != VolumeState.High) {
                MainViewModel.Volume = VolumeState.High;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeHigh;
            }
        }

        private void PlayPause(object sender, RoutedEventArgs e) {
            MediaViewModel.Play();
        }

        private void MusicSlider_DragCompleted(object sender, DragCompletedEventArgs e) {
            MediaViewModel.SliderDragging = false;
            MainViewModel.Player.Position = TimeSpan.FromSeconds(MusicSlider.Value);
            if (MainViewModel.Status == SongStatus.Paused)
                MediaViewModel.Play();
        }

        private void MusicSlider_DragStarted(object sender, DragStartedEventArgs e) {
            MediaViewModel.SliderDragging = true;
            if (MainViewModel.Status == SongStatus.Playing)
                MediaViewModel.Play(); // Pause
        }

        private void MoveBackSong(object sender, RoutedEventArgs e) {
            if (MainViewModel.NowPlaying.Songs.Count <= 1 || --MainViewModel.NowPlaying.CurrentSongPos == -1) {
                MediaViewModel.Stop();
                return;
            }
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[MainViewModel.NowPlaying.CurrentSongPos];
            MediaViewModel.Stop();
            MediaViewModel.Play();
        }

        private void MoveSongForward(object sender, RoutedEventArgs e) {
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

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var playListBox = sender as ListBox;
            var song = playListBox.SelectedItem as Song;
            playListBox.UpdateLayout();
            MediaViewModel.Stop();
            MainViewModel.NowPlaying.CurrentSong = song;
            MainViewModel.NowPlaying.CurrentSongPos = playListBox.SelectedIndex;
            MediaViewModel.Play();
        }

        private void Randomizer(object sender, RoutedEventArgs e) {
            MainViewModel.NowPlaying.ShuffleSongs = !MainViewModel.NowPlaying.ShuffleSongs;
        }

        private void Repeater(object sender, RoutedEventArgs e) {
            MainViewModel.NowPlaying.RepeatSong = !MainViewModel.NowPlaying.RepeatSong;
        }

        private void SaveVolumeSliderValue(object sender, RoutedEventArgs e) {
            MediaViewModel.SaveVolume();
        }

        private void ShowVolumeNumber(object sender, MouseButtonEventArgs e) {
            VolumeLabel.Show(VolumeGrid, VolumeSlider.Value);
        }

        private void HideVolumeNumber(object sender, MouseButtonEventArgs e) {
            VolumeLabel.Hide(VolumeGrid);
        }

        private void MuteButton(object sender, RoutedEventArgs e) {
            if (MainViewModel.Volume != VolumeState.Mute) {
                MainViewModel.Player.Volume = 0;
                MainViewModel.Volume = VolumeState.Mute;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeMute;
            }
            else {
                MainViewModel.Player.Volume = Math.Round(Math.Pow(VolumeSlider.Value / 100d, 1.150515), 3);
                MainViewModel.Volume = VolumeState.On;
                SetVolumeToPlayer();
            }
        }
    }
}
