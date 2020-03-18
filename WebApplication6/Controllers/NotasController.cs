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
    public class NotasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxid=0;

        // GET: Notas
        public async Task<ActionResult> Index(int idCurso)
            {
            auxid = idCurso;
            ViewBag.Id = auxid;
            var notas = db.Notas.Include(n => n.Curso_Asignaturas.Materias).Include(n => n.Estudiantes)
                .Include(x=>x.Curso_Asignaturas).
                WhereIf(!string.IsNullOrEmpty(idCurso.ToString()),x=>x.Curso_Asignaturas.Id_Curso==idCurso);
            return View(await notas.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult> Index(int NumMateria,int id=0)
        {
            ViewBag.Id = auxid;
            var notas = db.Notas.Include(n => n.Curso_Asignaturas.Materias).Include(n => n.Estudiantes).
                WhereIf(!string.IsNullOrEmpty(NumMateria.ToString()), x => x.Curso_Asignaturas.Id_Materia == NumMateria).Where(x=>x.Id_Curso==auxid); ;
            return View(await notas.ToListAsync());
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
            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura");
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre");
            return View();
        }

        // POST: Notas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Nota,Id_Estudiante,Id_Curso,Nota1,Nota2,Nota3,Nota4,promedio,Bl_Aprobado")] Notas notas)
        {
            if (ModelState.IsValid)
            {
                db.Notas.Add(notas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", notas.Id_Curso);
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", notas.Id_Estudiante);
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
            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", notas.Id_Curso);
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", notas.Id_Estudiante);
            return View(notas);
        }

        // POST: Notas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Nota,Id_Estudiante,Id_Curso,Nota1,Nota2,Nota3,Nota4,promedio,Bl_Aprobado")] Notas notas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Curso = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", notas.Id_Curso);
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", notas.Id_Estudiante);
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
            var materias = db.tbCursoAsignaturas.Include(x => x.Materias).Where(x => x.Id_Curso== auxid).ToList();
            return Json(materias);
        }
    }
}
