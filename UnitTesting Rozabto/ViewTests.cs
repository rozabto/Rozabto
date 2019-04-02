using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.View;
using System.Windows.Controls;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class ViewTests
    {
        [TestMethod]
        public void TestMethod_Songs()
        {
            var songs = new Songs();
            songs.AddToPlayList(null, null);
            songs.RemoveSong(null, null);
            songs.Add_Closed(null, null);
        }

        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Rozabto.View.Album();
            album.AddToPlayList(null, null);
            album.RemoveAlbum(null, null);
            album.SelectAlbum(new ListBox(), null);
            album.Add_Closed(null, null);
        }

        [TestMethod]
        public void TestMethod_PlayLists()
        {
            var playLists = new Rozabto.View.Playlists();
        }

        [TestMethod]
        public void TestMethod_Band()
        {
            var band = new Rozabto.View.Bands();
            band.AddToPlayList(null, null);
            band.RemoveBand(null, null);
            band.SelectBand(new ListBox(), null);
            band.Add_Closed(null, null);
        }

    }
}
