using System.Collections.Generic;

namespace Rozabto.Model
{
    public class PlayList : SongList
    {
        public string Name { get; }

        public PlayList(string name) : base()
        {
            Name = name;
        }

        public PlayList(string name, List<Song> songs) : base(songs)
        {
            Name = name;
        }
    }
}
