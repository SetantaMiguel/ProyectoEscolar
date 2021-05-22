using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class TPMedicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Tipo de medicacion")]
        public string Str_Tipo { get; set; }
    }
}