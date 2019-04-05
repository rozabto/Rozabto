using MaterialDesignThemes.Wpf;
using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View 
{
    public partial class Album : UserControl 
    {
        public string SelectedAlbumName { get; set; }
        public Album() 
        {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }
         
        public void SelectAlbum(object sender, SelectionChangedEventArgs e) 
        {
            var listbox = sender as ListBox;
            // Взимаме grid който показва страницата от MainWindow.
            var grid = ((MainWindow)Application.Current.MainWindow).GridPrincipal;
            grid.Children.Clear();
            // Активираме ABP с избрания албум от листа.
            MainViewModel.ActivateABP(MainViewModel.Collection.Albums[listbox.SelectedIndex]);
            grid.Children.Add(new ABPContent());
        }

        public void AddToPlayList(object sender, RoutedEventArgs e) 
        {
            var add = new AddToPlayList(MainViewModel.GetSongsFromAlbum(SelectedAlbumName)) 
            {
                Owner = Application.Current.MainWindow
            };
            add.Show();
        }

        public void RemoveAlbum(object sender, RoutedEventArgs e) 
        {
            MainViewModel.RemoveAlbum(SelectedAlbumName);
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e) 
        {
            SelectedAlbumName = (((sender as PopupBox).Parent as DockPanel).Children[1] as Label).Content.ToString();
        }
    }
}
