using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Album
    {
        public List<int> IDsongs { get; set; }
        public List<Song> Songs { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int SongsCount => Songs.Count;

        public Album ()
        {

           Songs = new List<Song>();
           IDsongs = new List<int>();

        }

    }
    
}
