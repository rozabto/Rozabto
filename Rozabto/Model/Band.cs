using System.Collections.Generic;

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
