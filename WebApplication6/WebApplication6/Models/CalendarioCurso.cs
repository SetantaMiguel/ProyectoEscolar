using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class CalendarioCurso
    {
        [Key]
        public int Id_CalendarioCurso { get; set; }

        [ForeignKey("Id_Curso")]
        public CursoEscolar CursoEscolar { get; set; }

        public int Id_Curso { get; set; }

        [ForeignKey("Id_Periodo")]
        public PeriodosEscolares PeriodosEscolares { get; set; }
        public int Id_Periodo{ get; set; }


        [ForeignKey("Id_CursoAsignatura")]
        public Curso_Asignaturas Curso_Asignaturas { get; set; }

        public int Id_CursoAsignatura { get; set; }

        [Display(Name ="Estado del tiempo en el calendario")]
        public bool Bl_Estado { get; set; }
    }
}