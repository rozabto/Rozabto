using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozabto.Model.Data
{
    public class BlogDBContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<AlbumEF> AlbumEFs { get; set; }
        public DbSet<AlbumSongsEF> AlbumsSongEFs { get; set; }
        public DbSet<BandEF> BandEFs { get; set; }
        public DbSet<BandSongsEF> BandsSongEFs { get; set; }
        public DbSet<PlayListEF> PlayListEFs { get; set; }
        public DbSet<PlayListSongsEF> PlayListsSongEFs { get; set; }
    }
}
