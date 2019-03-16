using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class PlayListSongsEF
    {
        public int ID { get; set; }
        public int PlayListID { get; set; }
        public int SongID { get; set; }
    }
}
