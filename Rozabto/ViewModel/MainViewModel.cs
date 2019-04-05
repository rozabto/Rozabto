using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System.Linq;

namespace Rozabto.ViewModel
{
    /// <summary>
    /// Прави връзка между View и Model.
    /// </summary>
    public static class MainViewModel
    {
        public static Collection Collection { get; private set; }
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static ABPNotify ABP { get; private set; }
        public static bool Theme { get; set; }

        static MainViewModel()
        {
            SetCollection();
            MySongs = new MySongsNotify(Collection);
            NowPlaying = new NowPlayingNotify(Collection);
            PlayList = new PlayListsNotify(Collection);
        }

        /// <summary>
        /// Премахва песен от плейлист.
        /// </summary>
        public static void RemoveSongFromPlayList(string name)
        {
            var context = new BlogDBContext();
            var song = context.Songs.FirstOrDefault(f => f.Name == name);
            // Взимаме плейлиста от ABP класа.
            var playList = context.PlayLists.FirstOrDefault(f => f.Name == ABP.Name);
            // Ако не намерим плейлиста или песента се връщаме.
            if (song is null || playList is null)
                return;
            // Взимаме песента от SQL базата данни и я премахваме.
            var playListSong = context.PlayListsSongs.FirstOrDefault(f => f.PlayListID == playList.ID && f.SongID == song.ID);
            if (playListSong != null)
                context.PlayListsSongs.Remove(playListSong);
            context.SaveChanges();
            // Премахваме песента от ABP класа.
            var abpSong = ABP.Songs.FirstOrDefault(f => f.Name == name);
            if (abpSong != null)
                ABP.Songs.Remove(abpSong);
            ABP.OnPropertyChanged("Songs");
            // Премахваме песента от collection класа и обновяваме интерфейса.
            var collectionPlayList = Collection.PlayLists.FirstOrDefault(f => f.Name == ABP.Name);
            var removeSong = collectionPlayList?.Songs.FirstOrDefault(f => f.Name == name);
            if (removeSong != null)
                collectionPlayList.Songs.Remove(removeSong);
            RefreshDataBase();
        }

        /// <summary>
        /// Премахваме плейлист.
        /// </summary>
        public static void RemovePlayList(string name)
        {
            var context = new BlogDBContext();
            var playList = context.PlayLists.FirstOrDefault(f => f.Name == name);
            // Ако плейлиста не съществува се връщаме.
            if (playList is null)
                return;
            // Взимаме всички песни от плейлиста и ги махаме от него.
            var playListSongs = context.PlayListsSongs.Where(w => w.PlayListID == playList.ID);
            context.PlayListsSongs.RemoveRange(playListSongs);
            context.PlayLists.Remove(playList);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
        }

        /// <summary>
        /// Връща лист с песните и информация.
        /// </summary>
        public static void ActivateABP(object type)
        {
            // type е банда,албум или плейлист.
            var objType = type.GetType();
            if (objType == typeof(Band))
            {
                var band = type as Band;
                ABP = new ABPNotify(band.Songs, band.Name);
            }
            else if (objType == typeof(Album))
            {
                var album = type as Album;
                ABP = new ABPNotify(album.Songs, album.Name);
            }
            else if (objType == typeof(PlayList))
            {
                var playlist = type as PlayList;
                ABP = new ABPNotify(playlist.Songs, playlist.Name, true);
            }
        }

        /// <summary>
        /// Добавяме песни към плейлист.
        /// </summary>
        public static void AddSongsToPlayList(string name, Song[] songs)
        {
            var context = new BlogDBContext();
            var playListEF = context.PlayLists.FirstOrDefault(f => f.Name == name);
            // Ако плейлиста не съществува се връщаме.
            if (playListEF is null)
                return;
            // Добавяме тези песни, които вече не са в плейлиста към него.
            var addSongs = songs.Select(s => new PlayListSongsEF
            {
                PlayListID = playListEF.ID,
                SongID = s.ID
            });
            addSongs = addSongs.Where(w => context.PlayListsSongs.FirstOrDefault(f => f.PlayListID == playListEF.ID && f.SongID == w.SongID) is null);
            context.PlayListsSongs.AddRange(addSongs);
            context.SaveChanges();
            // Взимаме плейлиста и го обновяваме.
            var playList = Collection.PlayLists.FirstOrDefault(f => f.Name == name);
            playList.Songs.AddRange(songs.Where(w => playList.Songs.FirstOrDefault(f => f.ID == w.ID) is null));
            PlayList.OnPropertyChanged("PlayList");
        }

