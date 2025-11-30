namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTenNguoiNhanToDonHang : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DonHangs", new[] { "UserId" });
            AddColumn("dbo.DonHangs", "TenNguoiNhan", c => c.String(maxLength: 100));
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DonHangs", "UserId");
            AddForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DonHangs", new[] { "UserId" });
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.DonHangs", "TenNguoiNhan");
            CreateIndex("dbo.DonHangs", "UserId");
            AddForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
