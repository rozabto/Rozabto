namespace Rozabto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedABP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumEFs", "Name", c => c.String());
            AddColumn("dbo.BandEFs", "Name", c => c.String());
            AddColumn("dbo.PlayListEFs", "Name", c => c.String());
            DropColumn("dbo.AlbumEFs", "ABPID");
            DropColumn("dbo.AlbumEFs", "SongID");
            DropColumn("dbo.BandEFs", "ABPID");
            DropColumn("dbo.BandEFs", "SongID");
            DropColumn("dbo.PlayListEFs", "ABPID");
            DropColumn("dbo.PlayListEFs", "SongID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayListEFs", "SongID", c => c.Int(nullable: false));
            AddColumn("dbo.PlayListEFs", "ABPID", c => c.Int(nullable: false));
            AddColumn("dbo.BandEFs", "SongID", c => c.Int(nullable: false));
            AddColumn("dbo.BandEFs", "ABPID", c => c.Int(nullable: false));
            AddColumn("dbo.AlbumEFs", "SongID", c => c.Int(nullable: false));
            AddColumn("dbo.AlbumEFs", "ABPID", c => c.Int(nullable: false));
            DropColumn("dbo.PlayListEFs", "Name");
            DropColumn("dbo.BandEFs", "Name");
            DropColumn("dbo.AlbumEFs", "Name");
        }
    }
}
