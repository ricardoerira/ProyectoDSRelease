namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Docentes", "Rotacion_rotacionId", "dbo.Rotacions");
            DropForeignKey("dbo.Estudiantes", "Rotacion_rotacionId", "dbo.Rotacions");
            DropIndex("dbo.Docentes", new[] { "Rotacion_rotacionId" });
            DropIndex("dbo.Estudiantes", new[] { "Rotacion_rotacionId" });
            DropColumn("dbo.Docentes", "Rotacion_rotacionId");
            DropColumn("dbo.Estudiantes", "Rotacion_rotacionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Estudiantes", "Rotacion_rotacionId", c => c.Int());
            AddColumn("dbo.Docentes", "Rotacion_rotacionId", c => c.Int());
            CreateIndex("dbo.Estudiantes", "Rotacion_rotacionId");
            CreateIndex("dbo.Docentes", "Rotacion_rotacionId");
            AddForeignKey("dbo.Estudiantes", "Rotacion_rotacionId", "dbo.Rotacions", "rotacionId");
            AddForeignKey("dbo.Docentes", "Rotacion_rotacionId", "dbo.Rotacions", "rotacionId");
        }
    }
}
