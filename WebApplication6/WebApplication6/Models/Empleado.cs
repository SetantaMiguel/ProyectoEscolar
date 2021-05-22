using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Nombre del Empleado")]
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
        [StringLength(50)]
        [Display(Name = "Código Empleado")]
        public string Codigo_Empleado { get; set; }

        [Display(Name = "Estado civil")]
        public string EstadoCivil { get; set; }

        [Required(ErrorMessage = "Digite una fecha de contratacion valida")]
        [Display(Name = " Fecha de contratacion ")]
        [DataType(DataType.Date)]
        public DateTime Dt_Contratacion { get; set; }

        [Display(Name = "Estado de Empleado")]
        [Required(ErrorMessage = "El campo {0} es necesario")]
        public bool BL_Estado_Empleado { get; set; }
        
        [Display(Name = "Profesor guia")]
        public bool BL_ProfesorGuia { get; set; }

        [Display(Name = "Primera sesion")]
        public bool FirstLogin { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int Edad {
            get => DateTime.Now.Year - FechaNaci.Year;
        }


        [NotMapped]
        public string NCompleto
        {
            get => Nombre + " " + Apellido;
        }
    }
    }