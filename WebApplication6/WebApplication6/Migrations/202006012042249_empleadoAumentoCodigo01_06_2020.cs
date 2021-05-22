namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class empleadoAumentoCodigo01_06_2020 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Empleadoes", "Nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Empleadoes", "Codigo_Empleado", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Empleadoes", "Codigo_Empleado", c => c.String());
            AlterColumn("dbo.Empleadoes", "Nombre", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
