using Rozabto.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Rozabto.ViewModel.Notify
{
    public class PlayListsNotify : INotifyPropertyChanged
    {
        public Collection Collection { get; }
        public ObservableCollection<PlayList> PlayList => new ObservableCollection<PlayList>(Collection.PlayLists);

        public PlayListsNotify(Collection collection)
        {
            Collection = collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
