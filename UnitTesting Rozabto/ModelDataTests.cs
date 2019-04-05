using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Model.Data;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class ModelDataTests
    {
        [TestMethod]
        public void TestMethod_BlogDBContext()
        {
            var context = new BlogDBContext();
            var albums = context.Albums;
            var albumsSongs = context.AlbumsSongs;
            var bands = context.Bands;
            var bandsSongs = context.BandsSongs;
            var playLists = context.PlayLists;
            var playListsSongs = context.PlayListsSongs;
            var songs = context.Songs;

            Assert.AreEqual(context.Albums.Count(), context.Albums.ToArray().Length);
            Assert.AreEqual(context.AlbumsSongs.Count(), context.AlbumsSongs.ToArray().Length);
            Assert.AreEqual(context.Bands.Count(), context.Bands.ToArray().Length);
            Assert.AreEqual(context.BandsSongs.Count(), context.BandsSongs.ToArray().Length);
            Assert.AreEqual(context.PlayLists.Count(), context.PlayLists.ToArray().Length);
            Assert.AreEqual(context.PlayListsSongs.Count(), context.PlayListsSongs.ToArray().Length);
            Assert.AreEqual(context.Songs.Count(), context.Songs.ToArray().Length);

        }
    }
}
