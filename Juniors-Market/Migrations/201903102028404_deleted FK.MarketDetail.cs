namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedFKMarketDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MarketDetails", "MarketId", "dbo.MarketSearches");
            DropIndex("dbo.MarketDetails", new[] { "MarketId" });
            DropColumn("dbo.MarketDetails", "MarketId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MarketDetails", "MarketId", c => c.String(maxLength: 128));
            CreateIndex("dbo.MarketDetails", "MarketId");
            AddForeignKey("dbo.MarketDetails", "MarketId", "dbo.MarketSearches", "SearchId");
        }
    }
}
