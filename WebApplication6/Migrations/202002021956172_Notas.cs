namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
         "dbo.Curso_Asignaturas",
         c => new
         {
             Id_Curso_Asignatura = c.Int(nullable: false, identity: true),
             Id_Curso = c.Int(nullable: false),
             Id_Materia = c.Int(nullable: false),
             Id_Empleado = c.Int(nullable: false)
         })
         .PrimaryKey(t => t.Id_Curso_Asignatura)
        .ForeignKey("dbo.CursoEscolars", t => t.Id_Curso, cascadeDelete: true)
        .ForeignKey("dbo.Materias", t => t.Id_Materia, cascadeDelete: true)
        .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true);

            CreateTable(
                "dbo.Notas",
                c => new
                    {
                        Id_Nota = c.Int(nullable: false, identity: true),
                        Id_Estudiante = c.Int(nullable: false),
                        Id_Curso = c.Int(nullable: false),
                        Nota1 = c.Single(nullable: false),
                        Nota2 = c.Single(nullable: false),
                        Nota3 = c.Single(nullable: false),
                        Nota4 = c.Single(nullable: false),
                        promedio = c.Single(nullable: false),
                        Bl_Aprobado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Nota)
                .ForeignKey("dbo.Curso_Asignaturas", t => t.Id_Curso, cascadeDelete: true)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: false)
                .Index(t => t.Id_Estudiante)
                .Index(t => t.Id_Curso);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notas", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.Notas", "Id_Curso", "dbo.Curso_Asignaturas");
            DropIndex("dbo.Notas", new[] { "Id_Curso" });
            DropIndex("dbo.Notas", new[] { "Id_Estudiante" });
            DropTable("dbo.Notas");
        }
    }
}
