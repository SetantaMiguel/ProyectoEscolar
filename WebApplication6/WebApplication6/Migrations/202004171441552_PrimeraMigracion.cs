namespace WebApplication6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimeraMigracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalendarioCursoes",
                c => new
                    {
                        Id_CalendarioCurso = c.Int(nullable: false, identity: true),
                        Id_Curso = c.Int(nullable: false),
                        Id_Periodo = c.Int(nullable: false),
                        Id_CursoAsignatura = c.Int(nullable: false),
                        Bl_Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_CalendarioCurso)
                .ForeignKey("dbo.Curso_Asignaturas", t => t.Id_CursoAsignatura, cascadeDelete: true)
                .ForeignKey("dbo.CursoEscolars", t => t.Id_Curso, cascadeDelete: true)
                .ForeignKey("dbo.PeriodosEscolares", t => t.Id_Periodo, cascadeDelete: true)
                .Index(t => t.Id_Curso)
                .Index(t => t.Id_Periodo)
                .Index(t => t.Id_CursoAsignatura);
            
            CreateTable(
                "dbo.Curso_Asignaturas",
                c => new
                    {
                        Id_Curso_Asignatura = c.Int(nullable: false, identity: true),
                        Id_Curso = c.Int(nullable: false),
                        Id_Materia = c.Int(nullable: false),
                        Id_Empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Curso_Asignatura)
                .ForeignKey("dbo.CursoEscolars", t => t.Id_Curso, cascadeDelete: false)
                .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true)
                .ForeignKey("dbo.Materias", t => t.Id_Materia, cascadeDelete: true)
                .Index(t => t.Id_Curso)
                .Index(t => t.Id_Materia)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.CursoEscolars",
                c => new
                    {
                        Id_Curso = c.Int(nullable: false, identity: true),
                        Id_Modalidad = c.Int(nullable: false),
                        NombredeCurso = c.String(),
                        Bl_Estado = c.Boolean(nullable: false),
                        Id_Año = c.Int(nullable: false),
                        Id_Seccion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Curso)
                .ForeignKey("dbo.AniosACursars", t => t.Id_Año, cascadeDelete: true)
                .ForeignKey("dbo.Modalidades", t => t.Id_Modalidad, cascadeDelete: true)
                .ForeignKey("dbo.Secciones", t => t.Id_Seccion, cascadeDelete: true)
                .Index(t => t.Id_Modalidad)
                .Index(t => t.Id_Año)
                .Index(t => t.Id_Seccion);
            
            CreateTable(
                "dbo.AniosACursars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Str_Curso = c.String(nullable: false),
                        Estado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Estudiantes",
                c => new
                    {
                        Id_Estudiante = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Apellido = c.String(),
                        Direccion = c.String(),
                        Correo = c.String(),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Domicilio = c.String(),
                        Telefono = c.String(),
                        Genero = c.String(),
                        Estado_Estudiante = c.Boolean(nullable: false),
                        Codigo_Estudiante = c.String(nullable: false),
                        Codigo_MINED = c.String(nullable: false),
                        Str_Nombre_Padre = c.String(),
                        Str_Nombre_Madre = c.String(),
                        Image = c.Binary(storeType: "image"),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Estudiante)
                .ForeignKey("dbo.AniosACursars", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ControlDisciplinarios",
                c => new
                    {
                        Id_Control = c.Int(nullable: false, identity: true),
                        Fecha_Emision = c.DateTime(nullable: false),
                        Descripcion = c.String(),
                        Id_Estudiantes = c.Int(nullable: false),
                        Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id_Control)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiantes, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id_Estudiantes)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Id_Empleado = c.Int(nullable: false),
                        Fecha_Conexcion = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Apellido = c.String(),
                        Direccion = c.String(),
                        Correo = c.String(),
                        FechaNaci = c.DateTime(nullable: false),
                        Domicilio = c.String(),
                        Telefono = c.String(),
                        Genero = c.String(),
                        Identificacion = c.String(),
                        Codigo_Empleado = c.String(nullable: false, maxLength: 10),
                        EstadoCivil = c.String(),
                        Dt_Contratacion = c.DateTime(nullable: false),
                        BL_Estado_Empleado = c.Boolean(nullable: false),
                        BL_ProfesorGuia=c.Boolean(nullable:false),
                        FirstLogin = c.Boolean(nullable:false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProfesorGuias",
                c => new
                {
                    Id_ProfesorGuia = c.Int(nullable: false, identity: true),
                    Id_Empleado = c.Int(nullable: false),
                    Id_Curso = c.Int(nullable: false),
                    Estado_Profesor = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id_ProfesorGuia)
                .ForeignKey("dbo.CursoEscolars", t => t.Id_Curso, cascadeDelete: true)
                .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado)
                .Index(t => t.Id_Curso);

            CreateTable(
                "dbo.Estudios_Maestro",
                c => new
                    {
                        Id_Estudio = c.Int(nullable: false, identity: true),
                        Str_Tipo_Estudio = c.String(nullable: false),
                        Str_Nombre_Estudio = c.String(nullable: false),
                        Str_Centro_Estudio = c.String(nullable: false),
                        Bl_Estado_Estudio = c.Boolean(nullable: false),
                        Fch_Finalizacion = c.DateTime(nullable: false),
                        Id_Empleado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Estudio)
                .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true)
                .Index(t => t.Id_Empleado);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.EnfermedadEstudiantes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Medicacion = c.String(),
                        Id_Estudiante = c.Int(nullable: false),
                        Id_Enfermedad = c.Int(nullable: false),
                        Id_Medicacion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enfermedads", t => t.Id_Enfermedad, cascadeDelete: true)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: true)
                .ForeignKey("dbo.TPMedicacions", t => t.Id_Medicacion, cascadeDelete: true)
                .Index(t => t.Id_Estudiante)
                .Index(t => t.Id_Enfermedad)
                .Index(t => t.Id_Medicacion);
            
            CreateTable(
                "dbo.Enfermedads",
                c => new
                    {
                        Id_Enfermedad = c.Int(nullable: false, identity: true),
                        Str_NombreEnfermedad = c.String(nullable: false),
                        Str_Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id_Enfermedad);
            
            CreateTable(
                "dbo.TPMedicacions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Str_Tipo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FamiliarEstudiantes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Apellido = c.String(),
                        Direccion = c.String(),
                        Correo = c.String(),
                        FechaNaci = c.DateTime(nullable: false),
                        Domicilio = c.String(),
                        Telefono = c.String(),
                        Genero = c.String(),
                        Identificacion = c.String(),
                        EstadoCivil = c.String(),
                        BL_Estado_Familiar = c.Boolean(nullable: false),
                        BL_Responsable = c.Boolean(nullable: false),
                        Id_Estudiante = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: true)
                .Index(t => t.Id_Estudiante);
            
            CreateTable(
                "dbo.HistorialAcademicoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cod_Escuela = c.String(nullable: false),
                        Id_Estudiante = c.Int(nullable: false),
                        Str_NombreColegio = c.String(nullable: false),
                        Num_AnioCursado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: true)
                .Index(t => t.Id_Estudiante);
            
            CreateTable(
                "dbo.HistorialAñosCursado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Escuela = c.Int(nullable: false),
                        Id_Anio = c.Int(nullable: false),
                        FechFinaly = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AniosACursars", t => t.Id_Anio, cascadeDelete: true)
                .ForeignKey("dbo.HistorialAcademicoes", t => t.Id_Escuela, cascadeDelete: false)
                .Index(t => t.Id_Escuela)
                .Index(t => t.Id_Anio);
            
            CreateTable(
                "dbo.Modalidades",
                c => new
                    {
                        Id_Modalidad = c.Int(nullable: false, identity: true),
                        Str_Modalidad = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Modalidad);
            
            CreateTable(
                "dbo.Secciones",
                c => new
                    {
                        Id_Seccion = c.Int(nullable: false, identity: true),
                        Str_Seccion = c.String(),
                        Bl_Estado = c.Boolean(nullable: false),
                        EstudiantesAula = c.Int(nullable: false),
                        AulaLLena = c.Boolean(nullable: false),
                        Max_Estudiantes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Seccion);
            
            CreateTable(
                "dbo.Materias",
                c => new
                    {
                        Id_Materia = c.Int(nullable: false, identity: true),
                        Codigo_Materia = c.String(nullable: false),
                        Nombre_Materia = c.String(nullable: false),
                        EstadoMateria = c.Boolean(nullable: false),
                        Id_AñoEscolar = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Materia)
                .ForeignKey("dbo.AniosACursars", t => t.Id_AñoEscolar, cascadeDelete: false)
                .Index(t => t.Id_AñoEscolar);
            
            CreateTable(
                "dbo.PeriodosEscolares",
                c => new
                    {
                        Id_Periodo = c.Int(nullable: false, identity: true),
                        DiaSemana = c.String(),
                        InicioPeriodo = c.String(nullable: false),
                        FinPeriodo = c.String(nullable: false),
                        Bl_EstadoPeriodo = c.Boolean(nullable: false),
                        Nombre_Periodo = c.String(),
                    })
                .PrimaryKey(t => t.Id_Periodo);
            
            CreateTable(
                "dbo.CursoEstudiantes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Curso = c.Int(nullable: false),
                        Id_Estudiante = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CursoEscolars", t => t.Id_Curso, cascadeDelete: true)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: false)
                .Index(t => t.Id_Curso)
                .Index(t => t.Id_Estudiante);
            
            CreateTable(
                "dbo.EmpleadoCalendarios",
                c => new
                    {
                        Id_EmpleadoCalendario = c.Int(nullable: false, identity: true),
                        Id_Periodo = c.Int(nullable: false),
                        Id_Curso = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_EmpleadoCalendario)
                .ForeignKey("dbo.Curso_Asignaturas", t => t.Id_Curso, cascadeDelete: true)
                .ForeignKey("dbo.PeriodosEscolares", t => t.Id_Periodo, cascadeDelete: true)
                .Index(t => t.Id_Periodo)
                .Index(t => t.Id_Curso);
            
            CreateTable(
                "dbo.Evaluaciones",
                c => new
                    {
                        Id_Evaluacion = c.Int(nullable: false, identity: true),
                        Id_Materia = c.Int(nullable: false),
                        Id_CursoA = c.Int(nullable: false),
                        Fecha_Comienzo = c.DateTime(nullable: false),
                        Fecha_Final = c.DateTime(nullable: false),
                        Descripcion = c.String(),
                        ValorTotal = c.Int(nullable: false),
                        BL_Aprobado = c.Boolean(nullable: false),
                        Num_Evaluacion = c.Int(nullable: false),
                        Id_TipoEvaluacion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Evaluacion)
                .ForeignKey("dbo.Curso_Asignaturas", t => t.Id_CursoA, cascadeDelete: true)
                .ForeignKey("dbo.Materias", t => t.Id_Materia, cascadeDelete: false)
                .ForeignKey("dbo.TipoEvaluacions", t => t.Id_TipoEvaluacion, cascadeDelete: true)
                .Index(t => t.Id_Materia)
                .Index(t => t.Id_CursoA)
                .Index(t => t.Id_TipoEvaluacion);
            
            CreateTable(
                "dbo.TipoEvaluacions",
                c => new
                    {
                        Id_TipoEvaluacion = c.Int(nullable: false, identity: true),
                        strTipoEvaluacion = c.String(nullable: false),
                        DescripcionTipEvaluacion = c.String(),
                        bl_EstadoTipo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_TipoEvaluacion);
            
            CreateTable(
                "dbo.EvaluacionesEstudiantes",
                c => new
                    {
                        Id_EvaluacionesEstudiantes = c.Int(nullable: false, identity: true),
                        Id_Evaluaciones = c.Int(nullable: false),
                        Id_Estudiante = c.Int(nullable: false),
                        Resultado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_EvaluacionesEstudiantes)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: true)
                .ForeignKey("dbo.Evaluaciones", t => t.Id_Evaluaciones, cascadeDelete: true)
                .Index(t => t.Id_Evaluaciones)
                .Index(t => t.Id_Estudiante);
            
            CreateTable(
                "dbo.Notas",
                c => new
                    {
                        Id_Nota = c.Int(nullable: false, identity: true),
                        Id_Estudiante = c.Int(nullable: false),
                        Id_Materia = c.Int(nullable: false),
                        Id_Parcial = c.Int(nullable: false),
                        Bl_Aprobado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Nota)
                .ForeignKey("dbo.Estudiantes", t => t.Id_Estudiante, cascadeDelete: true)
                .ForeignKey("dbo.Materias", t => t.Id_Materia, cascadeDelete: true)
                .ForeignKey("dbo.Parciales", t => t.Id_Parcial, cascadeDelete: true)
                .Index(t => t.Id_Estudiante)
                .Index(t => t.Id_Materia)
                .Index(t => t.Id_Parcial);
            
            CreateTable(
                "dbo.Parciales",
                c => new
                    {
                        Id_Parcial = c.Int(nullable: false, identity: true),
                        Nombre_Parcial = c.String(nullable: false),
                        Estado_Parcial = c.Boolean(nullable: false),
                        Id_SemestreEscolar = c.Int(nullable: false),
                        Dt_Inicio = c.DateTime(nullable: false),
                        Dt_Final = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Parcial)
                .ForeignKey("dbo.SemestresEscolares", t => t.Id_SemestreEscolar, cascadeDelete: true)
                .Index(t => t.Id_SemestreEscolar);

            CreateTable(
                "dbo.SemestresEscolares",
                c => new
                {
                    Id_Semestre = c.Int(nullable: false, identity: true),
                    NombreSemestre = c.String(nullable: false),
                    EstadoSemestre = c.Boolean(nullable: false),
                    FchInicio = c.DateTime(nullable: false),
                    FchFinal = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id_Semestre);
            
            CreateTable(
                "dbo.OpcionesDeColegios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreColegio = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.EmpleadoMaterias",
                c => new
                    {
                        Id_EmpleadoMaterias = c.Int(nullable: false, identity: true),
                        Id_Empleado = c.Int(nullable: false),
                        Id_Materia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_EmpleadoMaterias)
                .ForeignKey("dbo.Empleadoes", t => t.Id_Empleado, cascadeDelete: true)
                .ForeignKey("dbo.Materias", t => t.Id_Materia, cascadeDelete: true)
                .Index(t => t.Id_Empleado)
                .Index(t => t.Id_Materia);
            
            CreateTable(
                "dbo.Permisos",
                c => new
                    {
                        Id_Permiso = c.Int(nullable: false, identity: true),
                        NombrePermiso = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Permiso);
            
            CreateTable(
                "dbo.PermisosYRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Rol = c.String(maxLength: 128),
                        Id_Permiso = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.Id_Rol)
                .ForeignKey("dbo.Permisos", t => t.Id_Permiso, cascadeDelete: true)
                .Index(t => t.Id_Rol)
                .Index(t => t.Id_Permiso);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermisosYRoles", "Id_Permiso", "dbo.Permisos");
            DropForeignKey("dbo.PermisosYRoles", "Id_Rol", "dbo.AspNetRoles");
            DropForeignKey("dbo.EmpleadoMaterias", "Id_Materia", "dbo.Materias");
            DropForeignKey("dbo.EmpleadoMaterias", "Id_Empleado", "dbo.Empleadoes");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Notas", "Id_Parcial", "dbo.Parciales");
            DropForeignKey("dbo.Parciales", "Id_SemestreEscolar", "dbo.SemestresEscolares");
            DropForeignKey("dbo.SemestresEscolares", "Id_AñoEscolar", "dbo.AñosEscolares");
            DropForeignKey("dbo.Notas", "Id_Materia", "dbo.Materias");
            DropForeignKey("dbo.Notas", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.EvaluacionesEstudiantes", "Id_Evaluaciones", "dbo.Evaluaciones");
            DropForeignKey("dbo.EvaluacionesEstudiantes", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.Evaluaciones", "Id_TipoEvaluacion", "dbo.TipoEvaluacions");
            DropForeignKey("dbo.Evaluaciones", "Id_Materia", "dbo.Materias");
            DropForeignKey("dbo.Evaluaciones", "Id_CursoA", "dbo.Curso_Asignaturas");
            DropForeignKey("dbo.EmpleadoCalendarios", "Id_Periodo", "dbo.PeriodosEscolares");
            DropForeignKey("dbo.EmpleadoCalendarios", "Id_Curso", "dbo.Curso_Asignaturas");
            DropForeignKey("dbo.CursoEstudiantes", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.CursoEstudiantes", "Id_Curso", "dbo.CursoEscolars");
            DropForeignKey("dbo.CalendarioCursoes", "Id_Periodo", "dbo.PeriodosEscolares");
            DropForeignKey("dbo.CalendarioCursoes", "Id_Curso", "dbo.CursoEscolars");
            DropForeignKey("dbo.CalendarioCursoes", "Id_CursoAsignatura", "dbo.Curso_Asignaturas");
            DropForeignKey("dbo.Curso_Asignaturas", "Id_Materia", "dbo.Materias");
            DropForeignKey("dbo.Materias", "Id_AñoEscolar", "dbo.AniosACursars");
            DropForeignKey("dbo.Curso_Asignaturas", "Id_Empleado", "dbo.Empleadoes");
            DropForeignKey("dbo.Curso_Asignaturas", "Id_Curso", "dbo.CursoEscolars");
            DropForeignKey("dbo.CursoEscolars", "Id_Seccion", "dbo.Secciones");
            DropForeignKey("dbo.CursoEscolars", "Id_Modalidad", "dbo.Modalidades");
            DropForeignKey("dbo.CursoEscolars", "Id_Año", "dbo.AniosACursars");
            DropForeignKey("dbo.HistorialAñosCursado", "Id_Escuela", "dbo.HistorialAcademicoes");
            DropForeignKey("dbo.HistorialAñosCursado", "Id_Anio", "dbo.AniosACursars");
            DropForeignKey("dbo.HistorialAcademicoes", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.FamiliarEstudiantes", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.EnfermedadEstudiantes", "Id_Medicacion", "dbo.TPMedicacions");
            DropForeignKey("dbo.EnfermedadEstudiantes", "Id_Estudiante", "dbo.Estudiantes");
            DropForeignKey("dbo.EnfermedadEstudiantes", "Id_Enfermedad", "dbo.Enfermedads");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Id_Empleado", "dbo.Empleadoes");
            DropForeignKey("dbo.Estudios_Maestro", "Id_Empleado", "dbo.Empleadoes");
            DropForeignKey("dbo.ControlDisciplinarios", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ControlDisciplinarios", "Id_Estudiantes", "dbo.Estudiantes");
            DropForeignKey("dbo.Estudiantes", "Id", "dbo.AniosACursars");
            DropIndex("dbo.PermisosYRoles", new[] { "Id_Permiso" });
            DropIndex("dbo.PermisosYRoles", new[] { "Id_Rol" });
            DropIndex("dbo.EmpleadoMaterias", new[] { "Id_Materia" });
            DropIndex("dbo.EmpleadoMaterias", new[] { "Id_Empleado" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SemestresEscolares", new[] { "Id_AñoEscolar" });
            DropIndex("dbo.Parciales", new[] { "Id_SemestreEscolar" });
            DropIndex("dbo.Notas", new[] { "Id_Parcial" });
            DropIndex("dbo.Notas", new[] { "Id_Materia" });
            DropIndex("dbo.Notas", new[] { "Id_Estudiante" });
            DropIndex("dbo.EvaluacionesEstudiantes", new[] { "Id_Estudiante" });
            DropIndex("dbo.EvaluacionesEstudiantes", new[] { "Id_Evaluaciones" });
            DropIndex("dbo.Evaluaciones", new[] { "Id_TipoEvaluacion" });
            DropIndex("dbo.Evaluaciones", new[] { "Id_CursoA" });
            DropIndex("dbo.Evaluaciones", new[] { "Id_Materia" });
            DropIndex("dbo.EmpleadoCalendarios", new[] { "Id_Curso" });
            DropIndex("dbo.EmpleadoCalendarios", new[] { "Id_Periodo" });
            DropIndex("dbo.CursoEstudiantes", new[] { "Id_Estudiante" });
            DropIndex("dbo.CursoEstudiantes", new[] { "Id_Curso" });
            DropIndex("dbo.Materias", new[] { "Id_AñoEscolar" });
            DropIndex("dbo.HistorialAñosCursado", new[] { "Id_Anio" });
            DropIndex("dbo.HistorialAñosCursado", new[] { "Id_Escuela" });
            DropIndex("dbo.HistorialAcademicoes", new[] { "Id_Estudiante" });
            DropIndex("dbo.FamiliarEstudiantes", new[] { "Id_Estudiante" });
            DropIndex("dbo.EnfermedadEstudiantes", new[] { "Id_Medicacion" });
            DropIndex("dbo.EnfermedadEstudiantes", new[] { "Id_Enfermedad" });
            DropIndex("dbo.EnfermedadEstudiantes", new[] { "Id_Estudiante" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Estudios_Maestro", new[] { "Id_Empleado" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Id_Empleado" });
            DropIndex("dbo.ControlDisciplinarios", new[] { "Id" });
            DropIndex("dbo.ControlDisciplinarios", new[] { "Id_Estudiantes" });
            DropIndex("dbo.Estudiantes", new[] { "Id" });
            DropIndex("dbo.CursoEscolars", new[] { "Id_Seccion" });
            DropIndex("dbo.CursoEscolars", new[] { "Id_Año" });
            DropIndex("dbo.CursoEscolars", new[] { "Id_Modalidad" });
            DropIndex("dbo.Curso_Asignaturas", new[] { "Id_Empleado" });
            DropIndex("dbo.Curso_Asignaturas", new[] { "Id_Materia" });
            DropIndex("dbo.Curso_Asignaturas", new[] { "Id_Curso" });
            DropIndex("dbo.CalendarioCursoes", new[] { "Id_CursoAsignatura" });
            DropIndex("dbo.CalendarioCursoes", new[] { "Id_Periodo" });
            DropIndex("dbo.CalendarioCursoes", new[] { "Id_Curso" });
            DropTable("dbo.PermisosYRoles");
            DropTable("dbo.Permisos");
            DropTable("dbo.EmpleadoMaterias");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.OpcionesDeColegios");
            DropTable("dbo.SemestresEscolares");
            DropTable("dbo.Parciales");
            DropTable("dbo.Notas");
            DropTable("dbo.EvaluacionesEstudiantes");
            DropTable("dbo.TipoEvaluacions");
            DropTable("dbo.Evaluaciones");
            DropTable("dbo.EmpleadoCalendarios");
            DropTable("dbo.CursoEstudiantes");
            DropTable("dbo.PeriodosEscolares");
            DropTable("dbo.Materias");
            DropTable("dbo.Secciones");
            DropTable("dbo.Modalidades");
            DropTable("dbo.HistorialAñosCursado");
            DropTable("dbo.HistorialAcademicoes");
            DropTable("dbo.FamiliarEstudiantes");
            DropTable("dbo.TPMedicacions");
            DropTable("dbo.Enfermedads");
            DropTable("dbo.EnfermedadEstudiantes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Estudios_Maestro");
            DropTable("dbo.Empleadoes");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ControlDisciplinarios");
            DropTable("dbo.Estudiantes");
            DropTable("dbo.AniosACursars");
            DropTable("dbo.CursoEscolars");
            DropTable("dbo.Curso_Asignaturas");
            DropTable("dbo.CalendarioCursoes");
            DropTable("dbo.AñosEscolares");
        }
    }
}
