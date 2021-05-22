namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluacionCurso07_05_2020_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Curso_Asignaturas", "Estado_Asignatura", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Curso_Asignaturas", "Estado_Asignatura");
        }
    }
}
