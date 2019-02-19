using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Band
    {
        public List<int> IDsongs { get; set; }
        public List<Song> Songs { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int SongsCount => Songs.Count;

        public Band ()
        {
            IDsongs = new List<int>();
            Songs = new List<Song>();
        }
    }
}
