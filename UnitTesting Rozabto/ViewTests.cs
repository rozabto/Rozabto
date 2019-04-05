using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.View;
using System.IO;
using System.Linq;

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
            Assert.Fail();

        }

        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Rozabto.View.Album();
            album.SelectedAlbumName = "The Spectre";
            album.RemoveAlbum(null, null);
            album.SelectedAlbumName = "Sweetener";
            album.AddToPlayList(null, null);
            Assert.Fail();

        }

        [TestMethod]
        public void TestMethod_PlayLists()
        {
            var playLists = new Rozabto.View.Playlists();
            playLists.Playlist.Text = "Testers";
            playLists.AddPlayList(null, null);
            playLists.SelectedPlayListName = "Testers";
            playLists.RemovePlayList(null, null);
            Assert.Fail();
        }

        [TestMethod]
        public void TestMethod_Band()
        {
            var band = new Rozabto.View.Bands();
            band.SelectedBandName = "Nerv";
            band.RemoveBand(null, null);
            band.SelectedBandName = "Like a Storm";
            band.AddToPlayList(null, null);
            Assert.Fail();

        }

        [TestMethod]
        public void TestMethod_Settings()
        {
            var settings = new Settings();
            settings.ChangeColor(null, null);
            Assert.Fail();
        }

        [TestMethod]
        public void TestMethod_SongCounter()
        {
            var counterLoaded = new SongCounter(Directory.EnumerateFiles(@"D:\song").ToArray());
            counterLoaded.CounterLoaded(null, null);
        }

        [TestMethod]
        public void TestMethod_NowplayingList()
        {
            var nowplayingList = new NowplayingList();
            nowplayingList.AddToPlayList(null, null);
            Assert.Fail();


        }

        [TestMethod]
        public void TestMethod_Nowplaying()
        {
            var nowplaying = new Nowplaying();
            Assert.Fail();
        }


        [TestMethod]
        public void TestMethod_MyMusic()
        {
            var mymusic = new MyMusic();
            mymusic.Album(null, null);
            mymusic.Song(null, null);
            mymusic.Band(null, null);

        }

        [TestMethod]
        public void TestMethod_MiniNowplaying()
        {
            var miniNowPlaying = new MiniNowplaying();
            Assert.Fail();

        }


        [TestMethod]
        public void TestMethod_ABPContent()
        {
            var abpContent = new ABPContent();
        }


        [TestMethod]
        public void TestMethod_AddToPlayList()
        {
            var addToPlayList = new AddToPlayList(null);
            addToPlayList.CancelPlayListCreation(null, null);

        }

    }
}
