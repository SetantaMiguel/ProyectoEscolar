using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class CursoEscolar
    {
        [Key]
        public int Id_Curso { get; set; }

        [ForeignKey("Id_Modalidad")]
        public Modalidades Modalidades{ get; set; }

        public int Id_Modalidad { get; set; }

        [Display(Name ="Nombre del curso")]
        public string NombredeCurso { get; set; }

        [Display(Name ="Estado del Curso")]
        public bool Bl_Estado { get; set; }
        
        [ForeignKey("Id_Año")]
        public AniosACursar AniosACursar { get; set; }

        public int Id_Año { get; set; }

        [ForeignKey("Id_Seccion")]
        public Secciones Secciones { get; set; }

        public int Id_Seccion { get; set; }

    }
}