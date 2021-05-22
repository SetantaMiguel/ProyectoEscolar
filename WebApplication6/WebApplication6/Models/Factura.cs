using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Factura
    {

        [Key, Column(Order = 0)]
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        public DateTime FechaFactura { get; set; }


        [Display(Name = "Cajero")]
        public int IdCajero { get; set; }
        public Cajero Cajero { get; set; }



        [Display(Name = " Caja ")]
        public int IdCaja { get; set; }
        public Caja Caja { get; set; }

        [Display(Name = "Carnet")]
        public string IdEstudiante { get; set; }

        [Key, Column(Order = 1)]
        [Display(Name = "Concepto")]
        public int IdConcepto { get; set; }
        public ConceptosPago Conceptos { get; set; }


        [Display(Name = "Tipo de Pago")]
        public int Id_Tipo { get; set; }
        public TipoPago TipoPago { get; set; }



        public decimal? Monto { get; set; }

        [Display(Name = "Moneda")]
        public int IdMoneda { get; set; }
        public Moneda Moneda { get; set; }


        [Display(Name = "Pago Abonado")]
        public decimal? PagoAbonado { get; set; }

        [Display(Name = "Equivalencia")]
        public decimal? IdEquivalencia { get; set; }

        [Display(Name = "Cambio")]

        public decimal? Cambio { get; set; }

        public decimal Total { get; set; }

        public decimal? Mora { get; set; }


        public virtual DbSet<Caja> CajaSet { get; set; }

        public virtual DbSet<Cajero> CajeroSet { get; set; }

        public virtual  DbSet<ConceptosPago> ConceptosSet { get; set; }

        public virtual DbSet<TipoPago> TipoSet { get; set; }

        public virtual DbSet<Moneda> MonedaSet { get; set; }
    }
}