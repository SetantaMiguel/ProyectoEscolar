using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Permisos
    {
        [Key]
        public int Id_Permiso { get; set; }

        [Display(Name ="Nombre de permiso")]
        [Required(ErrorMessage ="El {0} es requerido")]
        public string NombrePermiso { get; set; }
    }
}