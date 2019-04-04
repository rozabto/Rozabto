using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
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
            var mediaPlayer = MediaViewModel.Player;
            var mySongsNotify = MainViewModel.MySongs;
            var nowPlayingNotify = MainViewModel.NowPlaying;
            var playlistNotify = MainViewModel.PlayList;
            var abpNotify = MainViewModel.ABP;
            var songStatus = MediaViewModel.Status;
            var volumeState = MediaViewModel.Volume;
            var theme = MainViewModel.Theme;


            MediaViewModel.Status = Rozabto.Model.SongStatus.Paused;
            MediaViewModel.Volume = Rozabto.Model.VolumeState.High;
            MainViewModel.Theme = false;

            Assert.AreEqual(MainViewModel.PlayList.PlayList.Count, MainViewModel.PlayList.PlayList.Count);
            MainViewModel.ActivateABP(new Band("test"));
            MainViewModel.ActivateABP(new Album("test"));
            MainViewModel.ActivateABP(new PlayList("test", new System.Collections.Generic.List<Song> { new Song { Duration = TimeSpan.FromSeconds(120) } }));
            MainViewModel.AddPlayList("Test");
            MainViewModel.AddSongsToPlayList("Test", new Song[] { Song.EmptySong });
            MainViewModel.RefreshDataBase();
            MediaViewModel.Play();
            MediaViewModel.Status = Rozabto.Model.SongStatus.Playing;
            MediaViewModel.Play();
            MediaViewModel.Status = Rozabto.Model.SongStatus.Stopped;
            MediaViewModel.Play();
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

            var context = new BlogDBContext();

            var factory = new MusicInformation();
            foreach (var song in Directory.EnumerateFiles(@"D:\song"))
            {
                factory.SearchMusic(song).Wait();
            }

            MainViewModel.RemoveSong("My Heart", true);
            MainViewModel.RemoveAlbum(context.Albums.First().Name);
            MainViewModel.RemoveBand(context.Bands.First().Name);

            MainViewModel.RemoveSongFromPlayList("Test");
           
        }

        [TestMethod]
        public void TestMethod_MediaViewModel()
        {
            var slider = MediaViewModel.SliderDragging;
            MediaViewModel.SliderDragging = true;

            MediaViewModel.ConnectViewToViewModel(new Slider(), new Slider(), new Label());
            MediaViewModel.Player_MediaFailed(null, null);
            MediaViewModel.SliderTimer_Tick(null, null);
            MediaViewModel.Player_MediaEnded(null, null);
            MediaViewModel.Player_MediaOpened(new MediaPlayer (), null);
            MediaViewModel.TimerPlay();
            MediaViewModel.Play();
            MediaViewModel.SetVolumeToPlayer();
            MediaViewModel.Stop();
            MediaViewModel.SaveVolume();

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
        [TestMethod]
        public void TestMethod_Properties()
        {
            var propertie = Rozabto.Properties.Settings.Default;

        }

    }
}
