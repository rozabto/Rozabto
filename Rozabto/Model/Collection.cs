using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Collection
    {
        public List<Band> Bands { get; set; }
        public List<Album> Albums { get; set; }
        public List<Song> Songs { get; set; }
        public List<Playlist> Playlists { get; set; }

        public Collection()
        {
            Bands = new List<Band>();
            Albums = new List<Album>();
            Songs = new List<Song>();
            Playlists = new List<Playlist>();
        }
        
    }
}
