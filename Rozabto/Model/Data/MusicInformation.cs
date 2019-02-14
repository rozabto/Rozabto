using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace Rozabto.Model.Data
{
    public static class MusicInformation
    {
        public static readonly string BandNameIsUnknown = "Unknown";
        

        public class FileTagLib : TagLib.File.IFileAbstraction
        {
            private readonly FileInfo file;
            public FileTagLib(FileInfo file)
            {
                this.file = file;
            }


            public void CloseStream(Stream stream)
            {
                stream.Close();
            }

            public string Name => file.Name;
            public Stream ReadStream => file.OpenRead();
            public Stream WriteStream => file.OpenWrite();

        }
   
        public static void SearchMusic(string[] paths, Collection collection) 
        {
            Random random = new Random();

            foreach (var path in paths)
            {
                MusicInformation.SearchMusic(paths, collection);
                var file = new FileInfo(path);
                TagLib.File tagLibFile = null;
                try
                {
                    tagLibFile = TagLib.File.Create(path);
                }
                catch (Exception)
                {
                    continue;
                }
                using (tagLibFile)
                {
                    Band band = null;
                    Song song = null;
                    Album album = null;
                    var tag = tagLibFile.Tag;
                    var bandName = tag.FirstAlbumArtist ?? tag.FirstPerformer ?? tag.FirstComposer ?? "Unknown";

                    if (tag.Title is null)
                        tag.Title = file.Name;
                    var albumName = tag.Album ?? tag.Title;
                    band = collection.Bands.FirstOrDefault(b => b.Name == bandName);
                    if (band is null)
                    {
                        band = new Band
                        {
                            Name = bandName
                        };
                        collection.Bands.Add(band);
                    }
                    album = collection.Albums.FirstOrDefault(b => b.Name == albumName);
                    if (album is null)
                    {
                        album = new Album
                        {
                            Name = albumName
                        };
                        collection.Albums.Add(album);
                    }
                    song = collection.Songs.FirstOrDefault(s => s.Name == tag.Title);
                    if (song != null)
                        tag.Title += "_";
                    int rnd = random.Next(int.MinValue, int.MaxValue);
                    while (collection.Songs.FirstOrDefault(f => f.ID == rnd) != null)
                        rnd = random.Next(int.MinValue, int.MaxValue);
                    song = new Song
                    {
                        ID = rnd ,
                        Name = tag.Title,
                        Location = file.FullName , 
                        Duration = tagLibFile.Properties.Duration
                    };
                    band.IDsongs.Add(rnd);
                    album.IDsongs.Add(rnd);
                    collection.Songs.Add(song);
                }
            }
        }
    }
}