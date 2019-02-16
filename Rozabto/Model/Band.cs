using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Band
    {
        public List<Song> Songs { get; set; }
        public string Name { get; set; }
       

        public Band ()
        {
            Songs = new List<Song>();
        }
    }
}
