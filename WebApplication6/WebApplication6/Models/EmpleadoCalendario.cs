using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class EmpleadoCalendario
    {
        [Key]
        public int Id_EmpleadoCalendario{ get; set; }

        [ForeignKey("Id_Periodo")]
        public PeriodosEscolares PeriodosEscolares { get; set; }

        public int Id_Periodo { get; set; }

        [ForeignKey("Id_Curso")]
        public Curso_Asignaturas Curso_Asignaturas { get; set; }
        public int Id_Curso{ get; set; }
    }
}