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
    public class ConceptosPago
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Concepto { get; set; }


        [Display(Name ="Nombre del concepto")]
        public string NombreConcepto { get; set; }
              
        [Display(Name ="Monto Dolares")]
        public decimal Monto { get; set; }


        [Display(Name ="Fecha a aplicar mora")]
        [DataType(DataType.Date)]
        public DateTime fechaMora { get; set; }

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        [ScaffoldColumn(false)]
        public string NombreMonto { get => NombreConcepto + " - $ " + Monto; }

        [Display(Name ="Estado del concepto")]
        public bool Estado_Concepto { get; set; }


    }
}