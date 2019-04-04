namespace Rozabto.Model
{
    /// <summary>
    /// BandSongsEF съдържа песните, които после се дават на Band класът.
    /// </summary>
    public class BandSongsEF
    {
        public int ID { get; set; }
        public int BandID { get; set; }
        public int SongID { get; set; }
    }
}
