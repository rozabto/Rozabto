using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rozabto.View
{
    public partial class Songs : UserControl
    {
        private bool isOpened;
        private string SelectedSongName;
        public Songs()
        {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }

        public void SelectedSong(object sender, MouseEventArgs e)
        {
            if (!isOpened)
                SelectedSongName = ((sender as StackPanel).Children[0] as Label).Content.ToString();
        }

        public void AddToPlayList(object sender, RoutedEventArgs e)
        {
            var add = new AddToPlayList(MainViewModel.GetSongFromName(SelectedSongName))
            {
                Owner = (MainWindow)Application.Current.MainWindow
            };
            add.Show();
            isOpened = false;
        }

        public void RemoveSong(object sender, RoutedEventArgs e)
        {
            MainViewModel.RemoveSong(SelectedSongName);
            isOpened = false;
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            isOpened = true;
        }
    }
}
