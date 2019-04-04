namespace Rozabto.Model
{
    /// <summary>
    /// PlayListSongEF съдържа песните, които после се дават на Playlist класът.
    /// </summary>
    public class PlayListSongsEF
    {
        public int ID { get; set; }
        public int PlayListID { get; set; }
        public int SongID { get; set; }
    }
}
