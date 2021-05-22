using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;
using Microsoft.AspNet.Identity;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class NotasController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notas
        public async Task<ActionResult> Index(int idCurso = 0,int idMat=0, int pagina = 1)
        {
            //Obtengo el usuario para saber que profesor entra
            var usuario = db.Users.Find(User.Identity.GetUserId());

            var roles=usuario.Roles;
            var listaCursosAsignaturas = db.tbCursoAsignaturas.Include(x => x.Materias).Where(x => x.Id_Empleado == usuario.Id_Empleado)
                .WhereIf(idMat!=0,x=>x.Id_Materia==idMat)
                .Where(x => x.Estado_Asignatura == true).ToList();

           
            if (listaCursosAsignaturas.Count != 0)
            {
                ViewBag.HayClaseHabil = true;
            } 
            else
            {
                ViewBag.HayClaseHabil = false;
            }

            //lista de parciales activos para la tabla tenga una mejor estetica
            var listaParciales = await db.Parciales.Where(x => x.Estado_Parcial == true).Select(x => x.Nombre_Parcial).ToListAsync();
            ViewBag.Parciales = listaParciales;
            //identifico el parcial activo
            ViewBag.ParcialActivo = db.Parciales.Where(x => x.Estado_Parcial == true).Where(x => x.Dt_Inicio <= DateTime.Now && DateTime.Now <= x.Dt_Final).Select(x => x.Id_Parcial).FirstOrDefault();
            int idparcialActivo = Convert.ToInt32(ViewBag.ParcialActivo);
            //la cantidad de registro por paginas
            int cantidadRegistrosPorPagina = 12;

            //Este if es para saber si el admin esta entrando a ver todas las notas
            var modelo = new IndexViewModel2();
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Notas"))
            {

                var notas = await db.Notas.Include(n => n.Materias).Include(n => n.Estudiantes).Include(x => x.Parciales)
                    .WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .Where(x => x.Id_Parcial == idparcialActivo)
                    .OrderBy(x => x.Id_Nota).Skip((pagina - 1) * cantidadRegistrosPorPagina)
                    .Take(cantidadRegistrosPorPagina)
                    .ToListAsync();

                var totalDeRegistros = db.Notas
                    .WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .Where(x => x.Id_Parcial == idparcialActivo).Count();

                ViewBag.id = idCurso;
                modelo.Notas = notas;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            }
            else if (ViewBag.HayClaseHabil == true)
            {
                /*
                    Genero una lista para rellenarla solo con los usuarios permitidos y luego la obtengo de poco a poco
                */
                List<Notas> notas = new List<Notas>();

                foreach (var item in listaCursosAsignaturas)
                {

                    var result = db.Notas.Include(n => n.Materias).Include(n => n.Estudiantes).Include(x => x.Parciales)
                    .WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .Where(x => x.Id_Parcial == idparcialActivo).Where(x => x.Id_Materia == item.Id_Materia).ToList();
                    foreach (var notas1 in result)
                    {
                        notas.Add(notas1);

                    }
                }

                IQueryable<Notas> notasFinales = notas.AsQueryable();
                var final = notasFinales.OrderBy(x => x.Id_Nota).Skip((pagina - 1) * cantidadRegistrosPorPagina)
                    .Take(cantidadRegistrosPorPagina)
                    .ToList();

                var totalDeRegistros = notas.WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .Where(x => x.Id_Parcial == idparcialActivo).Count();
                ViewBag.id = idCurso;
                modelo.Notas = final;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            }
            else
            {
                return RedirectToRoute("EjemploHome");
            }
            //pasar parametros para los graficos
            if (idCurso != 0)
            {
                Session["IdCurso"] = idCurso;
                Session["Anio"] = db.tbCursoEscolar.Find(idCurso).Id_Año;
                ViewBag.NCurso = db.tbCursoEscolar.Find(idCurso).NombredeCurso;
                ViewBag.Id = Session["IdCurso"];
            }
            return View(modelo);
        }

        [HttpPost]
        public async Task<ActionResult> Index(int? NumMateria, int? NumEstudiantes, int pagina = 1)
        {
            pagina = 1;
            ViewBag.ParcialActivo = db.Parciales.Where(x => x.Estado_Parcial == true).Where(x => x.Dt_Inicio <= DateTime.Now && DateTime.Now <= x.Dt_Final).Select(x => x.Id_Parcial).First();
            ViewBag.NCurso = db.tbCursoEscolar.Find(Convert.ToInt32(Session["IdCurso"])).NombredeCurso;
            ViewBag.Id = Session["IdCurso"];
            var listaParciales = await db.Parciales.Where(x => x.Estado_Parcial == true).Select(x => x.Nombre_Parcial).ToListAsync();
            ViewBag.Parciales = listaParciales;
            int cantidadRegistrosPorPagina = 12;
            int idCurso = Convert.ToInt32(ViewBag.Id);
            int idparcialActivo = Convert.ToInt32(ViewBag.ParcialActivo);


            //Obtengo el usuario para saber que profesor entra
            var usuario = db.Users.Find(User.Identity.GetUserId()).Id_Empleado;

            var listaCursosAsignaturas = db.tbCursoAsignaturas.Include(x => x.Materias).Where(x => x.Id_Empleado == usuario).Where(x => x.Estado_Asignatura == true).ToList();

            if (listaCursosAsignaturas.Count != 0)
            {
                ViewBag.HayClaseHabil = true;
            }
            else
            {
                ViewBag.HayClaseHabil = false;
            }

            var modelo = new IndexViewModel2();
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Notas"))
            {
                var notas = await db.Notas.Include(n => n.Materias).Include(n => n.Estudiantes).Include(x => x.Parciales)
                .WhereIf(!string.IsNullOrEmpty(NumMateria.ToString()), x => x.Materias.Id_Materia == NumMateria).
                 WhereIf(!string.IsNullOrEmpty(NumEstudiantes.ToString()), x => x.Id_Estudiante == NumEstudiantes)
                .WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                .Where(x => x.Id_Parcial == idparcialActivo)
                .OrderBy(x => x.Id_Nota).Skip((pagina - 1) * cantidadRegistrosPorPagina)
                .Take(cantidadRegistrosPorPagina)
                .ToListAsync();

                var totalDeRegistros = db.Notas
                     .WhereIf(!string.IsNullOrEmpty(NumMateria.ToString()), x => x.Materias.Id_Materia == NumMateria).
                    WhereIf(!string.IsNullOrEmpty(NumEstudiantes.ToString()), x => x.Id_Estudiante == NumEstudiantes)
                    .WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .Where(x => x.Id_Parcial == idparcialActivo).Count();

                ViewBag.id = idCurso;
                modelo.Notas = notas;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            }
            else if(ViewBag.HayClaseHabil == true)
            {
                /*
                    Genero una lista para rellenarla solo con los usuarios permitidos y luego la obtengo de poco a poco
                */
                List<Notas> notas = new List<Notas>();

                foreach (var item in listaCursosAsignaturas)
                {

                    var result = db.Notas.Include(n => n.Materias).Include(n => n.Estudiantes).Include(x => x.Parciales)
                    .WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .Where(x => x.Id_Parcial == idparcialActivo).Where(x => x.Id_Materia == item.Id_Materia).ToList();
                    foreach (var notas1 in result)
                    {
                        notas.Add(notas1);

                    }
                }

                IQueryable<Notas> notasFinales = notas.AsQueryable();
                var final = notasFinales
                    .WhereIf(!string.IsNullOrEmpty(NumMateria.ToString()), x => x.Materias.Id_Materia == NumMateria).
                    WhereIf(!string.IsNullOrEmpty(NumEstudiantes.ToString()), x => x.Id_Estudiante == NumEstudiantes).OrderBy(x => x.Id_Nota).Skip((pagina - 1) * cantidadRegistrosPorPagina)
                    .Take(cantidadRegistrosPorPagina)
                    .ToList();

                var totalDeRegistros = notas.WhereIf(idCurso != 0, x => x.Id_CursoEscolar == idCurso)
                    .WhereIf(!string.IsNullOrEmpty(NumMateria.ToString()), x => x.Materias.Id_Materia == NumMateria).
                    WhereIf(!string.IsNullOrEmpty(NumEstudiantes.ToString()), x => x.Id_Estudiante == NumEstudiantes)
                    .Where(x => x.Id_Parcial == idparcialActivo).Count();

                    ViewBag.id = idCurso;
                    modelo.Notas = final;
                    modelo.PaginaActual = pagina;
                    modelo.TotalRegistros = totalDeRegistros;
                    modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            }

            return View(modelo);
        }
        // GET: Notas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notas notas = await db.Notas.FindAsync(id);
            if (notas == null)
            {
                return HttpNotFound();
            }
            return View(notas);
        }

        // GET: Notas/Create
        public ActionResult Create()
        {
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre");
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia");
            ViewBag.Id_Parcial = new SelectList(db.Parciales, "Id_Parcial", "Nombre_Parcial");
            return View();
        }

        // POST: Notas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Nota,Id_Estudiante,Id_Materia,Id_Parcial,Bl_Aprobado")] Notas notas)
        {
            if (ModelState.IsValid)
            {
                db.Notas.Add(notas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", notas.Id_Estudiante);
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia", notas.Id_Materia);
            ViewBag.Id_Parcial = new SelectList(db.Parciales, "Id_Parcial", "Nombre_Parcial", notas.Id_Parcial);
            return View(notas);
        }

        // GET: Notas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notas notas = await db.Notas.FindAsync(id);
            if (notas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", notas.Id_Estudiante);
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia", notas.Id_Materia);
            ViewBag.Id_Parcial = new SelectList(db.Parciales, "Id_Parcial", "Nombre_Parcial", notas.Id_Parcial);
            return View(notas);
        }

        // POST: Notas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Nota,Id_Estudiante,Id_Materia,Id_Parcial,Bl_Aprobado")] Notas notas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", notas.Id_Estudiante);
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia", notas.Id_Materia);
            ViewBag.Id_Parcial = new SelectList(db.Parciales, "Id_Parcial", "Nombre_Parcial", notas.Id_Parcial);
            return View(notas);
        }

        // GET: Notas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notas notas = await db.Notas.FindAsync(id);
            if (notas == null)
            {
                return HttpNotFound();
            }
            return View(notas);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Notas notas = await db.Notas.FindAsync(id);
            db.Notas.Remove(notas);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult Materias()
        {
            int id = Convert.ToInt32(Session["Anio"]) ;
            var materias = db.tbmaterias.Where(x => x.Id_AñoEscolar == id).ToList();

            return Json(materias);
        }
        [HttpPost]
        public JsonResult Estudiantes()
        {
            int id = Convert.ToInt32(Session["IdCurso"]) ;
            var estudiantes = db.CursoEstudiantes.Include(x=>x.Estudiantes).Where(x => x.Id_Curso == id).OrderBy(x=>x.Estudiantes.Nombre).ToList();
            return Json(estudiantes);
        }

    }
}