        /// <summary>
        /// Взимаме песните от бандата.
        /// </summary>
        public static Song[] GetSongsFromBand(string name)
        {
            var context = new BlogDBContext();
            var band = context.Bands.FirstOrDefault(f => f.Name == name);
            // Ако бандата не съществува се връщаме.
            if (band is null)
                return null;
            var bandSongs = context.BandsSongs.Where(w => w.BandID == band.ID);
            return context.Songs.Where(w => bandSongs.FirstOrDefault(f => f.SongID == w.ID) != null).ToArray();
        }

        /// <summary>
        /// Взимаме песните от албума.
        /// </summary>
        public static Song[] GetSongsFromAlbum(string name)
        {
            var context = new BlogDBContext();
            var album = context.Albums.FirstOrDefault(f => f.Name == name);
            // Ако албума не съществува се връщаме.
            if (album is null)
                return null;
            var albumSongs = context.AlbumsSongs.Where(w => w.AlbumID == album.ID);
            return context.Songs.Where(w => albumSongs.FirstOrDefault(f => f.SongID == w.ID) != null).ToArray();
        }

        /// <summary>
        /// Взимаме песента използвайки нейното име.
        /// </summary>
        public static Song[] GetSongFromName(string name)
        {
            var context = new BlogDBContext();
            var song = context.Songs.FirstOrDefault(f => f.Name == name);
            return song is null ? null : (new[] { song });
        }

        /// <summary>
        /// Премахваме албум.
        /// </summary>
        public static void RemoveAlbum(string name)
        {
            var context = new BlogDBContext();
            var album = context.Albums.FirstOrDefault(f => f.Name == name);
            // Ако албума вече не съществува се връщаме.
            if (album is null)
                return;
            // Взимаме всички песни от албума и ги махаме от бандите и плейлистите също.
            var albumSongs = context.AlbumsSongs.ToArray().Where(w => w.AlbumID == album.ID).ToArray();
            var songs = context.Songs.ToArray().Where(w => albumSongs.FirstOrDefault(f => f.SongID == w.ID) != null);
            var bandSongs = context.BandsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            var playListSongs = context.PlayListsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            context.BandsSongs.RemoveRange(bandSongs);
            context.AlbumsSongs.RemoveRange(albumSongs);
            context.PlayListsSongs.RemoveRange(playListSongs);
            context.SaveChanges();
            // Проверяваме дали някои банди, плейлистове или албуми са празни.
            var albumsSongs = context.AlbumsSongs.ToArray();
            var bandsSongs = context.BandsSongs.ToArray();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var bands = context.Bands.ToArray().Where(f => bandsSongs.FirstOrDefault(c => c.BandID == f.ID) == null);
            var playLists = context.PlayLists.ToArray().Where(f => playListsSongs.FirstOrDefault(c => c.PlayListID == f.ID) == null);
            var albums = context.Albums.ToArray().Where(f => albumsSongs.FirstOrDefault(c => c.AlbumID == f.ID) == null);
            // Ако са празни ги махаме.
            context.Albums.RemoveRange(albums);
            context.Bands.RemoveRange(bands);
            context.PlayLists.RemoveRange(playLists);
            context.Songs.RemoveRange(songs);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
        }

