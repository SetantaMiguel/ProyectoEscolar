using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class AperturaCaja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdApertura { get; set; }

        public int IdCaja { get; set; }

        [ForeignKey("IdCaja")]
        public Caja Caja { get; set; }

        public int IdCajero { get; set; }

        [ForeignKey("IdCajero")]
        public Cajero Cajero { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public decimal TotalCordoba { get; set; }

        public decimal TotalDolar { get; set; }

        public bool Estado { get; set; }

        public virtual DbSet<Caja> CajaSet { get; set; }

        public virtual DbSet<Cajero> CajeroSet { get; set; }
    }
}