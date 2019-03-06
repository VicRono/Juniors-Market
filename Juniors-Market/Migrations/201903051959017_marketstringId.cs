namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marketstringId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MarketSearches");
            AlterColumn("dbo.MarketSearches", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.MarketSearches", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MarketSearches");
            AlterColumn("dbo.MarketSearches", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.MarketSearches", "Id");
        }
    }
}
