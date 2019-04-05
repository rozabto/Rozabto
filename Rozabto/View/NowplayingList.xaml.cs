using MaterialDesignThemes.Wpf;
using Rozabto.Model;
using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rozabto.View
{
    public partial class NowplayingList : UserControl
    {
        private string SelectedSongName;
        public NowplayingList()
        {
            InitializeComponent();
            DataContext = MainViewModel.NowPlaying;
        }
        public void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Пускаме песента, избрана от листа.
            var playListBox = sender as ListBox;
            var song = playListBox.SelectedItem as Song;
            playListBox.UpdateLayout();
            MediaViewModel.Stop();
            MainViewModel.NowPlaying.CurrentSong = song;
            MainViewModel.NowPlaying.CurrentSongPos = playListBox.SelectedIndex;
            MediaViewModel.TimerPlay();
        }

        public void FocusOnSelectedItem(object sender, SelectionChangedEventArgs e)
        {
            SongsList.ScrollIntoView(SongsList.SelectedItem);
        }

        public void AddToPlayList(object sender, RoutedEventArgs e)
        {
            var add = new AddToPlayList(MainViewModel.GetSongFromName(SelectedSongName))
            {
                Owner = (MainWindow)Application.Current?.MainWindow
            };
            add.Show();
        }

        public void RemoveSong(object sender, RoutedEventArgs e)
        {
            MainViewModel.RemoveSong(SelectedSongName);
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e) 
        {
            SelectedSongName = (((sender as PopupBox).Parent as StackPanel).Children[1] as TextBlock).Text;
        }
    }
}
