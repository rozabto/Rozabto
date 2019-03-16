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

        public static void SearchMusic(string[] paths)
        {
            var context = new BlogDBContext();
            Random random = new Random();

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
                    BandEF band = null;
                    Song song = null;
                    AlbumEF album = null;
                    var tag = tagLibFile.Tag;
                    var bandName = tag.FirstAlbumArtist ?? tag.FirstPerformer ?? tag.FirstComposer ?? "Unknown";

                    if (tag.Title is null)
                        tag.Title = Path.GetFileNameWithoutExtension(file.Name);
                    var albumName = tag.Album ?? "Unknown";
                    song = context.Songs.FirstOrDefault(s => s.Name == tag.Title);

                    if (song != null)
                        continue;

                    band = context.BandEFs.FirstOrDefault(f => f.Name == bandName);
                    if (band is null)
                    {
                        band = new BandEF
                        {
                            Name = bandName
                        };
                        context.BandEFs.Add(band);
                    }

                    album = context.AlbumEFs.FirstOrDefault(f => f.Name == albumName);
                    if (album is null)
                    {
                        album = new AlbumEF
                        {
                            Name = albumName
                        };
                        context.AlbumEFs.Add(album);
                    }

                    song = new Song
                    {
                        Name = tag.Title,
                        Duration = tagLibFile.Properties.Duration,
                        Location = file.FullName
                    };

                    context.Songs.Add(song);
                    context.SaveChanges();
                    song = context.Songs.FirstOrDefault(f => f.Name == song.Name);
                    context.AlbumsSongEFs.Add(new AlbumSongsEF
                    {
                        AlbumID = context.AlbumEFs.FirstOrDefault(f => f.Name == albumName).ID,
                        SongID = song.ID
                    });
                    context.BandsSongEFs.Add(new BandSongsEF
                    {
                        BandID = context.BandEFs.FirstOrDefault(f => f.Name == bandName).ID,
                        SongID = song.ID
                    });
                }
            }
        }
    }
}