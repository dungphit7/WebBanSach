namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookstoreTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietDonHangs",
                c => new
                    {
                        MaCTDH = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        DonGia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaDonHang = c.Int(nullable: false),
                        MaSach = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaCTDH)
                .ForeignKey("dbo.DonHangs", t => t.MaDonHang, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.MaSach, cascadeDelete: true)
                .Index(t => t.MaDonHang)
                .Index(t => t.MaSach);
            
            CreateTable(
                "dbo.DonHangs",
                c => new
                    {
                        MaDonHang = c.Int(nullable: false, identity: true),
                        NgayDat = c.DateTime(),
                        TongTien = c.Decimal(precision: 18, scale: 2),
                        TrangThai = c.String(maxLength: 50),
                        DiaChiNhan = c.String(maxLength: 255),
                        SdtNhan = c.String(maxLength: 15),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaDonHang)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Saches",
                c => new
                    {
                        MaSach = c.Int(nullable: false, identity: true),
                        TenSach = c.String(nullable: false, maxLength: 255),
                        MoTa = c.String(storeType: "ntext"),
                        HinhAnh = c.String(maxLength: 255),
                        GiaBan = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SoLuongTon = c.Int(nullable: false),
                        MaTheLoai = c.Int(),
                        MaTacGia = c.Int(),
                        MaNXB = c.Int(),
                    })
                .PrimaryKey(t => t.MaSach)
                .ForeignKey("dbo.NhaXuatBans", t => t.MaNXB)
                .ForeignKey("dbo.TacGias", t => t.MaTacGia)
                .ForeignKey("dbo.TheLoais", t => t.MaTheLoai)
                .Index(t => t.MaTheLoai)
                .Index(t => t.MaTacGia)
                .Index(t => t.MaNXB);
            
            CreateTable(
                "dbo.NhaXuatBans",
                c => new
                    {
                        MaNXB = c.Int(nullable: false, identity: true),
                        TenNXB = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.MaNXB);
            
            CreateTable(
                "dbo.TacGias",
                c => new
                    {
                        MaTacGia = c.Int(nullable: false, identity: true),
                        TenTacGia = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.MaTacGia);
            
            CreateTable(
                "dbo.TheLoais",
                c => new
                    {
                        MaTheLoai = c.Int(nullable: false, identity: true),
                        TenTheLoai = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.MaTheLoai);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Saches", "MaTheLoai", "dbo.TheLoais");
            DropForeignKey("dbo.Saches", "MaTacGia", "dbo.TacGias");
            DropForeignKey("dbo.Saches", "MaNXB", "dbo.NhaXuatBans");
            DropForeignKey("dbo.ChiTietDonHangs", "MaSach", "dbo.Saches");
            DropForeignKey("dbo.ChiTietDonHangs", "MaDonHang", "dbo.DonHangs");
            DropForeignKey("dbo.DonHangs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Saches", new[] { "MaNXB" });
            DropIndex("dbo.Saches", new[] { "MaTacGia" });
            DropIndex("dbo.Saches", new[] { "MaTheLoai" });
            DropIndex("dbo.DonHangs", new[] { "UserId" });
            DropIndex("dbo.ChiTietDonHangs", new[] { "MaSach" });
            DropIndex("dbo.ChiTietDonHangs", new[] { "MaDonHang" });
            DropTable("dbo.TheLoais");
            DropTable("dbo.TacGias");
            DropTable("dbo.NhaXuatBans");
            DropTable("dbo.Saches");
            DropTable("dbo.DonHangs");
            DropTable("dbo.ChiTietDonHangs");
        }
    }
}
