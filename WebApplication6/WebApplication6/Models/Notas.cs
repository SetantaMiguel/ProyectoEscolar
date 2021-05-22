using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WebApplication6.Models
{
    public class Notas
    {
        [Key]
        public int Id_Nota { get; set; }

        [ForeignKey("Id_Estudiante")]

        public Estudiantes Estudiantes { get; set; }

        public int Id_Estudiante { get; set; }

        [ForeignKey("Id_Materia")]
        public Materias Materias { get; set; }
        public int Id_Materia { get; set; }

        public Parciales Parciales { get; set; }
        public int Id_Parcial{ get; set; }

        [ForeignKey("Id_CursoEscolar")]
        public CursoEscolar CursoEscolar { get; set; }
        public int Id_CursoEscolar { get; set; }

        [Display(Name ="Aprobado")]
        public bool Bl_Aprobado { get; set; }

        public int Nota { get; set; }
    }
}