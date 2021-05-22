using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication6.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Id_Empleado")]
        public Empleado Empleado { get; set; }
        [Display(Name ="Codigo Empleado")]
        [Required]
        public int Id_Empleado { get; set; }

        [Display(Name ="Ultima fecha de conexcion")]
        public DateTime Fecha_Conexcion{ get; set; }



        public List<ControlDisciplinario> controlDisciplinaris{ get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Empleado> TbEmpleado { get; set; }

        public DbSet<Materias> tbmaterias { get; set; }

        public DbSet<Estudios_Maestro> tbEstudios_Maestro { get; set; }

        public DbSet<AniosACursar> tbAniosACursar { get; set; }

        public DbSet<Estudiantes> tbestudiantes { get; set; }
        public DbSet<FamiliarEstudiante> FamiliarEstudiantes { get; set; }

        public DbSet<Enfermedad> tbEnfermedad { get; set; }

        public DbSet<TPMedicacion> tbTPmedicacion { get; set; }

        public DbSet<HistorialAcademico> tbHistorialAcademico { get; set; }
        public DbSet<EnfermedadEstudiantes> EnfermedadEstudiantes { get; set; }
        public DbSet<ControlDisciplinario> tbcontrolDisciplinario { get; set; }
        public DbSet<HistorialAñosCursado> tbHistorialAniosCursado { get; set; }

        public DbSet<Permisos> tbPermisos { get; set; }

        public DbSet<PermisosYRoles> tbpermisosyroles { get; set; }

        public DbSet<CursoEscolar> tbCursoEscolar{ get; set; }

        public DbSet<Curso_Asignaturas>  tbCursoAsignaturas { get; set; }

        public DbSet<EmpleadoMaterias> tbEmpleadoMaterias { get; set; }

        public DbSet<CursoEstudiantes> CursoEstudiantes{ get; set; }

        public  DbSet<OpcionesDeColegio>  OpcionesDeColegios  { get; set; }

        public DbSet<Modalidades> Modalidades { get; set; }

        public DbSet<Notas> Notas { get; set; }

        public DbSet<Evaluaciones> Evaluaciones { get; set; }

        public DbSet<EvaluacionesEstudiantes> EvaluacionesEstudiantes { get; set; }
        public DbSet<PeriodosEscolares> PeriodosEscolares { get; set; }

        public DbSet<TipoEvaluacion> TipoEvaluacions { get; set; }
        public DbSet<CalendarioCurso> CalendarioCursos { get; set; }

        public DbSet<SemestresEscolares> SemestresEscolares { get; set; }
        public DbSet<ProfesorGuia> ProfesorGuias { get; set; }

        public DbSet<Parciales> Parciales { get; set; }
        public DbSet<Pago> Pago { get; set; }

        public DbSet<DetallePago> DetallePago { get; set; }

        public DbSet<Deudores> Deudores { get; set; }

        public DbSet<DetalleDeudor> DetalleDeudor { get; set; }

        public DbSet<Factura> Facturas { get; set; }

        public DbSet<Cartera> Carteras { get; set; }

        public DbSet<Cajero> Cajero { get; set; }
        public DbSet<Caja> Caja { get; set; }
        public DbSet<AperturaCaja> AperturaCaja { get; set; }
        public DbSet<CierreCaja> CierreCajas { get; set; }

        public DbSet<TipoConcepto> TipoConcepto { get; set; }

        public DbSet<ConceptosPago> ConceptosPago { get; set; }
        public DbSet<TipoPago> TipoPago { get; set; }

        public DbSet<Moneda> Moneda { get; set; }

        public DbSet<Equivalencia> Equivalencia { get; set; }

        public DbSet<Estado> Estado { get; set; }

        public static ApplicationDbContext Create()

        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WebApplication6.Models.Secciones> Secciones { get; set; }

        public System.Data.Entity.DbSet<WebApplication6.Models.EmpleadoCalendario> EmpleadoCalendarios { get; set; }

    }
}