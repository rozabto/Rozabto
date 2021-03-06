﻿using MaterialDesignThemes.Wpf;
using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View
{
    public partial class Bands : UserControl 
    {
        public string SelectedBandName { get; set; }
        public Bands()
        {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }

        public void SelectBand(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            // Взимаме grid който показва страницата от MainWindow.
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            // Активираме ABP с избраната банда от листа.
            MainViewModel.ActivateABP(MainViewModel.Collection.Bands[listbox.SelectedIndex]);
            grid.Children.Add(new ABPContent());
        }

        public void AddToPlayList(object sender, RoutedEventArgs e)
        {
            var add = new AddToPlayList(MainViewModel.GetSongsFromBand(SelectedBandName))
            {
                Owner = Application.Current.MainWindow
            };
            add.Show();
        }

        public void RemoveBand(object sender, RoutedEventArgs e)
        {
            MainViewModel.RemoveBand(SelectedBandName);
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e) 
        {
            SelectedBandName = (((sender as PopupBox).Parent as DockPanel).Children[1] as Label).Content.ToString();
        }
    }
}
