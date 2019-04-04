namespace Rozabto.Model
{
    /// <summary>
    /// AlbumSongsEF съдържа песните, които после се дават на Album класът.
    /// </summary>
    public class AlbumSongsEF
    {
        public int ID { get; set; }
        public int AlbumID { get; set; }
        public int SongID { get; set; }
    }
}
