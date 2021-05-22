using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Cartera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCartera { get; set; }



        public string codigo { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        [ScaffoldColumn(false)]
        public string NombreCompleto
        {
            get => Nombre + " " + Apellido;
        }

        public virtual List<AperturaCaja> AperturaCaja { get; set; }

        public List<Factura> Facturas { get; set; }

    }

}