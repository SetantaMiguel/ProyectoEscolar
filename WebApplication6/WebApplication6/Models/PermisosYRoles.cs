using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class PermisosYRoles
    {
        public int Id { get; set; }
        
        [ForeignKey("Id_Rol")]
        public Microsoft.AspNet.Identity.EntityFramework.IdentityRole IdentityRole  { get; set; }
        [Display(Name ="Rol del sistema")]
        public string Id_Rol{ get; set; }

        [ForeignKey("Id_Permiso")]
        public Permisos Permisos { get; set; }
        [Display(Name ="Permiso del sistema")]
        public int Id_Permiso { get; set; }
    }
}