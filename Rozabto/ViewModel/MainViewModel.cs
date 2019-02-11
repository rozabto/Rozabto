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
        private static readonly MySongsNotify _mysongs;
        private static readonly NowPlayingNotify _nowplaying;
        private static readonly PlayListsNotify _playlists;
        private static readonly SettingsNotify _settings;
        private static readonly Collection _collection;

        static MainViewModel() {
            _player = new MediaPlayer();
            _collection = new Collection();
            //add logic
            _settings = new SettingsNotify();
            _mysongs = new MySongsNotify(_collection);
            _nowplaying = new NowPlayingNotify();
            _playlists = new PlayListsNotify();
        }

        public static void AddSongs(string[] songs) {
            MusicInformation.SearchMusic(songs, _collection);
            
        }
    }
}
