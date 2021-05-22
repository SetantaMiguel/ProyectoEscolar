using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class HistorialAñosCursado
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Id_Escuela")]
        public HistorialAcademico HistorialAcademico { get; set; }
        public int Id_Escuela { get; set; }

        [ForeignKey("Id_Anio")]
        public AniosACursar AniosACursar { get; set; }
        public int Id_Anio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de finalizacion")]
        public DateTime FechFinaly { get; set; }
    }
}