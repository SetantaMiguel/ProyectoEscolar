namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxEstudiantesAula1632020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Secciones", "Max_Estudiantes", c => c.Int(nullable: false));
            DropColumn("dbo.OpcionesDeColegios", "MaximoEstudiantes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpcionesDeColegios", "MaximoEstudiantes", c => c.Int(nullable: false));
            DropColumn("dbo.Secciones", "Max_Estudiantes");
        }
    }
}
