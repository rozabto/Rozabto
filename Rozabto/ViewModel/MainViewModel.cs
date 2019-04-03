using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    /// <summary>
    /// Прави връзка между View и Model.
    /// </summary>
    public static class MainViewModel {
        public static Collection Collection { get; private set; }
        public static MediaPlayer Player { get; }
        public static MySongsNotify MySongs { get; private set; }
        public static NowPlayingNotify NowPlaying { get; private set; }
        public static PlayListsNotify PlayList { get; private set; }
        public static ABPNotify ABP { get; private set; }
        public static SongStatus Status { get; set; }
        public static VolumeState Volume { get; set; }
        public static bool Theme { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
            SetCollection();
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
        public static void AddSongsToPlayList(string name, Song[] songs) {
            var context = new BlogDBContext();
            var playListEF = context.PlayLists.FirstOrDefault(f => f.Name == name);
            if (playListEF is null)
                return;
            var addSongs = songs.Select(s => new PlayListSongsEF {
                PlayListID = playListEF.ID,
                SongID = s.ID
            });
            addSongs = addSongs.Where(w => context.PlayListsSongs.FirstOrDefault(f => f.PlayListID == playListEF.ID && f.SongID == w.SongID) is null);
            context.PlayListsSongs.AddRange(addSongs);
            context.SaveChanges();
            var playList = Collection.PlayLists.FirstOrDefault(f => f.Name == name);
            playList.Songs.AddRange(songs.Where(w => playList.Songs.FirstOrDefault(f => f.ID == w.ID) is null));
            PlayList.OnPropertyChanged("PlayList");
        }


        public static Song[] GetSongsFromBand(string name) {
            var context = new BlogDBContext();
            var band = context.Bands.FirstOrDefault(f => f.Name == name);
            if (band is null)
                return null;
            var bandSongs = context.BandsSongs.Where(w => w.BandID == band.ID);
            return context.Songs.Where(w => bandSongs.FirstOrDefault(f => f.SongID == w.ID) != null).ToArray();
        }

        public static Song[] GetSongsFromAlbum(string name) {
            var context = new BlogDBContext();
            var album = context.Albums.FirstOrDefault(f => f.Name == name);
            if (album is null)
                return null;
            var albumSongs = context.AlbumsSongs.Where(w => w.AlbumID == album.ID);
            return context.Songs.Where(w => albumSongs.FirstOrDefault(f => f.SongID == w.ID) != null).ToArray();
        }

        public static Song[] GetSongFromName(string name) {
            var context = new BlogDBContext();
            var song = context.Songs.FirstOrDefault(f => f.Name == name);
            return song is null ? null : (new[] { song });
        }

        public static void RemoveAlbum(string name) {
            var context = new BlogDBContext();
            var album = context.Albums.FirstOrDefault(f => f.Name == name);
            if (album is null)
                return;
            var albumSongs = context.AlbumsSongs.ToArray().Where(w => w.AlbumID == album.ID).ToArray();
            var songs = context.Songs.ToArray().Where(w => albumSongs.FirstOrDefault(f => f.SongID == w.ID) != null);
            var bandSongs = context.BandsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            var playListSongs = context.PlayListsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            context.BandsSongs.RemoveRange(bandSongs);
            context.AlbumsSongs.RemoveRange(albumSongs);
            context.PlayListsSongs.RemoveRange(playListSongs);
            context.SaveChanges();
            var albumsSongs = context.AlbumsSongs.ToArray();
            var bandsSongs = context.BandsSongs.ToArray();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var bands = context.Bands.ToArray().Where(f => bandsSongs.FirstOrDefault(c => c.BandID == f.ID) == null);
            var playLists = context.PlayLists.ToArray().Where(f => playListsSongs.FirstOrDefault(c => c.PlayListID == f.ID) == null);
            var albums = context.Albums.ToArray().Where(f => albumsSongs.FirstOrDefault(c => c.AlbumID == f.ID) == null);
            context.Albums.RemoveRange(albums);
            if (bands.Count() != 0)
                context.Bands.RemoveRange(bands);
            if (playLists.Count() != 0)
                context.PlayLists.RemoveRange(playLists);
            context.Songs.RemoveRange(songs);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
        }

        public static void RemoveBand(string bandName) {
            var context = new BlogDBContext();
            var band = context.Bands.FirstOrDefault(f => f.Name == bandName);
            if (band is null)
                return;
            var bandSongs = context.BandsSongs.ToArray().Where(w => w.BandID == band.ID).ToArray();
            var songs = context.Songs.ToArray().Where(w => bandSongs.FirstOrDefault(f => f.SongID == w.ID) != null);
            var albumSongs = context.AlbumsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            var playListSongs = context.PlayListsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            context.BandsSongs.RemoveRange(bandSongs);
            context.AlbumsSongs.RemoveRange(albumSongs);
            context.PlayListsSongs.RemoveRange(playListSongs);
            context.SaveChanges();
            var albumsSongs = context.AlbumsSongs.ToArray();
            var bandsSongs = context.BandsSongs.ToArray();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var albums = context.Albums.ToArray().Where(f => albumsSongs.FirstOrDefault(c => c.AlbumID == f.ID) == null);
            var playLists = context.PlayLists.ToArray().Where(f => playListsSongs.FirstOrDefault(c => c.PlayListID == f.ID) == null);
            var bands = context.Bands.ToArray().Where(f => bandsSongs.FirstOrDefault(c => c.BandID == f.ID) == null);
            if (albums.Count() != 0)
                context.Albums.RemoveRange(albums);
            context.Bands.RemoveRange(bands);
            if (playLists.Count() != 0)
                context.PlayLists.RemoveRange(playLists);
            context.Songs.RemoveRange(songs);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
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

        public static void RemoveSong(string songName) {
            var context = new BlogDBContext();
            var song = context.Songs.FirstOrDefault(f => f.Name == songName);
            if (song is null) return;
            var albumSong = context.AlbumsSongs.FirstOrDefault(f => f.SongID == song.ID);
            var bandSong = context.BandsSongs.FirstOrDefault(f => f.SongID == song.ID);
            var playListSong = context.PlayListsSongs.FirstOrDefault(f => f.SongID == song.ID);
            context.AlbumsSongs.Remove(albumSong);
            context.BandsSongs.Remove(bandSong);
            if (playListSong != null)
                context.PlayListsSongs.Remove(playListSong);
            context.SaveChanges();
            var albumsSongs = context.AlbumsSongs.ToArray();
            var bandsSongs = context.BandsSongs.ToArray();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var album = context.Albums.ToArray().FirstOrDefault(f => albumsSongs.FirstOrDefault(c => c.AlbumID == f.ID) == null);
            var band = context.Bands.ToArray().FirstOrDefault(f => bandsSongs.FirstOrDefault(c => c.BandID == f.ID) == null);
            var playList = context.PlayLists.ToArray().FirstOrDefault(f => playListsSongs.FirstOrDefault(c => c.PlayListID == f.ID) == null);
            if (album != null)
                context.Albums.Remove(album);
            if (band != null)
                context.Bands.Remove(band);
            if (playList != null)
                context.PlayLists.Remove(playList);
            context.Songs.Remove(song);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
        }

        /// <summary>
        /// Обновява базата данни.
        /// </summary>
        public static void RefreshDataBase() {
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
        private static void SetCollection() {
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
            if (Collection is null)
                Collection = new Collection(albums, bands, playLists, songs);
            else {
                if (Collection.Albums.Count > albums.Count)
                    Collection.Albums.RemoveAll(r => albums.FirstOrDefault(f => f.Name == r.Name) is null);
                else if (Collection.Albums.Count < albums.Count)
                    Collection.Albums.AddRange(albums.Where(w => !Collection.Albums.Contains(w)));
                else
                    for (int i = 0; i < albums.Count; i++)
                        if (Collection.Albums[i].Songs.Count > albums[i].Songs.Count)
                            Collection.Albums[i].Songs.RemoveAll(r => !albums[i].Songs.Contains(r));
                        else if (Collection.Albums[i].Songs.Count < albums[i].Songs.Count)
                            Collection.Albums[i].Songs.AddRange(albums[i].Songs.Where(w => !Collection.Albums[i].Songs.Contains(w)));
                if (Collection.Bands.Count > bands.Count)
                    Collection.Bands.RemoveAll(r => bands.FirstOrDefault(f => f.Name == r.Name) is null);
                else if (Collection.Bands.Count < bands.Count)
                    Collection.Bands.AddRange(bands.Where(w => !Collection.Bands.Contains(w)));
                else
                    for (int i = 0; i < bands.Count; i++)
                        if (Collection.Bands[i].Songs.Count > bands[i].Songs.Count)
                            Collection.Bands[i].Songs.RemoveAll(r => !bands[i].Songs.Contains(r));
                        else if (Collection.Bands[i].Songs.Count < bands[i].Songs.Count)
                            Collection.Bands[i].Songs.AddRange(bands[i].Songs.Where(w => !Collection.Bands[i].Songs.Contains(w)));
                if (Collection.PlayLists.Count > playLists.Count)
                    Collection.PlayLists.RemoveAll(r => playLists.FirstOrDefault(f => f.Name == r.Name) is null);
                else if (Collection.PlayLists.Count < playLists.Count)
                    Collection.PlayLists.AddRange(playLists.Where(w => !Collection.PlayLists.Contains(w)));
                else
                    for (int i = 0; i < playLists.Count; i++)
                        if (Collection.PlayLists[i].Songs.Count > playLists[i].Songs.Count)
                            Collection.PlayLists[i].Songs.RemoveAll(r => !playLists[i].Songs.Contains(r));
                        else if (Collection.PlayLists[i].Songs.Count < playLists[i].Songs.Count)
                            Collection.PlayLists[i].Songs.AddRange(playLists[i].Songs.Where(w => !Collection.PlayLists[i].Songs.Contains(w)));
                if (Collection.Songs.Count > songs.Count)
                    Collection.Songs.RemoveAll(r => songs.FirstOrDefault(f => f.ID == r.ID) is null);
                else if (Collection.Songs.Count < songs.Count)
                    Collection.Songs.AddRange(songs.Where(w => !Collection.Songs.Contains(w)));
            }
        }
    }
}
