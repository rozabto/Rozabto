using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class AlbumSongsEF
    {
        public int ID { get; set; }
        public int AlbumID { get; set; }
        public int SongID { get; set; }
    }
}
