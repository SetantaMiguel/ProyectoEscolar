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
    public class DetallePago
    {
        
        [Key, Column(Order = 0)]
        public string   IdPago { get; set; }
        
        public Pago Pago { get; set; }


        [Key, Column(Order = 1)]
        public int IdConcepto { get; set; }
        
        public ConceptosPago Conceptos { get; set; }


        public int Id_Tipo { get; set; }
        
        public TipoPago TipoPago { get; set; }


        public decimal Monto { get; set; }


        public int IdMoneda { get; set; }
        
        public Moneda Moneda { get; set; }


        public decimal PagoAbonado { get; set; }


        public decimal IdEquivalencia { get; set; }
        
        


        public decimal Balance { get; set; }


        public int IdEstado { get; set; }
        
        public Estado Estado { get; set; }

        public decimal? Cambio { get; set; }
        
        public decimal Total { get; set; }


        public decimal? Mora { get; set; }

        public virtual DbSet<ConceptosPago> ConceptoPagoSet { get; set; }
        public virtual DbSet<TipoPago> TipoPagoSet { get; set; }

        public virtual DbSet<Moneda> MonedaSet { get; set; }

        //public virtual DbSet<Equivalencia> EquivalenciasSet { get; set; }
        public virtual DbSet<Estado> EstadoSet { get; set; }

    }
}