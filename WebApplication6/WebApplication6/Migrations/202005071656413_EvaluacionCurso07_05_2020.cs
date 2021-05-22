namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluacionCurso07_05_2020 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Evaluaciones", "Id_CursoA", "dbo.Curso_Asignaturas");
            DropIndex("dbo.Evaluaciones", new[] { "Id_CursoA" });
            AddColumn("dbo.Evaluaciones", "Id_Curso", c => c.Int(nullable: false));
            CreateIndex("dbo.Evaluaciones", "Id_Curso");
            AddForeignKey("dbo.Evaluaciones", "Id_Curso", "dbo.CursoEscolars", "Id_Curso", cascadeDelete: false);
            DropColumn("dbo.Evaluaciones", "Id_CursoA");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Evaluaciones", "Id_CursoA", c => c.Int(nullable: false));
            DropForeignKey("dbo.Evaluaciones", "Id_Curso", "dbo.CursoEscolars");
            DropIndex("dbo.Evaluaciones", new[] { "Id_Curso" });
            DropColumn("dbo.Evaluaciones", "Id_Curso");
            CreateIndex("dbo.Evaluaciones", "Id_CursoA");
            AddForeignKey("dbo.Evaluaciones", "Id_CursoA", "dbo.Curso_Asignaturas", "Id_Curso_Asignatura", cascadeDelete: true);
        }
    }
}
