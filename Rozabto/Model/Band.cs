﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    public class Band
    {
        public List<int> IDsongs { get; set; }
        public string Name { get; set; }
        public object songs { get; internal set; }

        public Band ()
        {
            IDsongs = new List<int>();
        }
    }
}
