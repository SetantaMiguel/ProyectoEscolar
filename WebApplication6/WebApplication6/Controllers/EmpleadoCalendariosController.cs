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

namespace WebApplication6.Controllers
{   
    [Authorize]
    public class EmpleadoCalendariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmpleadoCalendarios
        public async Task<ActionResult> Index(int id)
        {
            var empleadoCalendarios = db.EmpleadoCalendarios.Include(e => e.Curso_Asignaturas).Include(x=>x.Curso_Asignaturas.CursoEscolar).Include(x=>x.Curso_Asignaturas.Materias).Include(e => e.PeriodosEscolares).Where(x=>x.Curso_Asignaturas.Id_Empleado==id);
            return View(await empleadoCalendarios.ToListAsync());
        }

        // GET: EmpleadoCalendarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoCalendario empleadoCalendario = await db.EmpleadoCalendarios.FindAsync(id);
            if (empleadoCalendario == null)
            {
                return HttpNotFound();
            }
            return View(empleadoCalendario);
        }

        // GET: EmpleadoCalendarios/Create
        public ActionResult Create()
        {
            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas.Include(x=>x.CursoEscolar), "Id_Curso_Asignatura", "CursoEscolar.NombredeCurso");
            ViewBag.Id_Periodo = new SelectList(db.PeriodosEscolares, "Id_Periodo", "Nombre_Periodo");
            return View();
        }

        // POST: EmpleadoCalendarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_EmpleadoCalendario,Id_Periodo,Id_Curso")] EmpleadoCalendario empleadoCalendario)
        {
            if (ModelState.IsValid)
            {
                db.EmpleadoCalendarios.Add(empleadoCalendario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", empleadoCalendario.Id_Curso);
            ViewBag.Id_Periodo = new SelectList(db.PeriodosEscolares, "Id_Periodo", "DiaSemana", empleadoCalendario.Id_Periodo);
            return View(empleadoCalendario);
        }

        // GET: EmpleadoCalendarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoCalendario empleadoCalendario = await db.EmpleadoCalendarios.FindAsync(id);
            if (empleadoCalendario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", empleadoCalendario.Id_Curso);
            ViewBag.Id_Periodo = new SelectList(db.PeriodosEscolares, "Id_Periodo", "DiaSemana", empleadoCalendario.Id_Periodo);
            return View(empleadoCalendario);
        }

        // POST: EmpleadoCalendarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_EmpleadoCalendario,Id_Periodo,Id_Curso")] EmpleadoCalendario empleadoCalendario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleadoCalendario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", empleadoCalendario.Id_Curso);
            ViewBag.Id_Periodo = new SelectList(db.PeriodosEscolares, "Id_Periodo", "DiaSemana", empleadoCalendario.Id_Periodo);
            return View(empleadoCalendario);
        }

        // GET: EmpleadoCalendarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoCalendario empleadoCalendario = await db.EmpleadoCalendarios.FindAsync(id);
            if (empleadoCalendario == null)
            {
                return HttpNotFound();
            }
            return View(empleadoCalendario);
        }

        // POST: EmpleadoCalendarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmpleadoCalendario empleadoCalendario = await db.EmpleadoCalendarios.FindAsync(id);
            db.EmpleadoCalendarios.Remove(empleadoCalendario);
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
    }
}
