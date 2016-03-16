namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RotacionEstudiantes", "Docente_docenteId", "dbo.Docentes");
            DropIndex("dbo.RotacionEstudiantes", new[] { "Docente_docenteId" });
            AddColumn("dbo.RotacionDocentes", "nombre", c => c.String());
            DropColumn("dbo.RotacionEstudiantes", "Docente_docenteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RotacionEstudiantes", "Docente_docenteId", c => c.Int());
            DropColumn("dbo.RotacionDocentes", "nombre");
            CreateIndex("dbo.RotacionEstudiantes", "Docente_docenteId");
            AddForeignKey("dbo.RotacionEstudiantes", "Docente_docenteId", "dbo.Docentes", "docenteId");
        }
    }
}
