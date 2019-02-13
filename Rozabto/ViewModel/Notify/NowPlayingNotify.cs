using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.ViewModel.Notify {
    public class NowPlayingNotify : INotifyPropertyChanged {
        public virtual Collection Collection { get; }
        public ObservableCollection<Song> Songs => new ObservableCollection<Song>(Collection.Songs);
        public Song CurrentSong { get; set; }

        public NowPlayingNotify(Collection collection) {
            Collection = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
