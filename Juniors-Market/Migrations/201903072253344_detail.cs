namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarketDetails",
                c => new
                    {
                        Address = c.String(nullable: false, maxLength: 128),
                        GoogleLink = c.String(),
                        Products = c.String(),
                        Schedule = c.String(),
                        marketId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Address)
                .ForeignKey("dbo.MarketSearches", t => t.marketId)
                .Index(t => t.marketId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MarketDetails", "marketId", "dbo.MarketSearches");
            DropIndex("dbo.MarketDetails", new[] { "marketId" });
            DropTable("dbo.MarketDetails");
        }
    }
}
