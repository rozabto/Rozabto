using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rozabto.View;
using Rozabto.ViewModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

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
          
           
        }

        [TestMethod]
        public void TestMethod_Album()
        {
            var album = new Rozabto.View.Album();
            album.AddToPlayList(null, null);

 
        }

        [TestMethod]
        public void TestMethod_PlayLists()
        {
            var playLists = new Rozabto.View.Playlists();
            playLists.AddPlayList(null, null);
            playLists.SelectPlayList(new ListBox(), null);
        }

        [TestMethod]
        public void TestMethod_Band()
        {
            var band = new Rozabto.View.Bands();
            band.AddToPlayList(null, null);
           
        }

        [TestMethod]
        public void TestMethod_Settings()
        {
            var settings = new Settings();
            settings.ChangeColor(null, null);
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
     


        }

        [TestMethod]
        public void TestMethod_Nowplaying()
        {
            var nowplaying = new Nowplaying();
       
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
          

        }


        [TestMethod]
        public void TestMethod_ABPContent()
        {
            var abpContent = new ABPContent();
            abpContent.ListBox_SelectionChanged(null, null);

        }


        [TestMethod]
        public void TestMethod_AddToPlayList()
        {
            var addToPlayList = new AddToPlayList(null);
            addToPlayList.CancelPlayListCreation(null, null);
            addToPlayList.ChoosePlayList(null, null);

        }

    }
}
