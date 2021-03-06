﻿using MaterialDesignThemes.Wpf;
using Rozabto.Model;
using Rozabto.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Rozabto.View
{
    public partial class Nowplaying : UserControl
    {
        public Nowplaying()
        {
            InitializeComponent();
            SongListBox.Children.Add(new NowplayingList());
            DataContext = MainViewModel.NowPlaying;
            // Слагаме максимума и минимума на плъзгача за времетраенето.
            MusicSlider.Maximum = !MediaViewModel.Player.NaturalDuration.HasTimeSpan ? 1d :
                MediaViewModel.Player.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Minimum = 0;
            // Свързваме NowPlaying с MediaViewModel.
            MediaViewModel.ConnectViewToViewModel(MusicSlider, VolumeSlider, SongTime);
        }

        private Track GetSliderTrack(Slider slider)
        {
            var track = (Track)slider.Template.FindName("PART_Track", slider);
            return track;
        }

        public void SliderControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var track = GetSliderTrack(MusicSlider);
            double value = track.ValueFromPoint(e.GetPosition(track));
            MusicSlider.Value = value;
            MediaViewModel.Player.Position = TimeSpan.FromSeconds(MusicSlider.Value);
        }

        public void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Променяме звука.
            VolumeLabel.Changed(VolumeGrid, VolumeSlider.Value);
            MediaViewModel.SetVolumeToPlayer();
        }

        public void PlayPause(object sender, RoutedEventArgs e)
        {
            MediaViewModel.TimerPlay();
        }

        public void MusicSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            // Сменяме позицията на песента.
            MediaViewModel.SliderDragging = false;
            MediaViewModel.Player.Position = TimeSpan.FromSeconds(MusicSlider.Value);
            if (MediaViewModel.Status == SongStatus.Paused)
                MediaViewModel.TimerPlay();
        }

        public void MusicSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            MediaViewModel.SliderDragging = true;
            if (MediaViewModel.Status == SongStatus.Playing)
                MediaViewModel.TimerPlay(); // Пауза
        }

        public void MoveBackSong(object sender, RoutedEventArgs e)
        {
            // Местим песента с една назад.
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
            // Местим песента с една напред.
            if (MainViewModel.NowPlaying.Songs.Count <= 1)
            {
                MediaViewModel.Stop();
                return;
            }
            // Взимаме нова позиция на песента.
            var pos = MainViewModel.NowPlaying.ShuffleSongs ? new Random().Next(MainViewModel.NowPlaying.Songs.Count)
                : MainViewModel.NowPlaying.CurrentSongPos + 1;
            if (pos >= MainViewModel.NowPlaying.Songs.Count)
                pos -= MainViewModel.NowPlaying.Songs.Count;
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[pos];
            MainViewModel.NowPlaying.CurrentSongPos = pos;
            MediaViewModel.Stop();
            MediaViewModel.TimerPlay();
        }


        public void Randomizer(object sender, RoutedEventArgs e)
        {
            MainViewModel.NowPlaying.ShuffleSongs = !MainViewModel.NowPlaying.ShuffleSongs;
        }

        public void Repeater(object sender, RoutedEventArgs e)
        {
            MainViewModel.NowPlaying.RepeatSong = !MainViewModel.NowPlaying.RepeatSong;
        }

        public void SaveVolumeSliderValue(object sender, RoutedEventArgs e)
        {
            MediaViewModel.SaveVolume();
        }

        public void ShowVolumeNumber(object sender, MouseButtonEventArgs e)
        {
            VolumeLabel.Show(VolumeGrid, VolumeSlider.Value);
            MediaViewModel.Volume = VolumeState.On;
        }

        public void HideVolumeNumber(object sender, MouseButtonEventArgs e)
        {
            VolumeLabel.Hide(VolumeGrid);
        }

        public void MuteButton(object sender, RoutedEventArgs e)
        {
            if (MediaViewModel.Volume != VolumeState.Mute)
            {
                MediaViewModel.Player.Volume = 0;
                MediaViewModel.Volume = VolumeState.Mute;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeMute;
            }
            else
            {
                MediaViewModel.Volume = VolumeState.On;
                MediaViewModel.SetVolumeToPlayer();
            }
        }
    }
}
