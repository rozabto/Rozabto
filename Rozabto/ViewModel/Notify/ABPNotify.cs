using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.ViewModel.Notify {
    public class ABPNotify : INotifyPropertyChanged {
        public string Name { get; }
        public ObservableCollection<Song> Songs { get; }
        public int SongsCount => Songs.Count;
        public TimeSpan TotalTime => new TimeSpan(Songs.Sum(s => s.Duration.Ticks));

        public ABPNotify(List<Song> songs, string name) {
            Songs = new ObservableCollection<Song>(songs);
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
