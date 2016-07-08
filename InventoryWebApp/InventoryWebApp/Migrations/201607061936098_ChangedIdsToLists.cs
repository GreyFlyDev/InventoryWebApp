namespace InventoryWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedIdsToLists : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RestockProducts",
                c => new
                    {
                        Restock_RestockId = c.Int(nullable: false),
                        Product_ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Restock_RestockId, t.Product_ProductId })
                .ForeignKey("dbo.Restocks", t => t.Restock_RestockId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ProductId, cascadeDelete: true)
                .Index(t => t.Restock_RestockId)
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "dbo.SalesProducts",
                c => new
                    {
                        Sales_SalesId = c.Int(nullable: false),
                        Product_ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Sales_SalesId, t.Product_ProductId })
                .ForeignKey("dbo.Sales", t => t.Sales_SalesId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ProductId, cascadeDelete: true)
                .Index(t => t.Sales_SalesId)
                .Index(t => t.Product_ProductId);
            
            DropColumn("dbo.Restocks", "ProductId");
            DropColumn("dbo.Sales", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Restocks", "ProductId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SalesProducts", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.SalesProducts", "Sales_SalesId", "dbo.Sales");
            DropForeignKey("dbo.RestockProducts", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.RestockProducts", "Restock_RestockId", "dbo.Restocks");
            DropIndex("dbo.SalesProducts", new[] { "Product_ProductId" });
            DropIndex("dbo.SalesProducts", new[] { "Sales_SalesId" });
            DropIndex("dbo.RestockProducts", new[] { "Product_ProductId" });
            DropIndex("dbo.RestockProducts", new[] { "Restock_RestockId" });
            DropTable("dbo.SalesProducts");
            DropTable("dbo.RestockProducts");
        }
    }
}
