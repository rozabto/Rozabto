using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    public static class MainViewModel {
        public static Collection Collection { get; }
        public static MediaPlayer Player { get; }
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static ABPNotify ABP { get; }
        public static SettingsNotify Settings { get; }
        public static SongStatus Status { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
            Collection = new Collection();
            //add songs
            var songs = Json.Read<List<Song>>("Songs");
            if (songs != null) {
                Collection.Songs = songs;
                //add bands
                var bands = Json.Read<List<Band>>("Bands");
                if (bands != null)
                    Collection.Bands = bands;
                var albums = Json.Read<List<Album>>("Albums");
                if (albums != null)
                    Collection.Albums = albums;
                //add play lists
                var playlists = Json.Read<List<Playlist>>("PlayLists");
                if (playlists != null)
                    Collection.Playlists = playlists;
            }
            //instantiate
            Settings = new SettingsNotify();
            MySongs = new MySongsNotify(Collection);
            NowPlaying = new NowPlayingNotify(Collection);
            PlayList = new PlayListsNotify(Collection);
        }

        public static void AddPlayList(string name) {
            Collection.Playlists.Add(new Playlist { Name = name });
            PlayList.OnPropertyChanged("PlayList");
        }

        public static void AddSongs(string[] songs) {
            MusicInformation.SearchMusic(songs, Collection);
            NowPlaying.OnPropertyChanged("Songs");
            MySongs.OnPropertyChanged("Bands");
            MySongs.OnPropertyChanged("Albums");
            MySongs.OnPropertyChanged("Songs");
        }

        public static void SaveCollection() {
            Json.Write(Collection.Songs, "Songs");
            Json.Write(Collection.Albums.Select(s => new Tuple<IEnumerable<int>, string>(
                s.Songs.Select(r => r.ID), s.Name)), "Albums");
            Json.Write(Collection.Bands.Select(s => new Tuple<IEnumerable<int>, string>(
                s.Songs.Select(r => r.ID), s.Name)), "Bands");
            Json.Write(Collection.Playlists.Select(s => new Tuple<IEnumerable<int>, string>(
                s.Songs.Select(r => r.ID), s.Name)), "PlayLists");
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
