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
    public class DetalleDeudor
    {
        [Key, Column(Order = 0)]

        [Display(Name = "Deudor")]
        public int IdDeudor { get; set; }
        
        public Deudores Deudores { get; set; }


        [Key, Column(Order = 1)]
        [Display(Name = "Concepto")]
        public int IdConceptop { get; set; }
        
        public ConceptosPago TipoConcepto { get; set; }

        [Display(Name = "Total Pagado")]
        public decimal Total_Pagado { get; set; }

        [Display(Name = "Total a deber")]
        public decimal Total_Deber { get; set; }


        public int IdEstado { get; set; }
        
        public Estado Estado { get; set; }

        public virtual DbSet<ConceptosPago> ConceptosPagoSet { get; set; }
        public virtual DbSet<Estado> EstadoSet { get; set; }

        public virtual DbSet<DetalleDeudor> DetallaDeudorset { get; set; }
        public virtual List<DetalleDeudor> BuscarDetalleDeudor { get; set; }
        public List<ConceptosPago> BuscarConceptosPago { get; set; }

        public virtual ICollection<ConceptosPago> ConceptoPago { get; set; }
    }
}