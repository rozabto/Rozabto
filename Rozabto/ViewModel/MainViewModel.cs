using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    public static class MainViewModel {
        public static Collection Collection { get; private set; }
        public static MediaPlayer Player { get; }
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static ABPNotify ABP { get; private set; }
        public static SettingsNotify Settings { get; }
        public static SongStatus Status { get; set; }
        public static VolumeState Volume { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
            SetCollection();
            Settings = new SettingsNotify();
            MySongs = new MySongsNotify(Collection);
            NowPlaying = new NowPlayingNotify(Collection);
            PlayList = new PlayListsNotify(Collection);
            Volume = VolumeState.On;
        }

        public static void ActivateABP(object type) {
            if (type.GetType() == typeof(Band)) {
                var band = type as Band;
                ABP = new ABPNotify(band.Songs, band.Name);
            }
            else if (type.GetType() == typeof(Album)) {
                var album = type as Album;
                ABP = new ABPNotify(album.Songs, album.Name);
            }
            else if (type.GetType() == typeof(PlayList)) {
                var playlist = type as PlayList;
                ABP = new ABPNotify(playlist.Songs, playlist.Name);
            }
        }

        public static void AddPlayList(string name) {
            var context = new BlogDBContext();
            var playlist = new PlayList(name);
            Collection.PlayLists.Add(playlist);
            context.PlayLists.Add(new PlayListEF { Name = name });
            context.SaveChanges();
            PlayList.OnPropertyChanged("PlayList");
        }

        public static async Task AddSongs(string[] songs) {
            await MusicInformation.SearchMusic(songs);
            SetCollection();
            NowPlaying.OnPropertyChanged("Songs");
            MySongs.OnPropertyChanged("Bands");
            MySongs.OnPropertyChanged("Albums");
            MySongs.OnPropertyChanged("Songs");
        }

        public static void Play() {
            switch (Status) {
                case SongStatus.Stopped:
                    Player.Close();
                    if (NowPlaying.CurrentSong != Song.EmptySong) {
                        Player.Open(new Uri(NowPlaying.CurrentSong.Location, UriKind.RelativeOrAbsolute));
                        Player.Play();
                        Status = SongStatus.Playing;
                    }
                    break;
                case SongStatus.Playing:
                    Player.Pause();
                    Status = SongStatus.Paused;
                    break;
                case SongStatus.Paused:
                    Player.Play();
                    Status = SongStatus.Playing;
                    break;
            }
        }

        static void SetCollection() {
            var context = new BlogDBContext();
            var songs = context.Songs.ToList();
            var bandsSongs = context.BandsSongs.ToArray();
            var bands = context.Bands.ToArray().Select(s => new Band(s.Name,
                songs.Where(w => bandsSongs.Where(wh => wh.BandID == s.ID)
                .FirstOrDefault(f => f.SongID == w.ID) != null).ToList())).ToList();
            var albumsSongs = context.AlbumsSongs.ToArray();
            var albums = context.Albums.ToArray().Select(s => new Album(s.Name,
                songs.Where(w => albumsSongs.Where(wh => wh.AlbumID == s.ID)
                .FirstOrDefault(f => f.SongID == w.ID) != null).ToList())).ToList();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var playLists = context.PlayLists.ToArray().Select(s => new PlayList(s.Name,
                songs.Where(w => playListsSongs.Where(wh => wh.PlayListID == s.ID)
                .FirstOrDefault(f => f.SongID == w.ID) != null).ToList())).ToList();
            Collection = new Collection(albums, bands, playLists, songs);
        }
    }
}
