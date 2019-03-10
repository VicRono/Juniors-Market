namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Key : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MarketDetails");
            AddColumn("dbo.MarketDetails", "id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.MarketDetails", "Address", c => c.String());
            AddPrimaryKey("dbo.MarketDetails", "id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MarketDetails");
            AlterColumn("dbo.MarketDetails", "Address", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.MarketDetails", "id");
            AddPrimaryKey("dbo.MarketDetails", "Address");
        }
    }
}
