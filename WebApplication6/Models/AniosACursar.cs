using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class AniosACursar
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es necesario")]
        [Display(Name = "Año Escolar")]
        public string Str_Curso { get; set; }


        public bool Estado { get; set; }
        public List<Estudiantes> Estudiantes { get; set; }
    }
}