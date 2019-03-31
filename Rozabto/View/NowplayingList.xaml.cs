using Rozabto.Model;
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

namespace Rozabto.View
{
    /// <summary>
    /// Interaction logic for NowplayingList.xaml
    /// </summary>
    public partial class NowplayingList : UserControl
    {
        public NowplayingList()
        {
            InitializeComponent();
            DataContext = MainViewModel.NowPlaying;
        }
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Пускаме песента, избрана от листа.
            var playListBox = sender as ListBox;
            var song = playListBox.SelectedItem as Song;
            playListBox.UpdateLayout();
            MediaViewModel.Stop();
            MainViewModel.NowPlaying.CurrentSong = song;
            MainViewModel.NowPlaying.CurrentSongPos = playListBox.SelectedIndex;
            MediaViewModel.Play();
        }

    }
}
