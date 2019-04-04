using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rozabto.Model
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; }
        public float Volume { get; set; }
        public static Song EmptySong = new Song();

        public Song()
        {
            Name = "";
            Duration = default(TimeSpan);
            Location = "";
            Volume = 1;
        }
        [NotMapped]
        public string DurationString => Duration.Hours > 0 ? Duration.ToString(@"hh\:mm\:ss") : Duration.ToString(@"mm\:ss");
    }
}
