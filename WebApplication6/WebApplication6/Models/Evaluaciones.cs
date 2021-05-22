using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Evaluaciones
    {
        [Key]
        public int Id_Evaluacion { get; set; }

        [ForeignKey("Id_Materia")]
        public Materias Materias { get; set; }

        public int Id_Materia { get; set; }
        
        [ForeignKey("Id_Parciales ")]
        public Parciales Parciales { get; set; }

        public int Id_Parciales { get; set; }



        [ForeignKey("Id_Curso")]
        public CursoEscolar CursoEscolar{ get; set; }

        public int Id_Curso { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime Fecha_Comienzo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Final")]
        public DateTime Fecha_Final { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Evaluacion Aprobada")]
        public bool BL_Aprobado { get; set; }


        [ForeignKey("Id_TipoEvaluacion")]
        public TipoEvaluacion TipoEvaluacion { get; set; }

        public int Id_TipoEvaluacion { get; set; }


    }
}