using System.Collections.Generic;

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
