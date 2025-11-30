namespace Webbansach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateAtToSach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saches", "CreateAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saches", "CreateAt");
        }
    }
}
