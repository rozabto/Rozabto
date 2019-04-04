using System.Collections.Generic;

namespace Rozabto.Model
{
    /// <summary>
    /// Съдържа лист от песните.
    /// </summary>
    public class SongList
    {
        public List<Song> Songs { get; }

        public SongList()
        {
            Songs = new List<Song>();
        }

        public SongList(List<Song> songs)
        {
            Songs = songs;
        }
    }
}
