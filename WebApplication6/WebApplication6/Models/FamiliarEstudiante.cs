using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication6.Models
{
    public class FamiliarEstudiante
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(20)]
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        public string Correo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "Digite una {0}")]
        public DateTime FechaNaci { get; set; }

        public string Domicilio { get; set; }

        public string Telefono { get; set; }

        public string Genero { get; set; }

        public string Identificacion { get; set; }

        [Display(Name = "Estado civil")]
        public string EstadoCivil { get; set; }


        [Display(Name = "Estado del familiar")]
        public bool BL_Estado_Familiar { get; set; }


        [Display(Name = "Responsable")]
        public bool BL_Responsable { get; set; }

        [ForeignKey("Id_Estudiante")]
        public Estudiantes Estudiantes { get; set; }
        public int Id_Estudiante { get; set; }
    }
}