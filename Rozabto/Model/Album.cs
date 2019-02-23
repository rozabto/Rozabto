using Newtonsoft.Json;
using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Album
    {

        public List<Song> Songs { get; }
        public string Name { get; set; }

        [JsonIgnore]
        public int SongsCount => Songs.Count;

        public Album()
        {

            Songs = new List<Song>();
            
        }

        [JsonConstructor]
        public Album(int[] Item1, string Item2)
        {
            Songs = MainViewModel.Collection.Songs.Where(r => Item1.Contains(r.ID)).ToList();

            this.Name = Item2;
        }
        public Album(string name) : this()
        {
            Name = name;
        }


    }

}
