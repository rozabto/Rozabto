using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using TagFile = TagLib.File;

namespace Rozabto.Model.Data
{
    public static class MusicInformation
    {
        public static readonly string BandNameIsUnknown = "Unknown";
        

        public class FileTagLib : TagFile.IFileAbstraction
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
            

            foreach (var path in paths)
            {
                
                var file = new FileInfo(path);
                TagFile tagLibFile = null;
                try
                {
                    tagLibFile = TagFile.Create(path);
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
                    
                 
                       
                    song = new Song
                    {
                   
                        Name = tag.Title,
                        Location = file.FullName , 
                        Duration = tagLibFile.Properties.Duration
                    };
                    band.Songs.Add(song);
                    album.Songs.Add(song);
                    collection.Songs.Add(song);
                }
            }
        }
    }
}