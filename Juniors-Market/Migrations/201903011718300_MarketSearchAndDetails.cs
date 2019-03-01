namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarketSearchAndDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarketSearches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Marketname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MarketSearches");
        }
    }
}
