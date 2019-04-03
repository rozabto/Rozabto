using NAudio.Wave;
using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using TagFile = TagLib.File;

namespace Rozabto.Model.Data {
    /// <summary>
    /// Класът MusicInformation извлича всички песни и ги превръща в код.
    /// </summary>
    public class MusicInformation {
        public async Task SearchMusic(string path) {
            var context = new BlogDBContext();

            var file = new FileInfo(path);
            TagFile tagLibFile = null;
            try {
                // Прочитаме музикалния файл.
                tagLibFile = TagFile.Create(path);
            } catch (Exception) {
                // Ако не можем да го прочетем отиваме на следващия.
                return;
            }
            using (tagLibFile) {
                BandEF band = null;
                Song song = null;
                AlbumEF album = null;
                var tag = tagLibFile.Tag;
                // Взимаме името на бандата.
                var bandName = tag.FirstAlbumArtist ?? tag.FirstPerformer ?? tag.FirstComposer ?? "Unknown";

                // Ако tag.Title е null взимаме името на файла.           
                if (tag.Title is null)
                    tag.Title = Path.GetFileNameWithoutExtension(file.Name);
                //  Взимаме името на албума.
                var albumName = tag.Album ?? "Unknown";
                song = context.Songs.FirstOrDefault(s => s.Name == tag.Title);

                // Опитваме да видим дали има песен със същото име.
                // Ако има такава песен отиваме на следващата.
                if (song != null)
                    return;

                // Ако няма банда с това име, създаваме нова.
                band = context.Bands.FirstOrDefault(f => f.Name == bandName);
                if (band is null) {
                    band = new BandEF {
                        Name = bandName
                    };
                    MainViewModel.Collection.Bands.Add(new Band(bandName));
                    context.Bands.Add(band);
                }
                // Ако няма албум с това име, създаваме нов.
                album = context.Albums.FirstOrDefault(f => f.Name == albumName);
                if (album is null) {
                    album = new AlbumEF {
                        Name = albumName
                    };
                    MainViewModel.Collection.Albums.Add(new Album(albumName));
                    context.Albums.Add(album);
                }

                //Създаваме песента с нейната информация.
                song = new Song {
                    Name = tag.Title,
                    Duration = tagLibFile.Properties.Duration,
                    Location = file.FullName,
                    Volume = GetSongVolume(path)
                };
                MainViewModel.Collection.Songs.Add(song);

                // Добавяме песента към SQL базата данни.                  
                context.Songs.Add(song);
                // Запазваме песента.
                await context.SaveChangesAsync();
                // SQL създава ID на песента и я взимаме обратно.
                song = context.Songs.FirstOrDefault(f => f.Name == song.Name);
                // Добавяме албум с тази песен.
                context.AlbumsSongs.Add(new AlbumSongsEF {
                    AlbumID = context.Albums.FirstOrDefault(f => f.Name == albumName).ID,
                    SongID = song.ID
                });
                // Добавяме банда с тази песен.
                context.BandsSongs.Add(new BandSongsEF {
                    BandID = context.Bands.FirstOrDefault(f => f.Name == bandName).ID,
                    SongID = song.ID
                });
                await context.SaveChangesAsync();
            }

        }

        /// <summary>
        /// Получава мястото на песента и прочита силата на звука. След това връща силата на звука.
        /// </summary>
        public float GetSongVolume(string path) {
            // Ако AudioFileReader не може да разпознае формата хвърля exception.
            try {
                float max = 0;
                // Четем файла.
                using (var reader = new AudioFileReader(path)) {
                    // Взимаме дължината на песента и създаваме масив.
                    float[] buffer = new float[reader.WaveFormat.SampleRate];
                    int read;
                    do {
                        // Четем песента.
                        read = reader.Read(buffer, 0, buffer.Length);
                        for (int n = 0; n < read; n++) {
                            // Превръщаме звукът във float число от 0 до 1.
                            var abs = Math.Abs(buffer[n]);
                            // Ако числото е над 0.95 го връщаме като 1.
                            // Правим го, за да спестим време да не минаваме през цялата песен.
                            if (abs > 0.95) return 1;
                            if (abs > max) max = abs;
                        }
                    } while (read > 0);
                }
                return max;
            } catch {
                return 1;
            }
        }
    }
}