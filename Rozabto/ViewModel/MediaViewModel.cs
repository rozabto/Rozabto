using MaterialDesignThemes.Wpf;
using Rozabto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Rozabto.ViewModel
{
    /// <summary>
    /// Контролира звукът.
    /// </summary>
    public static class MediaViewModel
    {
        private static readonly DispatcherTimer SliderTimer;
        private static Slider MusicSlider;
        private static Slider VolumeSlider;
        private static Label SongTime;
        private static int VolumeValue;
        public static bool SliderDragging { get; set; }

        static MediaViewModel()
        {
            MainViewModel.Player.MediaFailed += Player_MediaFailed;
            MainViewModel.Player.MediaOpened += Player_MediaOpened;
            MainViewModel.Player.MediaEnded += Player_MediaEnded;

            MainViewModel.Player.Volume = 0.5f;
            VolumeValue = 50;

            SliderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            SliderTimer.Tick += SliderTimer_Tick;
        }

        /// <summary>
        /// Даваме нужните елементи на класа.
        /// </summary>
        public static void ConnectViewToViewModel(Slider musicSlider, Slider volumeSlider, Label songTime)
        {
            MusicSlider = musicSlider;
            VolumeSlider = volumeSlider;
            SongTime = songTime;
            VolumeSlider.Value = VolumeValue;
        }

        private static void Player_MediaFailed(object sender, ExceptionEventArgs e)
        {
            // Ако песента не може да се пусне показваме error.
            MessageBox.Show(e.ErrorException.Message);
            Stop();
        }

        private static void SliderTimer_Tick(object sender, EventArgs e)
        {
            // Ако не местим плъзгача за времетраенето на музиката,тогава той продължава да се движи заедно с песента.
            if (SliderDragging || MusicSlider is null) return;
            MusicSlider.Value = MainViewModel.Player.Position.TotalSeconds;
            SongTime.Content = MainViewModel.Player.Position.ToString(@"mm\:ss");
        }

        private static void Player_MediaEnded(object sender, EventArgs e)
        {
            // Ако песента свърши този метод се извиква.
            MusicSlider.Value = 0;
            SongTime.Content = "";
            SliderTimer.Stop();
            MainViewModel.Status = SongStatus.Stopped;
            // Ако е зададен да започне нова песен тогава я пускаме.
            if (MainViewModel.NowPlaying.RepeatSong)
            {
                Play();
                return;
            }
            if (MainViewModel.NowPlaying.Songs.Count <= 0)
                return;
            // Взимаме позицията на новата песен.
            var pos = MainViewModel.NowPlaying.ShuffleSongs ? new Random().Next(MainViewModel.NowPlaying.Songs.Count)
                : MainViewModel.NowPlaying.CurrentSongPos + 1;
            if (pos >= MainViewModel.NowPlaying.Songs.Count)
                pos -= MainViewModel.NowPlaying.Songs.Count;
            // Слагаме песента на позицията на новата песен.
            MainViewModel.NowPlaying.CurrentSong = MainViewModel.NowPlaying.Songs[pos];
            MainViewModel.NowPlaying.CurrentSongPos = pos;
            Play();
        }

        private static void Player_MediaOpened(object sender, EventArgs e)
        {
            var player = sender as MediaPlayer;
            player.Position = new TimeSpan(0, 0, 0);
            MusicSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            MusicSlider.Minimum = 0;
            // Слагаме звукът на плеъра спрямо звука на новата песен.
            SetVolumeToPlayer();
            MainViewModel.NowPlaying.OnPropertyChanged("SongBand");
        }

        /// <summary>
        /// Пуска или спира движението на плъзгача.
        /// </summary>
        public static void Play()
        {
            MainViewModel.Play();
            switch (MainViewModel.Status)
            {
                case SongStatus.Playing:
                    SliderTimer.Start();
                    MainViewModel.NowPlaying.PauseButton = PackIconKind.Pause;
                    break;
                case SongStatus.Paused:
                // Ако е на пауза трябва да отиде на спряно.
                case SongStatus.Stopped:
                    SliderTimer.Stop();
                    MainViewModel.NowPlaying.PauseButton = PackIconKind.Play;
                    break;
            }
        }

        /// <summary>
        /// Задава звука на плеъра и променя иконата на звука спрямо силата му.
        /// </summary>
        public static void SetVolumeToPlayer()
        {
            if (MainViewModel.Volume == VolumeState.Mute) return;
            // Ако са null тогава няма да променяме звука (избягваме появата на exception).
            if (VolumeSlider != null && MainViewModel.NowPlaying.CurrentSong != null)
                // Поставяме звука на плеъра спрямо резултата от формулата.
                MainViewModel.Player.Volume = Math.Round(Math.Pow(VolumeSlider.Value / 100d, 1.150515 - Math.Sin((1 - MainViewModel.NowPlaying.CurrentSong.Volume) / 2)), 3);
            var volume = MainViewModel.Player.Volume;
            if (volume == 0 && MainViewModel.Volume != VolumeState.Zero)
            {
                MainViewModel.Volume = VolumeState.Zero;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeMute;
            }
            else if (volume > 0 && volume <= 0.298 && MainViewModel.Volume != VolumeState.Low)
            {
                MainViewModel.Volume = VolumeState.Low;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeLow;
            }
            else if (volume > 0.298 && volume <= 0.663 && MainViewModel.Volume != VolumeState.Medium)
            {
                MainViewModel.Volume = VolumeState.Medium;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeMedium;
            }
            else if (volume > 0.663 && MainViewModel.Volume != VolumeState.High)
            {
                MainViewModel.Volume = VolumeState.High;
                MainViewModel.NowPlaying.MuteButton = PackIconKind.VolumeHigh;
            }
        }

        /// <summary>
        ///  Спираме песента.
        /// </summary>
        public static void Stop()
        {
            SliderTimer.Stop();
            MainViewModel.Status = SongStatus.Stopped;
            MainViewModel.NowPlaying.PauseButton = PackIconKind.Pause;
            MusicSlider.Value = 0;
            MainViewModel.Player.Stop();
        }

        /// <summary>
        /// Запазваме звука когато излезем от NowPlaying.
        /// </summary>
        public static void SaveVolume()
        {
            VolumeValue = (int)VolumeSlider.Value;
        }
    }
}
