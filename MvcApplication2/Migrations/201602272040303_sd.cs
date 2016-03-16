namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Docentes", "rotacionId", "dbo.Rotacions");
            DropIndex("dbo.Docentes", new[] { "rotacionId" });
            RenameColumn(table: "dbo.Docentes", name: "rotacionId", newName: "Rotacion_rotacionId");
            AlterColumn("dbo.Docentes", "Rotacion_rotacionId", c => c.Int());
            CreateIndex("dbo.Docentes", "Rotacion_rotacionId");
            AddForeignKey("dbo.Docentes", "Rotacion_rotacionId", "dbo.Rotacions", "rotacionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Docentes", "Rotacion_rotacionId", "dbo.Rotacions");
            DropIndex("dbo.Docentes", new[] { "Rotacion_rotacionId" });
            AlterColumn("dbo.Docentes", "Rotacion_rotacionId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Docentes", name: "Rotacion_rotacionId", newName: "rotacionId");
            CreateIndex("dbo.Docentes", "rotacionId");
            AddForeignKey("dbo.Docentes", "rotacionId", "dbo.Rotacions", "rotacionId", cascadeDelete: true);
        }
    }
}
