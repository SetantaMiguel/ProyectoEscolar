namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MejorarConceptosPago18_06_2020 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DetalleDeudors", "ConceptosPago_Id_Concepto", "dbo.ConceptosPagoes");
            DropIndex("dbo.DetalleDeudors", new[] { "ConceptosPago_Id_Concepto" });
            AlterColumn("dbo.ConceptosPagoes", "fechaMora", c => c.DateTime(nullable: false));
            DropColumn("dbo.DetalleDeudors", "ConceptosPago_Id_Concepto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DetalleDeudors", "ConceptosPago_Id_Concepto", c => c.Int());
            AlterColumn("dbo.ConceptosPagoes", "fechaMora", c => c.DateTime(nullable: false, storeType: "date"));
            CreateIndex("dbo.DetalleDeudors", "ConceptosPago_Id_Concepto");
            AddForeignKey("dbo.DetalleDeudors", "ConceptosPago_Id_Concepto", "dbo.ConceptosPagoes", "Id_Concepto");
        }
    }
}
