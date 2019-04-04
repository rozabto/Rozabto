using Microsoft.Win32;
using Rozabto.View;
using System.Windows;
using System.Windows.Input;

namespace Rozabto
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GridPrincipal.Children.Add(new Nowplaying());
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GetNewSongs(object sender, RoutedEventArgs e)
        {
            // Отваряме диалог, в който могат да се изберат нови песни.
            var fileDialog = new OpenFileDialog
            {
                Filter = "music|*.mp3;*.aac;*.wma;*.wav;*.aiff;*.m4a",
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Select Music Files"
            };
            var result = fileDialog.ShowDialog();
            // Ако са избрани песни или диалогът е затворен даваме на Model да ги прочете. 
            if (result == true && string.IsNullOrWhiteSpace(fileDialog.FileName) || result == false)
                return;
            SongsLoading.Height = new GridLength(30);
            GridSongsLoading.Children.Add(new SongCounter(fileDialog.FileNames));
        }

        public void HideCounter()
        {
            SongsLoading.Height = new GridLength(0);
            GridSongsLoading.Children.Clear();
        }

        private void ChangeWindowPosition(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ShowMyMusic(object sender, RoutedEventArgs e)
        {
            // Ако се преместим от NowPlaying показваме един малък вариант на плеъра.
            if (Playing.Children.Count == 0)
            {
                Playing.Children.Add(new MiniNowplaying());
                RowDef.Height = new GridLength(50);
            }
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new MyMusic());
        }

        private void ShowPlayList(object sender, RoutedEventArgs e)
        {
            // Ако се преместим от NowPlaying показваме един малък вариант на плеъра.
            if (Playing.Children.Count == 0)
            {
                Playing.Children.Add(new MiniNowplaying());
                RowDef.Height = new GridLength(50);
            }
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Playlists());
        }

        private void ShowNowPlaying(object sender, RoutedEventArgs e)
        {
            // Ако малкия плеър е показан го скриваме.
            Playing.Children.Clear();
            RowDef.Height = new GridLength(0);
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Nowplaying());
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            // Ако се преместим от NowPlaying показваме един малък вариант на плеъра.
            if (Playing.Children.Count == 0)
            {
                Playing.Children.Add(new MiniNowplaying());
                RowDef.Height = new GridLength(50);
            }
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Settings());
        }
    }
}
