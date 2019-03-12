namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLatAndLong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MarketDetails", "Longitude", c => c.String());
            AddColumn("dbo.MarketDetails", "Latitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MarketDetails", "Latitude");
            DropColumn("dbo.MarketDetails", "Longitude");
        }
    }
}
