using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class CursoEstudiantes
    {
        public int Id { get; set; }


        [ForeignKey("Id_Curso")]
        public CursoEscolar CursoEscolar { get; set; }

        public int Id_Curso { get; set; }


        [ForeignKey("Id_Estudiante")]
        public Estudiantes Estudiantes { get; set; }

        public int Id_Estudiante { get; set; }

    }
}