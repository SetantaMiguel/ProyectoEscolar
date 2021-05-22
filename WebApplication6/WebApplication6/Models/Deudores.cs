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
    public class Deudores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Deudor { get; set; }


        public int Id_Estudiante { get; set; }
        
        public Estudiantes Estudiantes { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        public DateTime Fecha_Ingreso { get; set; }

        [Display(Name = "Fecha a Pagar")]
        public DateTime Fecha_Pagar { get; set; }

        [Display(Name = "Total a Pagar")]
        public decimal? Total_Pagar { get; set; }

        [Display(Name = "Estado")]
        public int IdEstado { get; set; }
        
        public Estado Estado { get; set; }

        [Display(Name = "Detalles")]
        public string Detalles { get; set; }

        public List<Deudores> BuscarDeudores { get; set; }
        //public List<Estado> BuscarEstados { get; set; }
        

    }
}