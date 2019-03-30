namespace Rozabto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Volume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "Volume", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "Volume");
        }
    }
}
