namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdfads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rotacions", "servicio", c => c.String());
            AddColumn("dbo.Rotacions", "observaciones", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rotacions", "observaciones");
            DropColumn("dbo.Rotacions", "servicio");
        }
    }
}
