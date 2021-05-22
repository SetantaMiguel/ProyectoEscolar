using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class EvaluacionesEstudiantes
    {
        [Key]
        public int  Id_EvaluacionesEstudiantes{ get; set; }

        [ForeignKey("Id_Evaluaciones")]
        public  Evaluaciones Evaluaciones { get; set; }
        //-----------------------------------------------------------------
        public int Id_Evaluaciones { get; set; }

        [ForeignKey("Id_Estudiante")]
        public Estudiantes Estudiantes { get; set; }
        public int Id_Estudiante { get; set; }
        //-----------------------------------------------------------------
        public int Resultado { get; set; }

        [Display(Name ="Esta Evaluado")]

        public bool EstadoEvaluado { get; set; }
    }
}