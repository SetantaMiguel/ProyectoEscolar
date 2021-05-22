namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nota30_04_2020 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notas", "Nota", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notas", "Nota");
        }
    }
}
