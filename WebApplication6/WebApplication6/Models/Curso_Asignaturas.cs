using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Curso_Asignaturas
    {
        [Key]
        public int Id_Curso_Asignatura { get; set; }

        [ForeignKey("Id_Curso")]
        public CursoEscolar CursoEscolar { get; set; }

        public int Id_Curso { get; set; }

        [ForeignKey("Id_Materia")]
        public Materias Materias{ get; set; }
        public int Id_Materia { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado{ get; set; }
        public int Id_Empleado { get; set; }

        [Display(Name ="Estado del curso")]
        public bool Estado_Asignatura { get; set; }
    }
}