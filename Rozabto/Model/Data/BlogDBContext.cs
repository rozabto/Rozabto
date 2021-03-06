﻿using System.Data.Entity;

namespace Rozabto.Model.Data
{
    /// <summary>
    /// Връзка със SQL базата данни на плеъра.
    /// </summary>
    public class BlogDBContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<AlbumEF> Albums { get; set; }
        public DbSet<AlbumSongsEF> AlbumsSongs { get; set; }
        public DbSet<BandEF> Bands { get; set; }
        public DbSet<BandSongsEF> BandsSongs { get; set; }
        public DbSet<PlayListEF> PlayLists { get; set; }
        public DbSet<PlayListSongsEF> PlayListsSongs { get; set; }
    }
}
