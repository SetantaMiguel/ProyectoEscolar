using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Parciales
    {
        [Key]
        public int Id_Parcial { get; set; }

        [Display(Name ="Nombre del parcial")]
        [Required()]
        public string Nombre_Parcial { get; set; }

        [Display(Name ="Estado de Parcial")]
        public bool Estado_Parcial{ get; set; }

        [ForeignKey("Id_SemestreEscolar")]
        public SemestresEscolares SemestresEscolares { get; set; }
        public int Id_SemestreEscolar{ get; set; }

        [Display(Name ="Inicio del Parcial")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Dt_Inicio{ get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name ="Final del Periodo")]
        public DateTime Dt_Final{ get; set; }
    }
}