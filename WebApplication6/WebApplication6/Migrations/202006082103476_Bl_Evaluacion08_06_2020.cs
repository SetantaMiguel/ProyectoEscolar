namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bl_Evaluacion08_06_2020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EvaluacionesEstudiantes", "EstadoEvaluado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EvaluacionesEstudiantes", "EstadoEvaluado");
        }
    }
}
