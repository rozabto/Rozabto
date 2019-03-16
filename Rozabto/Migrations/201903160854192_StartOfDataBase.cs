namespace Rozabto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StartOfDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumEFs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ABPID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AlbumSongsEFs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AlbumID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BandEFs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ABPID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BandSongsEFs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BandID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlayListEFs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ABPID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlayListSongsEFs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PlayListID = c.Int(nullable: false),
                        SongID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        Location = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Songs");
            DropTable("dbo.PlayListSongsEFs");
            DropTable("dbo.PlayListEFs");
            DropTable("dbo.BandSongsEFs");
            DropTable("dbo.BandEFs");
            DropTable("dbo.AlbumSongsEFs");
            DropTable("dbo.AlbumEFs");
        }
    }
}
