using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Secciones
    {

        [Key]
        public int Id_Seccion { get; set; }
        
        [Display(Name ="Seccion")]
        public string Str_Seccion{ get; set; }

        [Display(Name ="Estado de Seccion")]
        public bool Bl_Estado{ get; set; }


        [Display(Name ="Estudiantes en Aula")]
        public int EstudiantesAula { get; set; }

        [Display(Name ="Aula Ocupada")]
        public bool AulaLLena { get; set; }

        [Required]
        [Display(Name ="Maximo estudiantes por aula")]
        public int Max_Estudiantes { get; set; }
    }
}