using MaterialDesignThemes.Wpf;
using Rozabto.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View
{
    public partial class ABPContent : UserControl
    {
        private string SelectedSongName;
        public ABPContent()
        {
            InitializeComponent();
            DataContext = MainViewModel.ABP;
        }

        private void RemoveSong(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.ABP.IsPlayList)
                MainViewModel.RemoveSongFromPlayList(SelectedSongName);
            else MainViewModel.RemoveSong(SelectedSongName, true);
        }

        private void PopupBox_Opened(object sender, RoutedEventArgs e)
        {
            SelectedSongName = (((sender as PopupBox).Parent as StackPanel).Children[0] as Label).Content.ToString();
        }
    }
}
