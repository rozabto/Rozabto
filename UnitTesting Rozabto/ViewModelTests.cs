using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel;

namespace UnitTesting_Rozabto
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void TestMethod_MainViewModel()
        {
            var collection = MainViewModel.Collection;
            var mediaPlayer = MainViewModel.Player;
            var mySongsNotify = MainViewModel.MySongs;
            var nowPlayingNotify = MainViewModel.NowPlaying;
            var playlistNotify = MainViewModel.PlayList;
            var abpNotify = MainViewModel.ABP;
            var songStatus = MainViewModel.Status;
            var volumeState = MainViewModel.Volume;
            var theme = MainViewModel.Theme;


            MainViewModel.Status = Rozabto.Model.SongStatus.Paused;
            MainViewModel.Volume = Rozabto.Model.VolumeState.High;
            MainViewModel.Theme = false;

            Assert.AreEqual(MainViewModel.PlayList.PlayList.Count, MainViewModel.PlayList.PlayList.Count);
            MainViewModel.ActivateABP(new Band("test"));
            MainViewModel.ActivateABP(new Album("test"));
            MainViewModel.ActivateABP(new PlayList("test"));
            MainViewModel.AddPlayList("Test");
            MainViewModel.RefreshDataBase();
            MainViewModel.Play();
            MainViewModel.Status = Rozabto.Model.SongStatus.Playing;
            MainViewModel.Play();
            MainViewModel.Status = Rozabto.Model.SongStatus.Stopped;
            MainViewModel.Play();
            Assert.AreEqual(MainViewModel.NowPlaying.Songs.Count, MainViewModel.NowPlaying.Songs.Count);
            var songband = MainViewModel.NowPlaying.SongBand;
            var pausebutton = MainViewModel.NowPlaying.PauseButton;
            var mutebutton = MainViewModel.NowPlaying.MuteButton;
            var repeatsong = MainViewModel.NowPlaying.RepeatSong;
            var shuflesong = MainViewModel.NowPlaying.ShuffleSongs;
            var currentsongpos = MainViewModel.NowPlaying.CurrentSongPos;
            MainViewModel.NowPlaying.RepeatSong = false;
            MainViewModel.NowPlaying.ShuffleSongs = false;
            MainViewModel.NowPlaying.CurrentSongPos = 9;

            Assert.AreEqual(MainViewModel.MySongs.Albums.Count, MainViewModel.MySongs.Albums.Count);
            Assert.AreEqual(MainViewModel.MySongs.Bands.Count, MainViewModel.MySongs.Bands.Count);
            Assert.AreEqual(MainViewModel.MySongs.Songs.Count, MainViewModel.MySongs.Songs.Count);

            var totalTime = MainViewModel.ABP.TotalTime;
            var name = MainViewModel.ABP.Name;
            var songscount = MainViewModel.ABP.SongsCount;
            var songs = MainViewModel.ABP.Songs;


        }

        [TestMethod]
        public void TestMethod_MediaViewModel()
        {
            var slider = MediaViewModel.SliderDragging;
        }

        [TestMethod]
        public void TestMethod_VolumeLabel()
        {
            var grid = new Grid();
            VolumeLabel.Show(grid, 1);
            VolumeLabel.Changed(grid, 1);
            VolumeLabel.Hide(grid);
            VolumeLabel.Changed(grid, 1);
        }

    }
}
