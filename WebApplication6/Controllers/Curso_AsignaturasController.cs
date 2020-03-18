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
    public class Curso_AsignaturasController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int idCurso = 0;

        // GET: Curso_Asignaturas
        public async Task<ActionResult> Index(int IdCurso = 0)
        {
            if (IdCurso != 0)
            {
                idCurso = IdCurso;
                var tbCursoAsignaturas = db.tbCursoAsignaturas.Include(c => c.CursoEscolar).Include(c => c.Empleado).Include(c => c.Materias).Where(x => x.Id_Curso == IdCurso);
                ViewBag.NombreCurso = db.tbCursoEscolar.Find(IdCurso).NombredeCurso;
                ViewBag.Id_Curso = IdCurso;
                return PartialView(tbCursoAsignaturas.ToList());
            }
            else
            {
                var tbCursoAsignaturas = db.tbCursoAsignaturas.Include(c => c.CursoEscolar).Include(c => c.Empleado).Include(c => c.Materias);
                return PartialView(await tbCursoAsignaturas.ToListAsync());
            }


        }

        // GET: Curso_Asignaturas/Create
        public ActionResult Create()
        {
            SelectList listItems;
            List<Curso_Asignaturas> materiasdelcuro = db.tbCursoAsignaturas.Where(x => x.Id_Curso == idCurso).ToList();
            List<Materias> materiastotales = db.tbmaterias.ToList();
            List<Materias> materiastotales2 = db.tbmaterias.ToList();
            foreach (var item in materiastotales)
            {
                foreach (var item2 in materiasdelcuro)
                {
                    if (item.Id == item2.Id_Materia)
                    {
                        materiastotales2.Remove(item);
                    }
                }
            }
            listItems = new SelectList(materiastotales2, "Id", "Nombre_Materia");
            ViewBag.Id_Materia = listItems;
            return PartialView();

        }

        // POST: Curso_Asignaturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Curso_Asignatura,Id_Curso,Id_Materia,Id_Empleado")] Curso_Asignaturas curso_Asignaturas)
        {
            if (ModelState.IsValid)
            {
                curso_Asignaturas.Id_Curso = idCurso;
                db.tbCursoAsignaturas.Add(curso_Asignaturas);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "CursoEscolars",new { id=curso_Asignaturas.Id_Curso});
            }
            return View(curso_Asignaturas);
        }

        // GET: Curso_Asignaturas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso_Asignaturas curso_Asignaturas = await db.tbCursoAsignaturas.Include(x=>x.Materias).Include(x=>x.CursoEscolar).FirstAsync();
            if (curso_Asignaturas == null)
            {
                return HttpNotFound();
            }
            return View(curso_Asignaturas);
        }

        // POST: Curso_Asignaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Curso_Asignaturas curso_Asignaturas = await db.tbCursoAsignaturas.FindAsync(id);
            db.tbCursoAsignaturas.Remove(curso_Asignaturas);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "CursoEscolars", new { id = curso_Asignaturas.Id_Curso });
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
        public JsonResult EmpleadosHabiles(int IdMateria)
        {
            var empleados = db.tbEmpleadoMaterias.Include(x => x.Empleado).Where(x => x.Id_Materia == IdMateria).ToList();
            return Json(empleados);
        }

    }
}
