using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.ViewModel.Notify {
    public class MySongsNotify : INotifyPropertyChanged {
        public Collection Collection { get; }
        public ObservableCollection<Band> Bands => new ObservableCollection<Band>(Collection.Bands);
        public ObservableCollection<Album> Albums => new ObservableCollection<Album>(Collection.Albums);
        public ObservableCollection<Song> Songs => new ObservableCollection<Song>(Collection.Songs);

        public MySongsNotify(Collection collection) {
            Collection = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
