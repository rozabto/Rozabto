using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    public static class MainViewModel {
        private static readonly Collection _collection;

        public static MediaPlayer Player { get; }
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static SettingsNotify Settings { get; }
        public static SongStatus Status { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
            _collection = new Collection();
            var songs = Json.Read<List<Song>>("Songs");
            var albums = Json.Read<List<Tuple<int[], string>>>("Albums");
            var bands = Json.Read<List<Tuple<int[], string>>>("Bands");
            var playlists = Json.Read<List<Tuple<int[], string>>>("PlayLists");
            //add songs
            if (songs != null) {
                _collection.Songs = songs;
                //add albums
                if (albums != null)
                    _collection.Albums = albums.Select(s => new Album {
                        Name = s.Item2,
                        Songs = songs.Where(r => s.Item1.Contains(r.ID)).ToList()
                    }).ToList();
                //add bands
                if (bands != null)
                    _collection.Bands = bands.Select(s => new Band {
                        Name = s.Item2,
                        Songs = songs.Where(r => s.Item1.Contains(r.ID)).ToList()
                    }).ToList();
                //add playlists
                if (playlists != null)
                    _collection.Playlists = playlists.Select(s => new Playlist {
                        Name = s.Item2,
                        Songs = songs.Where(r => s.Item1.Contains(r.ID)).ToList()
                    }).ToList();
            }
            //instantiate
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
            MySongs.OnPropertyChanged("Songs");
        }

        public static void SaveCollection() {
            Json.Write(_collection.Songs, "Songs");
            Json.Write(_collection.Albums.Select(s => new Tuple<IEnumerable<int>, string>(
                s.Songs.Select(r => r.ID), s.Name)), "Albums");
            Json.Write(_collection.Bands.Select(s => new Tuple<IEnumerable<int>, string>(
                s.Songs.Select(r => r.ID), s.Name)), "Bands");
            Json.Write(_collection.Playlists.Select(s => new Tuple<IEnumerable<int>, string>(
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
