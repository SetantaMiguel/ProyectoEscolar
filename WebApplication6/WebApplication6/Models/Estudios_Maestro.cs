using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Estudios_Maestro
    {
        [Key]
        public int Id_Estudio { get; set; }
        [Display(Name = "Tipo de estudio")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Str_Tipo_Estudio { get; set; }
        //
        [Display(Name = "Nombre del estudio")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Str_Nombre_Estudio { get; set; }
        [Display(Name = "Centro de estudio")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Str_Centro_Estudio { get; set; }
        [Display(Name = "Estado del Estudio")]
        public bool Bl_Estado_Estudio { get; set; }
        [Display(Name = "Fecha de culminacion")]
        [Required(ErrorMessage = "La {0} es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fch_Finalizacion { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }

        public int Id_Empleado { get; set; }
    }
}