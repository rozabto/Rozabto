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
        private static readonly Collection _collection;

        public static MediaPlayer Player { get; private set; }
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static SettingsNotify Settings { get; }
        public static SongStatus Status { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
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
            MySongs.OnPropertyChanged("Bands");
            MySongs.OnPropertyChanged("Albums");
        }

        public static void Play() {
            if (Status == SongStatus.Stopped) {
                Player.Close();
                if (NowPlaying.CurrentSong != Song.EmptySong) {
                    Player.Open(new Uri(NowPlaying.CurrentSong.Location, UriKind.RelativeOrAbsolute));
                    Player.Play();
                    Status = SongStatus.Playing;
                }
            }
            else if (Status == SongStatus.Playing) {
                Player.Pause();
                Status = SongStatus.Paused;
            }
            else if (Status == SongStatus.Paused) {
                Player.Play();
                Status = SongStatus.Playing;
            }
        }
    }
}
