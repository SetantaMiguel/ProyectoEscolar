using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class EnfermedadEstudiantes
    {
        [Key]
        public int Id { get; set; }
        public string Medicacion { get; set; }
        //relacion con estudiante
        [ForeignKey("Id_Estudiante")]
        public Estudiantes Estudiantes { get; set; }

        public int Id_Estudiante { get; set; }

        //relacion con enfermedad
        [ForeignKey("Id_Enfermedad")]
        public Enfermedad Enfermedad { get; set; }

        public int Id_Enfermedad { get; set; }
        //Relacion con medicacion
        [ForeignKey("Id_Medicacion")]
        public TPMedicacion TPMedicacion { get; set; }

        public int Id_Medicacion { get; set; }
    }
}