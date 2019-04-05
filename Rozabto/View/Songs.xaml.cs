using MaterialDesignThemes.Wpf;
using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View
{
    public partial class Songs : UserControl
    {
        private string SelectedSongName;
        public Songs()
        {
            InitializeComponent();
            DataContext = MainViewModel.MySongs;
        }

        public void AddToPlayList(object sender, RoutedEventArgs e)
        {
            var add = new AddToPlayList(MainViewModel.GetSongFromName(SelectedSongName))
            {
                Owner = Application.Current.MainWindow
            };
            add.Show();
        }

        public void RemoveSong(object sender, RoutedEventArgs e)
        {
            MainViewModel.RemoveSong(SelectedSongName);
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            SelectedSongName = (((sender as PopupBox).Parent as StackPanel).Children[0] as Label).Content.ToString();
        }
    }
}
