﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model
{
    /// <summary>
    /// PlayListEF съдържа плейлистите, които после се дават на Playlist класът.
    /// </summary>
    public class PlayListEF
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}