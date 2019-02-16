using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Album
    {
        public List<Song> Songs { get; set; }
        public string Name { get; set; }
        
        public Album ()
        {
           Songs = new List<Song>();
         
        }

    }
    
}
