using Rozabto.Model.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rozabto.View
{
    public partial class SongCounter : UserControl
    {
        private bool running;
        private readonly string[] paths;
        private int songCount;
        public SongCounter(string[] paths)
        {
            InitializeComponent();
            this.paths = paths;
            songCount = 0;
            running = true;
            Max.Content = paths.Length;
            Counter.Content = songCount;
        }

        public async void CounterLoaded(object sender, RoutedEventArgs e)
        {
            var factory = new MusicInformation();
            for (int i = 0; i < paths.Length && running; i++)
            {
                await SongCompleted(paths[i]);
                await factory.SearchMusic(paths[i]);
                await Task.Delay(0);
            }
            ViewModel.MainViewModel.SetCollection();
            ViewModel.MainViewModel.RefreshDataBase();
            ((MainWindow)Application.Current.MainWindow).HideCounter();
        }

        public Task SongCompleted(string songName)
        {
            Loading.Content = System.IO.Path.GetFileNameWithoutExtension(songName);
            Counter.Content = ++songCount;
            return Task.CompletedTask;
        }

        private void StopLoading(object sender, RoutedEventArgs e)
        {
            running = false;
        }
    }
}
