using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Album : SongList
    {
        public string Name { get; }

        public Album(string name) : base()
        {
            Name = name;
        }

        public Album(string name, List<Song> songs) : base(songs)
        {
            Name = name;
        }
    }
}