        /// <summary>
        /// Премахваме банда.
        /// </summary>
        public static void RemoveBand(string bandName)
        {
            var context = new BlogDBContext();
            var band = context.Bands.FirstOrDefault(f => f.Name == bandName);
            // Ако бандата вече я няма се връщаме.
            if (band is null)
                return;
            // Взимаме всички песни от бандата и ги махаме от албумите и плейлистите също.
            var bandSongs = context.BandsSongs.ToArray().Where(w => w.BandID == band.ID).ToArray();
            var songs = context.Songs.ToArray().Where(w => bandSongs.FirstOrDefault(f => f.SongID == w.ID) != null);
            var albumSongs = context.AlbumsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            var playListSongs = context.PlayListsSongs.ToArray().Where(w => songs.FirstOrDefault(f => f.ID == w.SongID) != null);
            context.BandsSongs.RemoveRange(bandSongs);
            context.AlbumsSongs.RemoveRange(albumSongs);
            context.PlayListsSongs.RemoveRange(playListSongs);
            context.SaveChanges();
            // Проверяваме дали някои банди, плейлистове или албуми са празни.
            var albumsSongs = context.AlbumsSongs.ToArray();
            var bandsSongs = context.BandsSongs.ToArray();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var albums = context.Albums.ToArray().Where(f => albumsSongs.FirstOrDefault(c => c.AlbumID == f.ID) == null);
            var playLists = context.PlayLists.ToArray().Where(f => playListsSongs.FirstOrDefault(c => c.PlayListID == f.ID) == null);
            var bands = context.Bands.ToArray().Where(f => bandsSongs.FirstOrDefault(c => c.BandID == f.ID) == null);
            // Ако са празни ги махаме.
            context.Albums.RemoveRange(albums);
            context.Bands.RemoveRange(bands);
            context.PlayLists.RemoveRange(playLists);
            context.Songs.RemoveRange(songs);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
        }

