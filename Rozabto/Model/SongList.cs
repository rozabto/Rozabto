using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class SongList
    {
        public List<Song> Songs { get; }

        public SongList()
        {
            Songs = new List<Song>();
        }

        public SongList(List<Song> songs)
        {
            Songs = songs;
        }
    }
}
