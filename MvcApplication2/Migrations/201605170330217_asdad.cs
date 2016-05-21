namespace MvcApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RotacionEstudianteDetalles",
                c => new
                    {
                        rotacionEstudianteDetalleId = c.Int(nullable: false, identity: true),
                        rotacionEstudianteId = c.Int(nullable: false),
                        IPS_ESEId = c.Int(nullable: false),
                        horario = c.String(),
                        servicio = c.String(),
                        fecha_inicio = c.DateTime(nullable: false),
                        fecha_terminacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.rotacionEstudianteDetalleId)
                .ForeignKey("dbo.IPS_ESE", t => t.IPS_ESEId, cascadeDelete: true)
                .ForeignKey("dbo.RotacionEstudiantes", t => t.rotacionEstudianteId, cascadeDelete: false)
                .Index(t => t.IPS_ESEId)
                .Index(t => t.rotacionEstudianteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RotacionEstudianteDetalles", "rotacionEstudianteId", "dbo.RotacionEstudiantes");
            DropForeignKey("dbo.RotacionEstudianteDetalles", "IPS_ESEId", "dbo.IPS_ESE");
            DropIndex("dbo.RotacionEstudianteDetalles", new[] { "rotacionEstudianteId" });
            DropIndex("dbo.RotacionEstudianteDetalles", new[] { "IPS_ESEId" });
            DropTable("dbo.RotacionEstudianteDetalles");
        }
    }
}
