using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class ProfesorGuia
    {
        [Key]
        public int Id_ProfesorGuia { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }

        [Display(Name ="Profesor guia")]
        public int Id_Empleado { get; set; }

        [ForeignKey("Id_Curso")]
        public CursoEscolar CursoEscolar { get; set; }
        public int Id_Curso { get; set; }
        [Display(Name ="Estado del profesor")]
        public bool Estado_Profesor { get; set; }

    }
}