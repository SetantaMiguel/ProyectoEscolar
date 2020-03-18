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
{   [Authorize]
    public class CalendarioCursoController : Controller
    {
        static private int auxid = 0;
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarioCurso
        public async Task<ActionResult> Index(int idCurso)
        {

            if (TempData["CursoOcupado"] != null)
            {
                ViewBag.CursoOcupado = true;

            }
            var calendarioCursos = db.CalendarioCursos.Include(c => c.Curso_Asignaturas).Include(c => c.CursoEscolar).Include(c => c.PeriodosEscolares).Include(x=>x.Curso_Asignaturas.Materias).WhereIf(!string.IsNullOrEmpty(idCurso.ToString()),x=>x.Id_Curso==idCurso);
            auxid = idCurso;
            return View(await calendarioCursos.ToListAsync());
        }

        // GET: CalendarioCurso/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.FindAsync(id);
            if (calendarioCurso == null)
            {
                return HttpNotFound();
            }
            return View(calendarioCurso);
        }

        // GET: CalendarioCurso/Create
        public ActionResult Create()
        {
            List<PeriodosEscolares> periodosEscolares = db.PeriodosEscolares.ToList();
            List<PeriodosEscolares> periodosEscolares2 = db.PeriodosEscolares.ToList();
            List<CalendarioCurso> calendarioCursos = db.CalendarioCursos.Where(x=>x.Id_Curso==auxid).ToList();
            foreach (var item in calendarioCursos)
            {
                foreach (var item2 in periodosEscolares)
                {
                    if (item.Id_Periodo==item2.Id_Periodo)
                    {
                        periodosEscolares2.Remove(item2);
                    }
                }
            }
            ViewBag.Id_CursoAsignatura = new SelectList(db.tbCursoAsignaturas.Include(x=>x.Materias).Where(x=>x.Id_Curso==auxid), "Id_Curso_Asignatura", "Materias.Nombre_Materia");
            ViewBag.Id_Periodo = new SelectList(periodosEscolares2, "Id_Periodo", "Nombre_Periodo");
            return View();
        }

        // POST: CalendarioCurso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public async Task<ActionResult> Create([Bind(Include = "Id_CalendarioCurso,Id_Curso,Id_Periodo,Id_CursoAsignatura")] CalendarioCurso calendarioCurso)
        {
            calendarioCurso.Id_Curso = auxid;
            if (ModelState.IsValid)
            {
                var cursoasignatura = db.tbCursoAsignaturas.Find(calendarioCurso.Id_CursoAsignatura);
               int empleado = db.TbEmpleado.Find(cursoasignatura.Id_Empleado).Id;
                var Empleadoocupado = db.EmpleadoCalendarios.Where(x => x.Curso_Asignaturas.Id_Empleado == empleado).Where(x => x.Id_Periodo == calendarioCurso.Id_Periodo);
                if (Empleadoocupado.Count()==0)
                {
                    db.CalendarioCursos.Add(calendarioCurso);
                    EmpleadoCalendario empleadoCalendario = new EmpleadoCalendario();
                    empleadoCalendario.Id_Curso = calendarioCurso.Id_CursoAsignatura;
                    empleadoCalendario.Id_Periodo = calendarioCurso.Id_Periodo;
                    db.EmpleadoCalendarios.Add(empleadoCalendario);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", new { idCurso = auxid });
                }
            }
            TempData["CursoOcupado"] = false;
            return RedirectToAction("Index", "CalendarioCurso", new { idCurso = auxid });
        }

        // GET: CalendarioCurso/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.FindAsync(id);
            if (calendarioCurso == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_CursoAsignatura = new SelectList(db.tbCursoAsignaturas.Include(x=>x.Materias), "Id_Curso_Asignatura", "Materias.Nombre_Materia", calendarioCurso.Id_CursoAsignatura);
            ViewBag.Id_Curso = new SelectList(db.tbCursoEscolar, "Id_Curso", "NombredeCurso", calendarioCurso.Id_Curso);
            ViewBag.Id_Periodo = new SelectList(db.PeriodosEscolares, "Id_Periodo", "Nombre_Periodo", calendarioCurso.Id_Periodo);
            return PartialView(calendarioCurso);
        }

        // POST: CalendarioCurso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_CalendarioCurso,Id_Curso,Id_Periodo,Id_CursoAsignatura")] CalendarioCurso calendarioCurso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendarioCurso).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_CursoAsignatura = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", calendarioCurso.Id_CursoAsignatura);
            ViewBag.Id_Curso = new SelectList(db.tbCursoEscolar, "Id_Curso", "NombredeCurso", calendarioCurso.Id_Curso);
            ViewBag.Id_Periodo = new SelectList(db.PeriodosEscolares, "Id_Periodo", "DiaSemana", calendarioCurso.Id_Periodo);
            return View(calendarioCurso);
        }

        // GET: CalendarioCurso/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.FindAsync(id);
            if (calendarioCurso == null)
            {
                return HttpNotFound();
            }
            return View(calendarioCurso);
        }

        // POST: CalendarioCurso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.FindAsync(id);
            db.CalendarioCursos.Remove(calendarioCurso);
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

        public JsonResult Test(int id_Periodo,int Id_CursoAsignatura)
        {
            string[] array = { "XD" };
            return Json(array);
        }

        
    }
}
