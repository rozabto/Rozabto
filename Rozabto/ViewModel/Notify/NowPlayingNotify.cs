using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.ViewModel.Notify {
    public class NowPlayingNotify : INotifyPropertyChanged {
        public virtual List<Song> Songs { get; }
        public Song CurrentSong { get; set; }

        public NowPlayingNotify(List<Song> songs) {
            Songs = songs;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
