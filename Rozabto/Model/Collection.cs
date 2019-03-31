using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{

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
