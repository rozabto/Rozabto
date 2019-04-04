using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rozabto.View
{
    public partial class Playlists : UserControl
    {
        public string SelectedPlayListName { get; set; }
        public Playlists()
        {
            InitializeComponent();
            DataContext = MainViewModel.PlayList;
        }

        public void AddPlayList(object sender, RoutedEventArgs e)
        {
            // Добавяме нов плейлист с името, което сме дали.
            var text = Playlist.Text;
            if (string.IsNullOrWhiteSpace(text) || text.Length < 5 || text.Length > 20)
                return;
            MainViewModel.AddPlayList(text);
            Playlistdialog.IsOpen = false;
        }

        public void SelectPlayList(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            // Взимаме grid който показва страницата от MainWindow.
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            // Активираме ABP с избрания плейлист от листа.
            MainViewModel.ActivateABP(MainViewModel.Collection.PlayLists[listbox.SelectedIndex]);
            grid.Children.Add(new ABPContent());
        }

        public void RemovePlayList(object sender, RoutedEventArgs e)
        {
            MainViewModel.RemovePlayList(SelectedPlayListName);
        }

        public void SelectPlayList(object sender, MouseEventArgs e)
        {
            SelectedPlayListName = ((sender as DockPanel).Children[1] as Label).Content.ToString();
        }
    }
}
