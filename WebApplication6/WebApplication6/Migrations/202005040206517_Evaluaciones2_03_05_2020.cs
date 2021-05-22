namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Evaluaciones2_03_05_2020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Evaluaciones", "Id_Parciales", c => c.Int(nullable: false));
            CreateIndex("dbo.Evaluaciones", "Id_Parciales");
            AddForeignKey("dbo.Evaluaciones", "Id_Parciales", "dbo.Parciales", "Id_Parcial", cascadeDelete: true);
            DropColumn("dbo.Evaluaciones", "Num_Evaluacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Evaluaciones", "Num_Evaluacion", c => c.Int(nullable: false));
            DropForeignKey("dbo.Evaluaciones", "Id_Parciales", "dbo.Parciales");
            DropIndex("dbo.Evaluaciones", new[] { "Id_Parciales" });
            DropColumn("dbo.Evaluaciones", "Id_Parciales");
        }
    }
}
