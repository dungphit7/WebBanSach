namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStudentIdAndUniversityToDonHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DonHangs", "StudentId", c => c.String());
            AddColumn("dbo.DonHangs", "University", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DonHangs", "University");
            DropColumn("dbo.DonHangs", "StudentId");
        }
    }
}
