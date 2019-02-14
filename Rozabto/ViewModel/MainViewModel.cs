using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    public static class MainViewModel {
        private static readonly MediaPlayer _player;
        private static readonly Collection _collection;
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static SettingsNotify Settings { get; }

        static MainViewModel() {
            _player = new MediaPlayer();
            _collection = new Collection();
            //add logic
            Settings = new SettingsNotify();
            MySongs = new MySongsNotify(_collection);
            NowPlaying = new NowPlayingNotify(_collection);
            PlayList = new PlayListsNotify();
        }

        public static void AddSongs(string[] songs) {
            MusicInformation.SearchMusic(songs, _collection);
            NowPlaying.OnPropertyChanged("Songs");
        }
    }
}
