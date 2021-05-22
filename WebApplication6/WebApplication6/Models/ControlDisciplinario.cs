using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class ControlDisciplinario
    {
        [Key]
        public int Id_Control { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de emision")]
        [Required(ErrorMessage = "Digite una {0}")]
        public DateTime Fecha_Emision { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [ForeignKey("Id_Estudiantes")]
        public Estudiantes Estudiantes { get; set; }

        public int Id_Estudiantes { get; set; }

        [ForeignKey("Id")]
        public ApplicationUser IdentityUser { get; set; }
        public string Id { get; set; }
    }
}