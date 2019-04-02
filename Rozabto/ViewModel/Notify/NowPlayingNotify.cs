using MaterialDesignThemes.Wpf;
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

        public string SongBand => Collection.Bands.FirstOrDefault(f => f.Songs.Contains(_currentSong))?.Name;

        private Song _currentSong;

        public Song CurrentSong {
            get => _currentSong;
            set {
                _currentSong = value;
                OnPropertyChanged("CurrentSong");
            }
        }

        private PackIconKind _pauseButton;

        public PackIconKind PauseButton {
            get => _pauseButton;
            set {
                _pauseButton = value;
                OnPropertyChanged("PauseButton");
            }
        }

        private PackIconKind _muteButton;

        public PackIconKind MuteButton {
            get => _muteButton;
            set {
                _muteButton = value;
                OnPropertyChanged("MuteButton");
            }
        }

        public bool RepeatSong { get; set; }
        public bool ShuffleSongs { get; set; }
        public int CurrentSongPos { get; set; }

        public NowPlayingNotify(Collection collection) {
            Collection = collection;
            PauseButton = PackIconKind.Pause;
            MuteButton = PackIconKind.VolumeHigh;
            CurrentSong = Song.EmptySong;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
