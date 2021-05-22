using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace WebApplication6.Models
{
    public class EmpleadoMaterias
    {
        [Key]
        public int Id_EmpleadoMaterias { get; set; }

        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
        public int Id_Empleado{ get; set; }

        [ForeignKey("Id_Materia")]
        public Materias Materias{ get; set; }
        public int Id_Materia { get; set; }
    }
}