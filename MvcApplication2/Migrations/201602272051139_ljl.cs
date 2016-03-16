namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ljl : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Estudiantes", "rotacionId", "dbo.Rotacions");
            DropIndex("dbo.Estudiantes", new[] { "rotacionId" });
            RenameColumn(table: "dbo.Estudiantes", name: "rotacionId", newName: "Rotacion_rotacionId");
            AlterColumn("dbo.Estudiantes", "Rotacion_rotacionId", c => c.Int());
            CreateIndex("dbo.Estudiantes", "Rotacion_rotacionId");
            AddForeignKey("dbo.Estudiantes", "Rotacion_rotacionId", "dbo.Rotacions", "rotacionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Estudiantes", "Rotacion_rotacionId", "dbo.Rotacions");
            DropIndex("dbo.Estudiantes", new[] { "Rotacion_rotacionId" });
            AlterColumn("dbo.Estudiantes", "Rotacion_rotacionId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Estudiantes", name: "Rotacion_rotacionId", newName: "rotacionId");
            CreateIndex("dbo.Estudiantes", "rotacionId");
            AddForeignKey("dbo.Estudiantes", "rotacionId", "dbo.Rotacions", "rotacionId", cascadeDelete: true);
        }
    }
}
