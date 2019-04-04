using System.Collections.Generic;

namespace Rozabto.Model
{  
    /// <summary>
    /// Класът Collection съдържа базата данни, която се чете от View-то.
    /// </summary>
    public class Collection
    {
        public List<Album> Albums { get; }
        public List<Band> Bands { get; }
        public List<PlayList> PlayLists { get; }
        public List<Song> Songs { get; }

        public Collection(List<Album> albums, List<Band> bands, List<PlayList> playlists, List<Song> songs)
        {
            Albums = albums;
            Bands = bands;
            PlayLists = playlists;
            Songs = songs;
        }
    }
}
