﻿using Rozabto.Model;
using Rozabto.Model.Data;
using Rozabto.ViewModel.Notify;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Rozabto.ViewModel {
    public static class MainViewModel {
        public static Collection Collection { get; private set; }
        public static MediaPlayer Player { get; }
        public static MySongsNotify MySongs { get; }
        public static NowPlayingNotify NowPlaying { get; }
        public static PlayListsNotify PlayList { get; }
        public static ABPNotify ABP { get; private set; }
        public static SettingsNotify Settings { get; }
        public static SongStatus Status { get; set; }
        public static VolumeState Volume { get; set; }

        static MainViewModel() {
            Player = new MediaPlayer();
            SetCollection();
            Settings = new SettingsNotify();
            MySongs = new MySongsNotify(Collection);
            NowPlaying = new NowPlayingNotify(Collection);
            PlayList = new PlayListsNotify(Collection);
            Volume = VolumeState.On;
        }

        public static void ActivateABP(object type) {
            if (type.GetType() == typeof(Band)) {
                var band = type as Band;
                ABP = new ABPNotify(band.Songs, band.Name);
            }
            else if (type.GetType() == typeof(Album)) {
                var album = type as Album;
                ABP = new ABPNotify(album.Songs, album.Name);
            }
            else if (type.GetType() == typeof(PlayList)) {
                var playlist = type as PlayList;
                ABP = new ABPNotify(playlist.Songs, playlist.Name);
            }
        }

        public static void AddPlayList(string name) {
            var context = new BlogDBContext();
            var playlist = new PlayList(name);
            Collection.PlayLists.Add(playlist);
            context.PlayListEFs.Add(new PlayListEF { Name = name });
            context.SaveChanges();
            PlayList.OnPropertyChanged("PlayList");
        }

        public static void AddSongs(string[] songs) {
            MusicInformation.SearchMusic(songs);
            SetCollection();
            NowPlaying.OnPropertyChanged("Songs");
            MySongs.OnPropertyChanged("Bands");
            MySongs.OnPropertyChanged("Albums");
            MySongs.OnPropertyChanged("Songs");
        }

        public static void Play() {
            switch (Status) {
                case SongStatus.Stopped:
                    Player.Close();
                    if (NowPlaying.CurrentSong != Song.EmptySong) {
                        Player.Open(new Uri(NowPlaying.CurrentSong.Location, UriKind.RelativeOrAbsolute));
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

        static void SetCollection() {
            var context = new BlogDBContext();
            var songs = new List<Song>(context.Songs);
            var bands = new List<Band>();
            var albums = new List<Album>();
            var playLists = new List<PlayList>();
            if (songs.Count != 0) {
                var bandEFs = new List<BandEF>(context.BandEFs);
                foreach (var bandSong in context.BandsSongEFs) {
                    var list = new List<Song>();
                    foreach (var song in songs)
                        if (bandSong.SongID == song.ID)
                            list.Add(song);
                    string name = null;
                    foreach (var band in bandEFs)
                        if (band.ID == bandSong.BandID) {
                            name = band.Name;
                            break;
                        }
                    bands.Add(new Band(name, list));
                }
                var albumEFs = new List<AlbumEF>(context.AlbumEFs);
                foreach (var albumSong in context.AlbumsSongEFs) {
                    var list = new List<Song>();
                    foreach (var song in songs)
                        if (albumSong.SongID == song.ID)
                            list.Add(song);
                    string name = null;
                    foreach (var album in albumEFs)
                        if (album.ID == albumSong.AlbumID) {
                            name = album.Name;
                            break;
                        }
                    albums.Add(new Album(name, list));
                }
            }
            var playlistEFs = new List<PlayListEF>(context.PlayListEFs);
            foreach (var playListSong in context.PlayListsSongEFs) {
                var list = new List<Song>();
                foreach (var song in songs)
                    if (playListSong.SongID == song.ID)
                        list.Add(song);
                string name = null;
                foreach (var playlist in playlistEFs)
                    if (playlist.ID == playListSong.PlayListID) {
                        name = playlist.Name;
                        break;
                    }
                playLists.Add(new PlayList(name, list));
            }
            Collection = new Collection(albums, bands, playLists, songs);
        }
    }
}
