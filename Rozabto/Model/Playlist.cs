using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
     public class Playlist
    {
        public List<int> IDsongs { get; set; }
        public List<Song> Songs { get; set; }
        public string Name { get; set; }
        public Playlist()
        {
            IDsongs = new List<int>();
            Songs = new List<Song>();

        }

    }
}
