using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Model;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Album("Album_Namee");
            Assert.AreEqual(album.Name, "Album_Namee");
        }
        [TestMethod]
        public void TestMethod_Band()
        {
            var band = new Band("Band_Namee");
            Assert.AreEqual(band.Name, "Band_Namee");
        }

        [TestMethod]
        public void TestMethod_Playlist()
        {
            var playlist = new PlayList("Playlist_Namee");
            Assert.AreEqual(playlist.Name, "Playlist_Namee");
        }

        [TestMethod]
        public void TestMethod_Song()
        {
            var song = new Song
            {
                Name = "",
                Duration = default(TimeSpan),
                Location = ""
            };
            Assert.AreEqual(song.Name, Song.EmptySong.Name);
            Assert.AreEqual(song.Location, Song.EmptySong.Location);
            Assert.AreEqual(song.Duration, Song.EmptySong.Duration);
        }

    }
}
