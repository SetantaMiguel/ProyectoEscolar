using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class HistorialAcademico
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido ")]
        [Display(Name = "Codigo de escuela")]
        public string Cod_Escuela { get; set; }

        //LLave foranea
        [ForeignKey("Id_Estudiante")]
        public Estudiantes Estudiantes { get; set; }

        public int Id_Estudiante { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Nombre del colegio")]
        public string Str_NombreColegio { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Año cursado")]
        public int Num_AnioCursado { get; set; }

        public List<HistorialAñosCursado> HistorialAñosCursados { get; set; }
    }
}