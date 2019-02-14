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

       //JasonIgnore public string duration => duration.ToString

       

    }
}
