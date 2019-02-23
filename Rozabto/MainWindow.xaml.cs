﻿using Microsoft.Win32;
using Rozabto.View;
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

namespace Rozabto {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            GridPrincipal.Children.Add(new Nowplaying());
        }

        private void CloseApplication(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void GetNewSongs(object sender, RoutedEventArgs e) {
            var fileDialog = new OpenFileDialog {
                Filter = "mp3|*.mp3",
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Select Music Files"
            };
            var result = fileDialog.ShowDialog();
            if (result == true && string.IsNullOrWhiteSpace(fileDialog.FileName) || result == false)
                return;
            MainViewModel.AddSongs(fileDialog.FileNames);
        }

        private void ChangeWindowPosition(object sender, MouseButtonEventArgs e) {
            DragMove();
        }

        private void Minimize(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void Window_Closed(object sender, EventArgs e) {
            MainViewModel.SaveCollection();
        }

        private void ShowMyMusic(object sender, RoutedEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new MyMusic());
        }

        private void ShowPlayList(object sender, RoutedEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Playlists());
        }

        private void ShowNowPlaying(object sender, RoutedEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Nowplaying());
        }

        private void ShowSettings(object sender, RoutedEventArgs e) {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Settings());
        }
    }
}
