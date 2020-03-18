namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CursoEscolar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CursoEscolars",
                c => new
                    {
                        Id_Curso = c.Int(nullable: false, identity: true),
                        Str_Modalidad = c.String(nullable: false),
                        Bl_Estado = c.Boolean(nullable: false),
                        Id_Año = c.Int(nullable: false),
                        Id_Seccion = c.Int(nullable: false),
                        NombredeCurso= c.String(nullable:true),
                    })
                .PrimaryKey(t => t.Id_Curso)
                .ForeignKey("dbo.AniosACursars", t => t.Id_Año, cascadeDelete: true)
                .ForeignKey("dbo.Secciones", t => t.Id_Seccion, cascadeDelete: true)
                .Index(t => t.Id_Año)
                .Index(t => t.Id_Seccion);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CursoEscolars", "Id_Seccion", "dbo.Secciones");
            DropForeignKey("dbo.CursoEscolars", "Id_Año", "dbo.AniosACursars");
            DropIndex("dbo.CursoEscolars", new[] { "Id_Seccion" });
            DropIndex("dbo.CursoEscolars", new[] { "Id_Año" });
            DropTable("dbo.CursoEscolars");
        }
    }
}
