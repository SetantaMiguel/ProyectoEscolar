using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Materias
    {
        [Key]
        public int Id_Materia { get; set; }
        //
        [Display(Name = "Codigo Materia")]
        [Required(ErrorMessage = "El {0} Es requerido")]
        public string Codigo_Materia { get; set; }
        //
        //[Index(IsUnique = true)]
        [Display(Name = "Nombre de materia")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Nombre_Materia { get; set; }

        [Display(Name ="Estado Materias")]
        public bool EstadoMateria{ get; set; }

        [ForeignKey("Id_AñoEscolar")]
        public AniosACursar AniosACursar { get; set; }
        public int Id_AñoEscolar { get; set; }
    }
}