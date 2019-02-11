using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.ViewModel.Notify {
    public class MySongsNotify : INotifyPropertyChanged {
        public virtual Collection Collection { get; }

        public MySongsNotify(Collection collection) {
            Collection = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
