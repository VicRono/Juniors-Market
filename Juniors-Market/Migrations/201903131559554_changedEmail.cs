namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Emails", "To", c => c.String());
            AddColumn("dbo.Emails", "From", c => c.String());
            DropColumn("dbo.Emails", "ToLine");
            DropColumn("dbo.Emails", "FromLine");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Emails", "FromLine", c => c.String());
            AddColumn("dbo.Emails", "ToLine", c => c.String());
            DropColumn("dbo.Emails", "From");
            DropColumn("dbo.Emails", "To");
        }
    }
}
