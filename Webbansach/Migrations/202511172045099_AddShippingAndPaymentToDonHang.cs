namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShippingAndPaymentToDonHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DonHangs", "Quocgia", c => c.String(maxLength: 100));
            AddColumn("dbo.DonHangs", "Tpho", c => c.String(maxLength: 100));
            AddColumn("dbo.DonHangs", "Phuong", c => c.String(maxLength: 100));
            AddColumn("dbo.DonHangs", "ShippingMethod", c => c.String(maxLength: 100));
            AddColumn("dbo.DonHangs", "PaymentMethod", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DonHangs", "PaymentMethod");
            DropColumn("dbo.DonHangs", "ShippingMethod");
            DropColumn("dbo.DonHangs", "Phuong");
            DropColumn("dbo.DonHangs", "Tpho");
            DropColumn("dbo.DonHangs", "Quocgia");
        }
    }
}
