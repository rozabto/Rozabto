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
            playLists.AddPlayList(null, null);
            playLists.SelectPlayList(new ListBox(), null);
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
            nowplayingList.FocusOnSelectedItem(null, null);
            nowplayingList.AddToPlayList(null, null);
            nowplayingList.RemoveSong(null, null);
            nowplayingList.ListBox_MouseDoubleClick(new ListBox(), null);


        }

        [TestMethod]
        public void TestMethod_Nowplaying()
        {
            var nowplaying = new Nowplaying();
            nowplaying.SliderControl_PreviewMouseLeftButtonDown(null, new MouseButtonEventArgs(null, 0, MouseButton.Right));
            nowplaying.VolumeSlider_ValueChanged(null, null);
            nowplaying.PlayPause(null, null);
            nowplaying.MusicSlider_DragCompleted(null, null);
            nowplaying.MusicSlider_DragStarted(null, null);
            nowplaying.MoveBackSong(null, null);
            nowplaying.MoveSongForward(null, null);
            nowplaying.Randomizer(null, null);
            nowplaying.Repeater(null, null);
            nowplaying.SaveVolumeSliderValue(null, null);
            nowplaying.ShowVolumeNumber(null, null);
            nowplaying.HideVolumeNumber(null, null);
            nowplaying.MuteButton(null, null);


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
            miniNowPlaying.PlayPause(null, null);
            miniNowPlaying.MoveBackSong(null, null);
            miniNowPlaying.MoveSongForward(null, null);

        }


        [TestMethod]
        public void TestMethod_ABPContent()
        {
            var abpContent = new ABPContent();
            abpContent.ListBox_SelectionChanged(null, null);

        }

    }
}
