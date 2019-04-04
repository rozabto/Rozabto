using MaterialDesignThemes.Wpf;
using Rozabto.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Rozabto.ViewModel
{
    /// <summary>
    /// Контролира звукът.
    /// </summary>
    public static class MediaViewModel {
        public static MediaPlayer Player { get; }
        public static VolumeState Volume { get; set; }
        public static SongStatus Status { get; set; }
        public static bool SliderDragging { get; set; }

        private static readonly DispatcherTimer _sliderTimer;
        private static Slider _musicSlider;
        private static Slider _volumeSlider;
        private static Label _songTime;
        private static int _volumeValue;

        static MediaViewModel() {
            Player = new MediaPlayer();
            Player.MediaFailed += Player_MediaFailed;
            Player.MediaOpened += Player_MediaOpened;
            Player.MediaEnded += Player_MediaEnded;

            Player.Volume = 0.5f;
            _volumeValue = 50;

            _sliderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            _sliderTimer.Tick += SliderTimer_Tick;
            Volume = VolumeState.On;
        }

        /// <summary>
        /// Даваме нужните елементи на класа.
        /// </summary>
        public static void ConnectViewToViewModel(Slider musicSlider, Slider volumeSlider, Label songTime)
        {
            _musicSlider = musicSlider;
            _volumeSlider = volumeSlider;
            _songTime = songTime;
            _volumeSlider.Value = _volumeValue;
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
            if (SliderDragging || _musicSlider is null) return;
            _musicSlider.Value = Player.Position.TotalSeconds;
            _songTime.Content = Player.Position.ToString(@"mm\:ss");
        }

        private static void Player_MediaEnded(object sender, EventArgs e)
        {
            // Ако песента свърши този метод се извиква.
            _musicSlider.Value = 0;
            _songTime.Content = "";
            _sliderTimer.Stop();
            Status = SongStatus.Stopped;
            var nowPlaying = MainViewModel.NowPlaying;
            // Ако е зададен да започне нова песен тогава я пускаме.
            if (nowPlaying.RepeatSong)
            {
                TimerPlay();
                return;
            }
            if (nowPlaying.Songs.Count <= 0)
                return;
            // Взимаме позицията на новата песен.
            var pos = nowPlaying.ShuffleSongs ? new Random().Next(nowPlaying.Songs.Count)
                : nowPlaying.CurrentSongPos + 1;
            if (pos >= nowPlaying.Songs.Count)
                pos -= nowPlaying.Songs.Count;
            // Слагаме песента на позицията на новата песен.
            nowPlaying.CurrentSong = nowPlaying.Songs[pos];
            nowPlaying.CurrentSongPos = pos;
            TimerPlay();
        }

        private static void Player_MediaOpened(object sender, EventArgs e)
        {
            var player = sender as MediaPlayer;
            player.Position = new TimeSpan(0, 0, 0);
            _musicSlider.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
            _musicSlider.Minimum = 0;
            // Слагаме звукът на плеъра спрямо звука на новата песен.
            SetVolumeToPlayer();
            MainViewModel.NowPlaying.OnPropertyChanged("SongBand");
        }

        /// <summary>
        /// Пуска или спира движението на плъзгача.
        /// </summary>
        public static void TimerPlay()
        {
            Play();
            switch (Status)
            {
                case SongStatus.Playing:
                    _sliderTimer.Start();
                    MainViewModel.NowPlaying.PauseButton = PackIconKind.Pause;
                    break;
                case SongStatus.Paused:
                // Ако е на пауза трябва да отиде на спряно.
                case SongStatus.Stopped:
                    _sliderTimer.Stop();
                    MainViewModel.NowPlaying.PauseButton = PackIconKind.Play;
                    break;
            }
        }

        /// <summary>
        /// Пуска, прекъсва или спира песните.
        /// </summary>
        public static void Play() {
            switch (Status) {
                case SongStatus.Stopped:
                    // Спираме песента.
                    Player.Close();
                    // Ако песента не е празна, пускаме нова песен.
                    if (MainViewModel.NowPlaying.CurrentSong != Song.EmptySong) {
                        Player.Open(new Uri(MainViewModel.NowPlaying.CurrentSong.Location, UriKind.RelativeOrAbsolute));
                        Player.Play();
                        Status = SongStatus.Playing;
                    }
                    break;
                case SongStatus.Playing:
                    Player.Pause();
                    Status = SongStatus.Paused;
                    break;
                case SongStatus.Paused:
                    Player.Play();
                    Status = SongStatus.Playing;
                    break;
            }
        }

        /// <summary>
        /// Задава звука на плеъра и променя иконата на звука спрямо силата му.
        /// </summary>
        public static void SetVolumeToPlayer()
        {
            if (Volume == VolumeState.Mute) return;
            var nowPlaying = MainViewModel.NowPlaying;
            // Ако са null тогава няма да променяме звука (избягваме появата на exception).
            if (_volumeSlider != null && nowPlaying.CurrentSong != null && nowPlaying.CurrentSong != Song.EmptySong)
                // Поставяме звука на плеъра спрямо резултата от формулата.
                Player.Volume = Math.Round(Math.Pow(_volumeSlider.Value / 100d, 1.150515 - Math.Sin((1 - nowPlaying.CurrentSong.Volume) / 2)), 3);
            var volume = Player.Volume;
            if (volume == 0 && Volume != VolumeState.Zero)
            {
                Volume = VolumeState.Zero;
                nowPlaying.MuteButton = PackIconKind.VolumeMute;
            }
            else if (volume > 0 && volume <= 0.298 && Volume != VolumeState.Low)
            {
                Volume = VolumeState.Low;
                nowPlaying.MuteButton = PackIconKind.VolumeLow;
            }
            else if (volume > 0.298 && volume <= 0.663 && Volume != VolumeState.Medium)
            {
                Volume = VolumeState.Medium;
                nowPlaying.MuteButton = PackIconKind.VolumeMedium;
            }
            else if (volume > 0.663 && Volume != VolumeState.High)
            {
                Volume = VolumeState.High;
                nowPlaying.MuteButton = PackIconKind.VolumeHigh;
            }
        }

        /// <summary>
        ///  Спираме песента.
        /// </summary>
        public static void Stop()
        {
            _sliderTimer.Stop();
            Status = SongStatus.Stopped;
            MainViewModel.NowPlaying.PauseButton = PackIconKind.Pause;
            _musicSlider.Value = 0;
            Player.Stop();
        }

        /// <summary>
        /// Запазваме звука когато излезем от NowPlaying.
        /// </summary>
        public static void SaveVolume()
        {
            _volumeValue = (int)_volumeSlider.Value;
        }
    }
}
