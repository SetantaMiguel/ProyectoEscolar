using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class PeriodosEscolares
    {
        [Key]
        public int Id_Periodo { get; set; }
        [Display(Name ="Dia de la Semana")]
        public string DiaSemana { get; set; }

        [Required]
        [Display(Name = "Inicio de Periodo")]
        public string InicioPeriodo { get; set; }

        [Required]
        [Display(Name = "Fin de Periodo")]
        public string FinPeriodo { get; set; }

        [Display(Name = "Periodo Activo")]
        public bool  Bl_EstadoPeriodo { get; set; }

        [Display(Name ="Nombre del periodo")]
        public string Nombre_Periodo { get; set; }

    }
}