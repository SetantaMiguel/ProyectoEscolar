using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Modalidades
    {
        [Key]
        public int Id_Modalidad { get; set; }

        [Required]
        [Display(Name ="Modalidad")]
        public string Str_Modalidad { get; set; }

    }
}