        /// <summary>
        /// Добавя нов плейлист към базата данни.
        /// </summary>
        public static void AddPlayList(string name)
        {
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
        /// Премахваме песен.
        /// </summary>
        public static void RemoveSong(string name, bool isAbp = false)
        {
            var context = new BlogDBContext();
            var song = context.Songs.FirstOrDefault(f => f.Name == name);
            // Ако песента вече я няма се връщаме.
            if (song is null) return;
            // Взимаме песента и я премахваме от бандите,албумите и плейлистите.
            var albumSong = context.AlbumsSongs.FirstOrDefault(f => f.SongID == song.ID);
            var bandSong = context.BandsSongs.FirstOrDefault(f => f.SongID == song.ID);
            var playListSong = context.PlayListsSongs.FirstOrDefault(f => f.SongID == song.ID);
            if (albumSong != null)
                context.AlbumsSongs.Remove(albumSong);
            if (bandSong != null)
                context.BandsSongs.Remove(bandSong);
            if (playListSong != null)
                context.PlayListsSongs.Remove(playListSong);
            if (isAbp && ABP.Songs.FirstOrDefault(f => f.Name == name) is Song abpSong)
            {
                ABP.Songs.Remove(abpSong);
                ABP.OnPropertyChanged("Songs");
            }
            context.Songs.Remove(song);
            context.SaveChanges();
            // Проверяваме дали някои банди, плейлистове или албуми са празни.
            var albumsSongs = context.AlbumsSongs.ToArray();
            var bandsSongs = context.BandsSongs.ToArray();
            var playListsSongs = context.PlayListsSongs.ToArray();
            var albums = context.Albums.ToArray().Where(f => albumsSongs.FirstOrDefault(c => c.AlbumID == f.ID) is null);
            var bands = context.Bands.ToArray().Where(f => bandsSongs.FirstOrDefault(c => c.BandID == f.ID) is null);
            var playLists = context.PlayLists.ToArray().Where(f => playListsSongs.FirstOrDefault(c => c.PlayListID == f.ID) is null);
            // Ако са празни ги махаме.
            context.Albums.RemoveRange(albums);
            context.Bands.RemoveRange(bands);
            context.PlayLists.RemoveRange(playLists);
            context.SaveChanges();
            SetCollection();
            RefreshDataBase();
        }

        /// <summary>
        /// Обновява базата данни.
        /// </summary>
        public static void RefreshDataBase()
        {
            NowPlaying.OnPropertyChanged("Songs");
            MySongs.OnPropertyChanged("Bands");
            MySongs.OnPropertyChanged("Albums");
            MySongs.OnPropertyChanged("Songs");
            PlayList.OnPropertyChanged("PlayList");
        }

        /// <summary>    
        /// Превръща данните от SQL към данни, които плеърът може да чете.
        /// </summary>
        public static void SetCollection()
        {
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
            else
            {
                // Ако албумите в collection класа са повече от тези в sql базата, махаме
                // тези които не са в collection класа.
                if (Collection.Albums.Count > albums.Count)
                    Collection.Albums.RemoveAll(r => albums.FirstOrDefault(f => f.Name == r.Name) is null);
                // Ако албумите в collection класа са по-малко от тези в sql базата, добавяме
                // тези които не са в collection класа.
                else if (Collection.Albums.Count < albums.Count)
                    Collection.Albums.AddRange(albums.Where(w => Collection.Albums.FirstOrDefault(f => f.Name == w.Name) is null));
                // Проверяваме дали всички песни в албума съвпадат с останалите.
                for (int i = 0; i < Collection.Albums.Count; i++)
                    if (!(albums.FirstOrDefault(f => f.Name == Collection.Albums[i].Name) is Album album))
                        continue;
                    else if (Collection.Albums[i].Songs.Count > album.Songs.Count)
                        Collection.Albums[i].Songs.RemoveAll(r => album.Songs.FirstOrDefault(f => f.ID == r.ID) is null);
                    else if (Collection.Albums[i].Songs.Count < album.Songs.Count)
                        Collection.Albums[i].Songs.AddRange(album.Songs.Where(w => Collection.Albums[i].Songs.FirstOrDefault(f => f.ID == w.ID) is null));
                // Ако бандите в collection класа са повече от тези в sql базата, махаме
                // тези които не са в collection класа.
                if (Collection.Bands.Count > bands.Count)
                    Collection.Bands.RemoveAll(r => bands.FirstOrDefault(f => f.Name == r.Name) is null);
                // Ако бандите в collection класа са по-малко от тези в sql базата, добавяме
                // тези които не са в collection класа.
                else if (Collection.Bands.Count < bands.Count)
                    Collection.Bands.AddRange(bands.Where(w => Collection.Bands.FirstOrDefault(f => f.Name == w.Name) is null));
                // Проверяваме дали всички песни в бандата съвпадат с останалите.
                for (int i = 0; i < Collection.Bands.Count; i++)
                    if (!(bands.FirstOrDefault(f => f.Name == Collection.Bands[i].Name) is Band band))
                        continue;
                    else if (Collection.Bands[i].Songs.Count > band.Songs.Count)
                        Collection.Bands[i].Songs.RemoveAll(r => band.Songs.FirstOrDefault(f => f.ID == r.ID) is null);
                    else if (Collection.Bands[i].Songs.Count < band.Songs.Count)
                        Collection.Bands[i].Songs.AddRange(band.Songs.Where(w => Collection.Bands[i].Songs.FirstOrDefault(f => f.ID == w.ID) is null));
                // Ако плейлистите в collection класа са повече от тези в sql базата, махаме
                // тези които не са в collection класа.
                if (Collection.PlayLists.Count > playLists.Count)
                    Collection.PlayLists.RemoveAll(r => playLists.FirstOrDefault(f => f.Name == r.Name) is null);
                // Ако плейлистите в collection класа са по-малко от тези в sql базата, добавяме
                // тези които не са в collection класа.
                else if (Collection.PlayLists.Count < playLists.Count)
                    Collection.PlayLists.AddRange(playLists.Where(w => Collection.PlayLists.FirstOrDefault(f => f.Name == w.Name) is null));
                // Проверяваме дали всички песни в плейлиста съвпадат с останалите.
                for (int i = 0; i < Collection.PlayLists.Count; i++)
                    if (!(playLists.FirstOrDefault(f => f.Name == Collection.PlayLists[i].Name) is PlayList playList))
                        continue;
                    else if (Collection.PlayLists[i].Songs.Count > playList.Songs.Count)
                        Collection.PlayLists[i].Songs.RemoveAll(r => playList.Songs.FirstOrDefault(f => f.ID == r.ID) is null);
                    else if (Collection.PlayLists[i].Songs.Count < playList.Songs.Count)
                        Collection.PlayLists[i].Songs.AddRange(playList.Songs.Where(w => Collection.PlayLists[i].Songs.FirstOrDefault(f => f.ID == w.ID) is null));
                // Ако песните в collection класа са повече от тези в sql базата, махаме
                // тези които не са в collection класа.
                if (Collection.Songs.Count > songs.Count)
                    Collection.Songs.RemoveAll(r => songs.FirstOrDefault(f => f.ID == r.ID) is null);
                // Ако песните в collection класа са по-малко от тези в sql базата, добавяме
                // тези които не са в collection класа.
                else if (Collection.Songs.Count < songs.Count)
                    Collection.Songs.AddRange(songs.Where(w => Collection.Songs.FirstOrDefault(f => f.ID == w.ID) is null));
            }
        }
    }
}
