using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Estudiantes
    {
        [Key]
        public int Id_Estudiante { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Nombre Estudiante")]
        public string Nombre { get; set; }

        public string Apellido { get; set; }
    
        [NotMapped]
        [Display(Name ="Nombre Completo")]
        [ScaffoldColumn(false)]
        public string NombreCompleto { get => Nombre + " " + Apellido; } 
       
      
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        public string Correo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "Digite una {0}")]
        public DateTime FechaNacimiento { get; set; }

        public string Domicilio { get; set; }

        public string Telefono { get; set; }

        public string Genero { get; set; }
        [Display(Name = "Estado del estudiante")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public bool Estado_Estudiante { get; set; }

        [Display(Name = "Codigo estudiante")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Codigo_Estudiante { get; set; }

        [Display(Name = "Codigo MINED")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Codigo_MINED { get; set; }

        [Display(Name = "Nombre de padre")]
        public string Str_Nombre_Padre { get; set; }

        [Display(Name = "Nombre de madre")]
        public string Str_Nombre_Madre { get; set; }


        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [ForeignKey("Id")]
        public AniosACursar AniosACursar { get; set; }
        public int Id { get; set; }


        public List<FamiliarEstudiante> FamiliarEstudiantes { get; set; }


        public List<EnfermedadEstudiantes> EnfermedadEstudiantes { get; set; }
        public List<ControlDisciplinario> ControlDisciplinarios { get; set; }
        public List<HistorialAcademico> HistorialAcademicos { get; set; }
    
        
    }
}