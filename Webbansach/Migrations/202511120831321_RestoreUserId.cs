namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestoreUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DonHangs", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DonHangs", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.DonHangs", "UserId");
            RenameColumn(table: "dbo.DonHangs", name: "ApplicationUser_Id", newName: "UserId");
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(nullable: true, maxLength: 128));
          
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DonHangs", "UserId");
            AddForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.DonHangs", "TenKhachHang");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DonHangs", "TenKhachHang", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DonHangs", new[] { "UserId" });
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(nullable: false));
            RenameColumn(table: "dbo.DonHangs", name: "UserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.DonHangs", "UserId", c => c.String(nullable: false));
            CreateIndex("dbo.DonHangs", "ApplicationUser_Id");
            AddForeignKey("dbo.DonHangs", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
