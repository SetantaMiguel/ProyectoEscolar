using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.viewModels
{
    public class IndexViewModel2:BaseModelo
    {
        public List<Estudiantes> Estudiantes { get; set; }

        public List<Empleado> Empleados { get; set; }

        public List<AniosACursar> AniosACursars { get; set; }

        public List<ControlDisciplinario> controlDisciplinarios{ get; set; }

        public List<CursoEscolar> CursoEscolars { get; set; }

        public List<CursoEstudiantes> CursoEstudiantes { get; set; }
        public List<Materias> Materias { get; set; }
        public List<Secciones> Secciones { get; set; }
        public List<Evaluaciones> Evaluaciones { get; set; }
        public List<Notas> Notas { get; set; }
    }
}