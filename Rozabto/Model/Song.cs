using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Song
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; }
        public static Song EmptySong = new Song { Name = "", Duration = default(TimeSpan), Location = "" };

        [JsonIgnore]
        public string DurationString => Duration.Hours > 0 ? Duration.ToString(@"hh\:mm\:ss") : Duration.ToString(@"mm\:ss");



    }
}
