using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Cajero
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCajero { get; set; }

        [Display(Name = "Numero de Cajero")]
        public int NumCajero { get; set; }

        public string codigo { get; set; }

        public int Id_Empleado { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado  Empleado{ get; set; }
        
        public virtual List<AperturaCaja> AperturaCaja { get; set; }

        public List<Factura> Facturas { get; set; }

    }
}