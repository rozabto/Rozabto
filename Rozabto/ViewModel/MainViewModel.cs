using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    /// <summary>
    /// Прави връзка между View и Model.
    /// </summary>
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
        public static bool Theme { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
            SetCollection();
            Settings = new SettingsNotify();
            MySongs = new MySongsNotify(Collection);
            NowPlaying = new NowPlayingNotify(Collection);
            PlayList = new PlayListsNotify(Collection);
            Volume = VolumeState.On;
        }

        /// <summary>
        /// Връща лист с песните и информация.
        /// </summary>
        public static void ActivateABP(object type) {
            // type е банда,албум или плейлист.
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

        /// <summary>
        /// Добавя нов плейлист към базата данни.
        /// </summary>
        public static void AddPlayList(string name) {
            var context = new BlogDBContext();
            // Създаваме нов плейлист с името.
            var playlist = new PlayList(name);
            // Добавяме плейлиста към базата данни, която четем в момента.
            Collection.PlayLists.Add(playlist);
            // Добавяме плейлиста към SQL базата данни.
            context.PlayLists.Add(new PlayListEF { Name = name });
            context.SaveChanges();
            // Обновяваме листа с плейлистите.
            PlayList.OnPropertyChanged("PlayList");
        }

        /// <summary>
        /// Добавяме песни към базата данни.
        /// </summary>
        public static void AddSongs() {
            SetCollection();
            // Обновяваме всичко.
            NowPlaying.OnPropertyChanged("Songs");
            MySongs.OnPropertyChanged("Bands");
            MySongs.OnPropertyChanged("Albums");
            MySongs.OnPropertyChanged("Songs");
        }

        /// <summary>
        /// Пуска, прекъсва или спира песните.
        /// </summary>
        public static void Play() {
            switch (Status) {
                case SongStatus.Stopped:
                    // Спираме песента.
                    Player.Close();
                    // Ако песента не е празна, пускаме нова песен.
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

        /// <summary>
        /// Превръща данните от SQL към данни, които плеърът може да чете.
        /// </summary>
        static void SetCollection() {
            var context = new BlogDBContext();
            // Взимаме песните и ги превръщаме в лист.
            var songs = context.Songs.ToList();
            // Взимаме песните на бандите и ги превръщаме в масив.
            var bandsSongs = context.BandsSongs.ToArray();
            // Създаваме лист от всички банди като вземем всички песни от масива от песни.
            var bands = context.Bands.ToArray().Select(s => new Band(s.Name,
                songs.Where(w => bandsSongs.Where(wh => wh.BandID == s.ID)
                .FirstOrDefault(f => f.SongID == w.ID) != null).ToList())).ToList();
            // Взимаме песните от албумите и ги превръщаме в масив.
            var albumsSongs = context.AlbumsSongs.ToArray();
            // Създаваме лист от всички албуми като вземем всички песни от масива от песни.
            var albums = context.Albums.ToArray().Select(s => new Album(s.Name,
                songs.Where(w => albumsSongs.Where(wh => wh.AlbumID == s.ID)
                .FirstOrDefault(f => f.SongID == w.ID) != null).ToList())).ToList();
            // Взимаме песните от плейлистите и ги превръщаме в масив.
            var playListsSongs = context.PlayListsSongs.ToArray();
            // Създаваме лист от всички плейлисти като вземем всички песни от масива от песни.
            var playLists = context.PlayLists.ToArray().Select(s => new PlayList(s.Name,
                songs.Where(w => playListsSongs.Where(wh => wh.PlayListID == s.ID)
                .FirstOrDefault(f => f.SongID == w.ID) != null).ToList())).ToList();
            // Даваме всичко нужно на класа Collection.
            Collection = new Collection(albums, bands, playLists, songs);
        }
    }
}
