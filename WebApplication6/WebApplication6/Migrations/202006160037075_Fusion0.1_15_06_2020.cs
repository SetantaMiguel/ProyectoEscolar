namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fusion01_15_06_2020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AperturaCajas",
                c => new
                    {
                        IdApertura = c.Int(nullable: false, identity: true),
                        IdCaja = c.Int(nullable: false),
                        IdCajero = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        TotalCordoba = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDolar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.Boolean(nullable: false),
                        Cartera_IdCartera = c.Int(),
                    })
                .PrimaryKey(t => t.IdApertura)
                .ForeignKey("dbo.Cajas", t => t.IdCaja, cascadeDelete: true)
                .ForeignKey("dbo.Cajeroes", t => t.IdCajero, cascadeDelete: true)
                .ForeignKey("dbo.Carteras", t => t.Cartera_IdCartera)
                .Index(t => t.IdCaja)
                .Index(t => t.IdCajero)
                .Index(t => t.Cartera_IdCartera);
            
            CreateTable(
                "dbo.Cajas",
                c => new
                    {
                        IdCaja = c.Int(nullable: false, identity: true),
                        NumCaja = c.String(),
                    })
                .PrimaryKey(t => t.IdCaja);
            
            CreateTable(
                "dbo.Facturas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IdConcepto = c.Int(nullable: false),
                        FechaFactura = c.DateTime(nullable: false),
                        IdCajero = c.Int(nullable: false),
                        IdCaja = c.Int(nullable: false),
                        IdEstudiante = c.String(),
                        Id_Tipo = c.Int(nullable: false),
                        Monto = c.Decimal(precision: 18, scale: 2),
                        IdMoneda = c.Int(nullable: false),
                        PagoAbonado = c.Decimal(precision: 18, scale: 2),
                        IdEquivalencia = c.Decimal(precision: 18, scale: 2),
                        Cambio = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Mora = c.Decimal(precision: 18, scale: 2),
                        Conceptos_Id_Concepto = c.Int(),
                        Moneda_Id_Moneda = c.Int(),
                        TipoPago_Id_TipoPago = c.Int(),
                        Cartera_IdCartera = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.IdConcepto })
                .ForeignKey("dbo.Cajas", t => t.IdCaja, cascadeDelete: true)
                .ForeignKey("dbo.Cajeroes", t => t.IdCajero, cascadeDelete: true)
                .ForeignKey("dbo.ConceptosPagoes", t => t.Conceptos_Id_Concepto)
                .ForeignKey("dbo.Monedas", t => t.Moneda_Id_Moneda)
                .ForeignKey("dbo.TipoPagoes", t => t.TipoPago_Id_TipoPago)
                .ForeignKey("dbo.Carteras", t => t.Cartera_IdCartera)
                .Index(t => t.IdCajero)
                .Index(t => t.IdCaja)
                .Index(t => t.Conceptos_Id_Concepto)
                .Index(t => t.Moneda_Id_Moneda)
                .Index(t => t.TipoPago_Id_TipoPago)
                .Index(t => t.Cartera_IdCartera);
            
            CreateTable(
                "dbo.Cajeroes",
                c => new
                    {
                        IdCajero = c.Int(nullable: false, identity: true),
                        NumCajero = c.Int(nullable: false),
                        codigo = c.String(),
                        Id_Empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCajero)
                .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.ConceptosPagoes",
                c => new
                    {
                        Id_Concepto = c.Int(nullable: false, identity: true),
                        NombreConcepto = c.String(),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fechaMora = c.DateTime(nullable: false, storeType: "date"),
                        DetalleDeudor_IdDeudor = c.Int(),
                        DetalleDeudor_IdConceptop = c.Int(),
                        DetalleDeudor_IdDeudor1 = c.Int(),
                        DetalleDeudor_IdConceptop1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Concepto)
                .ForeignKey("dbo.DetalleDeudors", t => new { t.DetalleDeudor_IdDeudor, t.DetalleDeudor_IdConceptop })
                .ForeignKey("dbo.DetalleDeudors", t => new { t.DetalleDeudor_IdDeudor1, t.DetalleDeudor_IdConceptop1 })
                .Index(t => new { t.DetalleDeudor_IdDeudor, t.DetalleDeudor_IdConceptop })
                .Index(t => new { t.DetalleDeudor_IdDeudor1, t.DetalleDeudor_IdConceptop1 });
            
            CreateTable(
                "dbo.DetalleDeudors",
                c => new
                    {
                        IdDeudor = c.Int(nullable: false),
                        IdConceptop = c.Int(nullable: false),
                        Total_Pagado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Deber = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdEstado = c.Int(nullable: false),
                        DetalleDeudor_IdDeudor = c.Int(),
                        DetalleDeudor_IdConceptop = c.Int(),
                        Estado_Id_Estado = c.Int(),
                        Deudores_Id_Deudor = c.Int(),
                        TipoConcepto_Id_Concepto = c.Int(),
                        ConceptosPago_Id_Concepto = c.Int(),
                    })
                .PrimaryKey(t => new { t.IdDeudor, t.IdConceptop })
                .ForeignKey("dbo.DetalleDeudors", t => new { t.DetalleDeudor_IdDeudor, t.DetalleDeudor_IdConceptop })
                .ForeignKey("dbo.Estadoes", t => t.Estado_Id_Estado)
                .ForeignKey("dbo.Deudores", t => t.Deudores_Id_Deudor)
                .ForeignKey("dbo.ConceptosPagoes", t => t.TipoConcepto_Id_Concepto)
                .ForeignKey("dbo.ConceptosPagoes", t => t.ConceptosPago_Id_Concepto)
                .Index(t => new { t.DetalleDeudor_IdDeudor, t.DetalleDeudor_IdConceptop })
                .Index(t => t.Estado_Id_Estado)
                .Index(t => t.Deudores_Id_Deudor)
                .Index(t => t.TipoConcepto_Id_Concepto)
                .Index(t => t.ConceptosPago_Id_Concepto);
            
            CreateTable(
                "dbo.Deudores",
                c => new
                    {
                        Id_Deudor = c.Int(nullable: false, identity: true),
                        Id_Estudiante = c.Int(nullable: false),
                        Fecha_Ingreso = c.DateTime(nullable: false),
                        Fecha_Pagar = c.DateTime(nullable: false),
                        Total_Pagar = c.Decimal(precision: 18, scale: 2),
                        IdEstado = c.Int(nullable: false),
                        Detalles = c.String(),
                        Deudores_Id_Deudor = c.Int(),
                        Estado_Id_Estado = c.Int(),
                    })
                .PrimaryKey(t => t.Id_Deudor)
                .ForeignKey("dbo.Deudores", t => t.Deudores_Id_Deudor)
                .ForeignKey("dbo.Estadoes", t => t.Estado_Id_Estado)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: true)
                .Index(t => t.Id_Estudiante)
                .Index(t => t.Deudores_Id_Deudor)
                .Index(t => t.Estado_Id_Estado);
            
            CreateTable(
                "dbo.Estadoes",
                c => new
                    {
                        Id_Estado = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id_Estado);
            
            CreateTable(
                "dbo.Monedas",
                c => new
                    {
                        Id_Moneda = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id_Moneda);
            
            CreateTable(
                "dbo.TipoPagoes",
                c => new
                    {
                        Id_TipoPago = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id_TipoPago);
            
            CreateTable(
                "dbo.Carteras",
                c => new
                    {
                        IdCartera = c.Int(nullable: false, identity: true),
                        codigo = c.String(),
                        Nombre = c.String(),
                        Apellido = c.String(),
                    })
                .PrimaryKey(t => t.IdCartera);
            
            CreateTable(
                "dbo.CierreCajas",
                c => new
                    {
                        Id_Cierre = c.Int(nullable: false, identity: true),
                        Id_Caja = c.Int(nullable: false),
                        Id_Cajero = c.Int(nullable: false),
                        Hora_Cierre = c.DateTime(nullable: false),
                        Total_Cordobas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total_Dolar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Cierre)
                .ForeignKey("dbo.Cajas", t => t.Id_Caja, cascadeDelete: true)
                .ForeignKey("dbo.Cajeroes", t => t.Id_Cajero, cascadeDelete: true)
                .Index(t => t.Id_Caja)
                .Index(t => t.Id_Cajero);
            
            CreateTable(
                "dbo.DetallePagoes",
                c => new
                    {
                        IdPago = c.String(nullable: false, maxLength: 128),
                        IdConcepto = c.Int(nullable: false),
                        Id_Tipo = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdMoneda = c.Int(nullable: false),
                        PagoAbonado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdEquivalencia = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IdEstado = c.Int(nullable: false),
                        Cambio = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Mora = c.Decimal(precision: 18, scale: 2),
                        Conceptos_Id_Concepto = c.Int(),
                        Estado_Id_Estado = c.Int(),
                        Moneda_Id_Moneda = c.Int(),
                        Pago_Id_Pago = c.Int(),
                        TipoPago_Id_TipoPago = c.Int(),
                    })
                .PrimaryKey(t => new { t.IdPago, t.IdConcepto })
                .ForeignKey("dbo.ConceptosPagoes", t => t.Conceptos_Id_Concepto)
                .ForeignKey("dbo.Estadoes", t => t.Estado_Id_Estado)
                .ForeignKey("dbo.Monedas", t => t.Moneda_Id_Moneda)
                .ForeignKey("dbo.Pagoes", t => t.Pago_Id_Pago)
                .ForeignKey("dbo.TipoPagoes", t => t.TipoPago_Id_TipoPago)
                .Index(t => t.Conceptos_Id_Concepto)
                .Index(t => t.Estado_Id_Estado)
                .Index(t => t.Moneda_Id_Moneda)
                .Index(t => t.Pago_Id_Pago)
                .Index(t => t.TipoPago_Id_TipoPago);
            
            CreateTable(
                "dbo.Pagoes",
                c => new
                    {
                        Id_Pago = c.Int(nullable: false, identity: true),
                        FechaTransaccion = c.DateTime(nullable: false),
                        IdCajero = c.Int(nullable: false),
                        IdCaja = c.Int(nullable: false),
                        IdEstudiante = c.String(),
                    })
                .PrimaryKey(t => t.Id_Pago)
                .ForeignKey("dbo.Cajas", t => t.IdCaja, cascadeDelete: true)
                .ForeignKey("dbo.Cajeroes", t => t.IdCajero, cascadeDelete: true)
                .Index(t => t.IdCajero)
                .Index(t => t.IdCaja);
            
            CreateTable(
                "dbo.Equivalencias",
                c => new
                    {
                        Id_Equivalencia = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id_Equivalencia);
            
            CreateTable(
                "dbo.TipoConceptoes",
                c => new
                    {
                        Id_Concepto = c.Int(nullable: false, identity: true),
                        NombreConcepto = c.String(),
                    })
                .PrimaryKey(t => t.Id_Concepto);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetallePagoes", "TipoPago_Id_TipoPago", "dbo.TipoPagoes");
            DropForeignKey("dbo.DetallePagoes", "Pago_Id_Pago", "dbo.Pagoes");
            DropForeignKey("dbo.Pagoes", "IdCajero", "dbo.Cajeroes");
            DropForeignKey("dbo.Pagoes", "IdCaja", "dbo.Cajas");
            DropForeignKey("dbo.DetallePagoes", "Moneda_Id_Moneda", "dbo.Monedas");
            DropForeignKey("dbo.DetallePagoes", "Estado_Id_Estado", "dbo.Estadoes");
            DropForeignKey("dbo.DetallePagoes", "Conceptos_Id_Concepto", "dbo.ConceptosPagoes");
            DropForeignKey("dbo.CierreCajas", "Id_Cajero", "dbo.Cajeroes");
            DropForeignKey("dbo.CierreCajas", "Id_Caja", "dbo.Cajas");
            DropForeignKey("dbo.Facturas", "Cartera_IdCartera", "dbo.Carteras");
            DropForeignKey("dbo.AperturaCajas", "Cartera_IdCartera", "dbo.Carteras");
            DropForeignKey("dbo.Facturas", "TipoPago_Id_TipoPago", "dbo.TipoPagoes");
            DropForeignKey("dbo.Facturas", "Moneda_Id_Moneda", "dbo.Monedas");
            DropForeignKey("dbo.Facturas", "Conceptos_Id_Concepto", "dbo.ConceptosPagoes");
            DropForeignKey("dbo.DetalleDeudors", "ConceptosPago_Id_Concepto", "dbo.ConceptosPagoes");
            DropForeignKey("dbo.DetalleDeudors", "TipoConcepto_Id_Concepto", "dbo.ConceptosPagoes");
            DropForeignKey("dbo.DetalleDeudors", "Deudores_Id_Deudor", "dbo.Deudores");
            DropForeignKey("dbo.Deudores", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.Deudores", "Estado_Id_Estado", "dbo.Estadoes");
            DropForeignKey("dbo.DetalleDeudors", "Estado_Id_Estado", "dbo.Estadoes");
            DropForeignKey("dbo.Deudores", "Deudores_Id_Deudor", "dbo.Deudores");
            DropForeignKey("dbo.ConceptosPagoes", new[] { "DetalleDeudor_IdDeudor1", "DetalleDeudor_IdConceptop1" }, "dbo.DetalleDeudors");
            DropForeignKey("dbo.DetalleDeudors", new[] { "DetalleDeudor_IdDeudor", "DetalleDeudor_IdConceptop" }, "dbo.DetalleDeudors");
            DropForeignKey("dbo.ConceptosPagoes", new[] { "DetalleDeudor_IdDeudor", "DetalleDeudor_IdConceptop" }, "dbo.DetalleDeudors");
            DropForeignKey("dbo.Facturas", "IdCajero", "dbo.Cajeroes");
            DropForeignKey("dbo.Cajeroes", "Id_Empleado", "dbo.Empleadoes");
            DropForeignKey("dbo.AperturaCajas", "IdCajero", "dbo.Cajeroes");
            DropForeignKey("dbo.Facturas", "IdCaja", "dbo.Cajas");
            DropForeignKey("dbo.AperturaCajas", "IdCaja", "dbo.Cajas");
            DropIndex("dbo.Pagoes", new[] { "IdCaja" });
            DropIndex("dbo.Pagoes", new[] { "IdCajero" });
            DropIndex("dbo.DetallePagoes", new[] { "TipoPago_Id_TipoPago" });
            DropIndex("dbo.DetallePagoes", new[] { "Pago_Id_Pago" });
            DropIndex("dbo.DetallePagoes", new[] { "Moneda_Id_Moneda" });
            DropIndex("dbo.DetallePagoes", new[] { "Estado_Id_Estado" });
            DropIndex("dbo.DetallePagoes", new[] { "Conceptos_Id_Concepto" });
            DropIndex("dbo.CierreCajas", new[] { "Id_Cajero" });
            DropIndex("dbo.CierreCajas", new[] { "Id_Caja" });
            DropIndex("dbo.Deudores", new[] { "Estado_Id_Estado" });
            DropIndex("dbo.Deudores", new[] { "Deudores_Id_Deudor" });
            DropIndex("dbo.Deudores", new[] { "Id_Estudiante" });
            DropIndex("dbo.DetalleDeudors", new[] { "ConceptosPago_Id_Concepto" });
            DropIndex("dbo.DetalleDeudors", new[] { "TipoConcepto_Id_Concepto" });
            DropIndex("dbo.DetalleDeudors", new[] { "Deudores_Id_Deudor" });
            DropIndex("dbo.DetalleDeudors", new[] { "Estado_Id_Estado" });
            DropIndex("dbo.DetalleDeudors", new[] { "DetalleDeudor_IdDeudor", "DetalleDeudor_IdConceptop" });
            DropIndex("dbo.ConceptosPagoes", new[] { "DetalleDeudor_IdDeudor1", "DetalleDeudor_IdConceptop1" });
            DropIndex("dbo.ConceptosPagoes", new[] { "DetalleDeudor_IdDeudor", "DetalleDeudor_IdConceptop" });
            DropIndex("dbo.Cajeroes", new[] { "Id_Empleado" });
            DropIndex("dbo.Facturas", new[] { "Cartera_IdCartera" });
            DropIndex("dbo.Facturas", new[] { "TipoPago_Id_TipoPago" });
            DropIndex("dbo.Facturas", new[] { "Moneda_Id_Moneda" });
            DropIndex("dbo.Facturas", new[] { "Conceptos_Id_Concepto" });
            DropIndex("dbo.Facturas", new[] { "IdCaja" });
            DropIndex("dbo.Facturas", new[] { "IdCajero" });
            DropIndex("dbo.AperturaCajas", new[] { "Cartera_IdCartera" });
            DropIndex("dbo.AperturaCajas", new[] { "IdCajero" });
            DropIndex("dbo.AperturaCajas", new[] { "IdCaja" });
            DropTable("dbo.TipoConceptoes");
            DropTable("dbo.Equivalencias");
            DropTable("dbo.Pagoes");
            DropTable("dbo.DetallePagoes");
            DropTable("dbo.CierreCajas");
            DropTable("dbo.Carteras");
            DropTable("dbo.TipoPagoes");
            DropTable("dbo.Monedas");
            DropTable("dbo.Estadoes");
            DropTable("dbo.Deudores");
            DropTable("dbo.DetalleDeudors");
            DropTable("dbo.ConceptosPagoes");
            DropTable("dbo.Cajeroes");
            DropTable("dbo.Facturas");
            DropTable("dbo.Cajas");
            DropTable("dbo.AperturaCajas");
        }
    }
}
