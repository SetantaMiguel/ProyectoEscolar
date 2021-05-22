using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Caja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCaja { get; set; }

        [Display(Name = "Numero de Caja")]
        public string NumCaja { get; set; }

        public List<AperturaCaja> AperturaCaja { get; set; }
        public List<Factura> Facturas { get; set; }




    }
}