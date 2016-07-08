namespace InventoryWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductIdaswellAsProductList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restocks", "ProductId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restocks", "ProductId");
        }
    }
}
