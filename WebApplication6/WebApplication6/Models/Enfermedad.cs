using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Enfermedad
    {
        [Key]
        public int Id_Enfermedad { get; set; }
        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Nombre de Enfermedad")]
        public string Str_NombreEnfermedad { get; set; }

        [Display(Name = "Descripcion")]
        public string Str_Descripcion { get; set; }

        public List<EnfermedadEstudiantes> Enfermedads { get; set; }
    }
}