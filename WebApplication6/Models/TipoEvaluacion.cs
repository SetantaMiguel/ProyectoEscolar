using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class TipoEvaluacion
    {
        [Key]
        public int Id_TipoEvaluacion { get; set; }

        [Display(Name ="Tipo de evaluacion")]
        [Required(ErrorMessage ="Este campo es requerido")]
        public string strTipoEvaluacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name ="Descripcion del tipo evaluacion")]
        public string DescripcionTipEvaluacion { get; set; }

        [Display(Name ="Estado del tipo evaluacion")]
        public bool bl_EstadoTipo { get; set; }
    }
}