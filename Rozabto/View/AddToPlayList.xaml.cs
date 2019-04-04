using Rozabto.Model;
using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Rozabto.View
{
    public partial class AddToPlayList : Window
    {
        private readonly Song[] songs;
        public AddToPlayList(Song[] songs)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            this.songs = songs;
            DataContext = MainViewModel.PlayList;
        }

        public void CancelPlayListCreation(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ChoosePlayList(object sender, MouseButtonEventArgs e)
        {
            MainViewModel.AddSongsToPlayList((PlayListList.SelectedItem as PlayList).Name, songs);
            Close();
        }
    }
}
