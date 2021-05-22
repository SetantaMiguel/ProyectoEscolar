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
    public class CierreCaja
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Cierre { get; set; }


        public int Id_Caja { get; set; }
        [ForeignKey("Id_Caja")]
        public Caja Caja { get; set; }



        public int Id_Cajero { get; set; }
        [ForeignKey("Id_Cajero")]
        public Cajero Cajero { get; set; }



        public DateTime Hora_Cierre { get; set; } 


        public decimal Total_Cordobas { get; set; }



        public decimal Total_Dolar { get; set; }


        public bool Estado { get; set; }

    }
}