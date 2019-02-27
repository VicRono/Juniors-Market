namespace Juniors_Market.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShoppingCarts", "ProduceId", "dbo.Produces");
            DropIndex("dbo.ShoppingCarts", new[] { "ProduceId" });
            AddColumn("dbo.Produces", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Produces", "ShoppingCart_ShoppingCartId", c => c.Int());
            CreateIndex("dbo.Produces", "ShoppingCart_ShoppingCartId");
            AddForeignKey("dbo.Produces", "ShoppingCart_ShoppingCartId", "dbo.ShoppingCarts", "ShoppingCartId");
            DropColumn("dbo.ShoppingCarts", "ProduceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShoppingCarts", "ProduceId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Produces", "ShoppingCart_ShoppingCartId", "dbo.ShoppingCarts");
            DropIndex("dbo.Produces", new[] { "ShoppingCart_ShoppingCartId" });
            DropColumn("dbo.Produces", "ShoppingCart_ShoppingCartId");
            DropColumn("dbo.Produces", "Quantity");
            CreateIndex("dbo.ShoppingCarts", "ProduceId");
            AddForeignKey("dbo.ShoppingCarts", "ProduceId", "dbo.Produces", "ProduceId", cascadeDelete: true);
        }
    }
}
