using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Model;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class ModelsTests
    {
        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Album("Album_Namee");
            Assert.AreEqual(album.Name, "Album_Namee");
            album = new Album("Name", new System.Collections.Generic.List<Song>());

        }

        [TestMethod]
        public void TestMethod_AlbumEF()
        {
            var album = new AlbumEF();

            album.ID = 42;
            album.Name = "Prodigy";

            Assert.AreEqual(album.ID, 42);
            Assert.AreEqual(album.Name, "Prodigy");
        }


        [TestMethod]
        public void TestMethod_AlbumSongsEF()
        {
            var album = new AlbumSongsEF();

            album.ID = 42;
            album.AlbumID = 25;
            album.SongID = 20;

            Assert.AreEqual(album.ID, 42);
            Assert.AreEqual(album.AlbumID, 25);
            Assert.AreEqual(album.SongID, 20);
        }



        [TestMethod]
        public void TestMethod_BandSongsEF()
        {
            var album = new BandSongsEF();

            album.ID = 42;
            album.BandID = 25;
            album.SongID = 20;

            Assert.AreEqual(album.ID, 42);
            Assert.AreEqual(album.BandID, 25);
            Assert.AreEqual(album.SongID, 20);
        }

        [TestMethod]
        public void TestMethod_Band()
        {
            var band = new Band("Band_Namee");
            Assert.AreEqual(band.Name, "Band_Namee");
            band = new Band("Name", new System.Collections.Generic.List<Song>());
        }

        [TestMethod]
        public void TestMethod_BandEF()
        {
            var band = new BandEF();

            band.ID = 42;
            band.Name = "Prodigy";

            Assert.AreEqual(band.ID, 42);
            Assert.AreEqual(band.Name, "Prodigy");
        }


        [TestMethod]
        public void TestMethod_PlayListSongsEF()
        {
            var album = new PlayListSongsEF();

            album.ID = 42;
            album.PlayListID = 25;
            album.SongID = 20;

            Assert.AreEqual(album.ID, 42);
            Assert.AreEqual(album.PlayListID, 25);
            Assert.AreEqual(album.SongID, 20);
        }

        [TestMethod]
        public void TestMethod_Playlist()
        {
            var playlist = new PlayList("Playlist_Namee");
            Assert.AreEqual(playlist.Name, "Playlist_Namee");
            playlist = new PlayList("Name", new System.Collections.Generic.List<Song>());
        }

        [TestMethod]
        public void TestMethod_PlaylistEF()
        {
            var playlist = new PlayListEF();

            playlist.ID = 42;
            playlist.Name = "Prodigy";

            Assert.AreEqual(playlist.ID, 42);
            Assert.AreEqual(playlist.Name, "Prodigy");
        }

        [TestMethod]
        public void TestMethod_Song()
        {
            var song = new Song
            {
                ID = 42,
                Name = "",
                Duration = default(TimeSpan),
                Location = ""
            };
            Assert.AreEqual(song.Name, Song.EmptySong.Name);
            Assert.AreEqual(song.Location, Song.EmptySong.Location);
            Assert.AreEqual(song.Duration, Song.EmptySong.Duration);
            Assert.AreEqual(song.Volume, Song.EmptySong.Volume);
            Assert.AreEqual(song.ID, 42);
            Assert.AreEqual(song.DurationString, Song.EmptySong.DurationString);
            song = new Song { Duration = TimeSpan.FromDays(1) };
            var duration = song.DurationString;
        }


        [TestMethod]
        public void TestMethod_Collection()
        {

            var collection = new Collection(
                new System.Collections.Generic.List<Album>(),
                new System.Collections.Generic.List<Band>(),
                new System.Collections.Generic.List<PlayList>(),
                new System.Collections.Generic.List<Song>()
                );
            Assert.AreEqual(collection.Albums.Count, new System.Collections.Generic.List<Album>().Count);
            Assert.AreEqual(collection.Bands.Count, new System.Collections.Generic.List<Band>().Count);
            Assert.AreEqual(collection.PlayLists.Count, new System.Collections.Generic.List<PlayList>().Count);
            Assert.AreEqual(collection.Songs.Count, new System.Collections.Generic.List<Song>().Count);
        }

        [TestMethod]
        public void TestMethod_SongList()
        {
            var songList = new SongList();
            songList = new SongList(new System.Collections.Generic.List<Song>());
            Assert.AreEqual(songList.Songs.Count, new System.Collections.Generic.List<Song>().Count);

        }

    }
}
