namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MarketSearches");
            AddColumn("dbo.MarketDetails", "SearchId", c => c.Int(nullable: false));
            AddColumn("dbo.MarketSearches", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.MarketSearches", "SearchId", c => c.String());
            AddPrimaryKey("dbo.MarketSearches", "Id");
            CreateIndex("dbo.MarketDetails", "SearchId");
            AddForeignKey("dbo.MarketDetails", "SearchId", "dbo.MarketSearches", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MarketDetails", "SearchId", "dbo.MarketSearches");
            DropIndex("dbo.MarketDetails", new[] { "SearchId" });
            DropPrimaryKey("dbo.MarketSearches");
            AlterColumn("dbo.MarketSearches", "SearchId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.MarketSearches", "Id");
            DropColumn("dbo.MarketDetails", "SearchId");
            AddPrimaryKey("dbo.MarketSearches", "SearchId");
        }
    }
}
