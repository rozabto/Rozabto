using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Album
    {
        public List<int> IDsongs { get; set; }
        public string Name { get; set; }
        
        public Album ()
        {
            IDsongs = new List<int>();
         
        }

    }
    
}
