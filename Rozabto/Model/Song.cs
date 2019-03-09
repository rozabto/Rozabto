using Newtonsoft.Json;
using Rozabto.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Song
    {
        public int ID { get; }
        public string Name { get; }
        public TimeSpan Duration { get; }
        public string Location { get; }

        public static Song EmptySong = new Song();

        [JsonConstructor]
        public Song(int ID, string Name, TimeSpan Duration, string Location)
        {
            this.ID = ID;
            this.Name = Name;
            this.Duration = Duration;
            this.Location = Location;
        }

        public Song()
        {
            Name = ""; Duration = default(TimeSpan); Location = "";
        }



        [JsonIgnore]
        public string DurationString => Duration.Hours > 0 ? Duration.ToString(@"hh\:mm\:ss") : Duration.ToString(@"mm\:ss");




    }
}
