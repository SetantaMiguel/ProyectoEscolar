namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Evaluaciones03_05_2020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEvaluacions", "Valor", c => c.Int(nullable: false));
            DropColumn("dbo.Evaluaciones", "ValorTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Evaluaciones", "ValorTotal", c => c.Int(nullable: false));
            DropColumn("dbo.TipoEvaluacions", "Valor");
        }
    }
}
