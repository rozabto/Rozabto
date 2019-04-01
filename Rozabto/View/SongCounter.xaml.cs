using Rozabto.Model.Data;
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

namespace Rozabto.View {
    public partial class SongCounter : UserControl {
        private readonly string[] paths;
        private int songCount;
        public SongCounter(string[] paths) {
            InitializeComponent();
            this.paths = paths;
            songCount = 0;
            Max.Content = paths.Length;
            Counter.Content = songCount;
        }

        private async void CounterLoaded(object sender, RoutedEventArgs e) {
            var factory = new MusicInformation();
            for (int i = 0; i < paths.Length; i++) {
                await SongCompleted(paths[i]);
                await factory.SearchMusic(paths[i]);
            }
            ViewModel.MainViewModel.AddSongs();
            ((MainWindow)Application.Current.MainWindow).HideCounter();
        }

        private Task SongCompleted(string songName) {
            Loading.Content = System.IO.Path.GetFileNameWithoutExtension(songName);
            Counter.Content = ++songCount;
            return Task.CompletedTask;
        }
    }
}
