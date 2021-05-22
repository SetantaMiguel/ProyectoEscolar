using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class SemestresEscolares
    {
        [Key]
        public int Id_Semestre { get; set; }

        [Required(ErrorMessage ="Este campo es requerido")]
        [Display(Name ="Nombre del semestre")]
        public string NombreSemestre{ get; set; }

        public bool EstadoSemestre { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Este campo es requerido")]
        [Display(Name ="Inico de semestre")]
        public DateTime FchInicio { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Este campo es requerido")]
        [Display(Name ="Final de semestre")]
        public DateTime FchFinal { get; set; }

    }
}