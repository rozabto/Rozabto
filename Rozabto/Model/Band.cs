using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Band : SongList
    {
        public string Name { get; }

        public Band(string name) : base()
        {
            Name = name;
        }

        public Band(string name, List<Song> songs) : base(songs)
        {
            Name = name;
        }
    }
}
