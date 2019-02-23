using Newtonsoft.Json;
using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
     public class Playlist
    {
        
        public List<Song> Songs { get; }
        public string Name { get; set; }
        
        public int SongsCount => Songs.Count;

        public Playlist()

        {
           
            Songs = new List<Song>();

        }
        [JsonConstructor]
        public Playlist (int[] Item1, string Item2)
        {
            Songs = MainViewModel.Collection.Songs.Where(r => Item1.Contains(r.ID)).ToList();

            this.Name = Item2;
        }
        public Playlist (string name) : this()
        {
            Name = name;
        }
    }
}
