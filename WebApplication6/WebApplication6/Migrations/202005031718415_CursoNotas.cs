namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CursoNotas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notas", "Id_CursoEscolar", c => c.Int(nullable: false));
            CreateIndex("dbo.Notas", "Id_CursoEscolar");
            AddForeignKey("dbo.Notas", "Id_CursoEscolar", "dbo.CursoEscolars", "Id_Curso", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notas", "Id_CursoEscolar", "dbo.CursoEscolars");
            DropIndex("dbo.Notas", new[] { "Id_CursoEscolar" });
            DropColumn("dbo.Notas", "Id_CursoEscolar");
        }
    }
}
    