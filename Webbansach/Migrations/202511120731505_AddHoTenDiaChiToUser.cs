namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHoTenDiaChiToUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DonHangs", new[] { "UserId" });
            AddColumn("dbo.AspNetUsers", "HoTen", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "DiaChi", c => c.String(maxLength: 255));
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DonHangs", "UserId");
            AddForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DonHangs", new[] { "UserId" });
            AlterColumn("dbo.DonHangs", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.AspNetUsers", "DiaChi");
            DropColumn("dbo.AspNetUsers", "HoTen");
            CreateIndex("dbo.DonHangs", "UserId");
            AddForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
