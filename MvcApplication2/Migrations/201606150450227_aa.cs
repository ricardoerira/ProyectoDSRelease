namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RotacionEstudianteDetalles", "docentes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RotacionEstudianteDetalles", "docentes");
        }
    }
}
