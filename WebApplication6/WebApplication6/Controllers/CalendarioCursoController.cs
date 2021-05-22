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
            var calendarioCursos = db.CalendarioCursos.Include(c => c.Curso_Asignaturas).Include(c => c.CursoEscolar).Include(c => c.PeriodosEscolares)
                .Include(x=>x.Curso_Asignaturas.Materias).WhereIf(!string.IsNullOrEmpty(idCurso.ToString()),x=>x.Id_Curso==idCurso).Where(x=>x.Bl_Estado==true);
            auxid = idCurso;
            ViewBag.NombreCurso = db.tbCursoEscolar.Find(idCurso).NombredeCurso;
            return View(await calendarioCursos.ToListAsync());
        }

        // GET: CalendarioCurso/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.Include(x=>x.CursoEscolar)
                .Include(x=>x.Curso_Asignaturas.Materias).Include(x=>x.PeriodosEscolares).
                Where(x=>x.Id_CalendarioCurso==id).FirstAsync();

            if (calendarioCurso == null)
            {
                return HttpNotFound();
            }
            return View(calendarioCurso);
        }
        public ActionResult Create()
        {
            List<PeriodosEscolares> periodosEscolares = db.PeriodosEscolares.ToList();
            List<PeriodosEscolares> periodosEscolares2 = db.PeriodosEscolares.ToList();
            List<CalendarioCurso> calendarioCursos = db.CalendarioCursos.Where(x=>x.Id_Curso==auxid).Where(x=>x.Bl_Estado==true).ToList();
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
   
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id_CalendarioCurso,Id_Curso,Id_Periodo,Id_CursoAsignatura")] CalendarioCurso calendarioCurso)
        {
            var respuesta = new Validador.BaseRespuesta { Mensaje = "Digito campos vacios " };
            calendarioCurso.Id_Curso = auxid;
            if (ModelState.IsValid)
            {
                var cursoasignatura = db.tbCursoAsignaturas.Find(calendarioCurso.Id_CursoAsignatura);
                var Empleadoocupado = db.EmpleadoCalendarios.Where(x => x.Curso_Asignaturas.Id_Empleado == cursoasignatura.Id_Empleado)
                    .Where(x => x.Id_Periodo == calendarioCurso.Id_Periodo);

                if (Empleadoocupado.Count()==0)
                {
                    calendarioCurso.Bl_Estado = true;
                    db.CalendarioCursos.Add(calendarioCurso);
                    EmpleadoCalendario empleadoCalendario = new EmpleadoCalendario();
                    empleadoCalendario.Id_Curso = calendarioCurso.Id_CursoAsignatura;
                    empleadoCalendario.Id_Periodo = calendarioCurso.Id_Periodo;
                    db.EmpleadoCalendarios.Add(empleadoCalendario);
                    await db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.Mensaje = "Se agrego correctamente la hora en el calendario";
                    return Json(respuesta);
                }
                else
                {
                    respuesta.Mensaje = "Ya esta ocupado ese maestro en ese tiempo";
                    return Json(respuesta);
                }
                
            }
            TempData["CursoOcupado"] = false;
            return RedirectToAction("Index", "CalendarioCurso", new { idCurso = auxid });
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.Include(x=>x.Curso_Asignaturas.Materias)
                .Include(x=>x.PeriodosEscolares).Where(x=>x.Id_CalendarioCurso==id).FirstAsync();
            if (calendarioCurso == null)
            {
                return HttpNotFound();
            }
            return View(calendarioCurso);
        }

        // POST: CalendarioCurso/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id, bool estado)
        {
            CalendarioCurso calendarioCurso = await db.CalendarioCursos.FindAsync(id);
            var cursoasignatura = db.tbCursoAsignaturas.Find(calendarioCurso.Id_CursoAsignatura);
            var Empleadoocupado = db.EmpleadoCalendarios.Where(x => x.Curso_Asignaturas.Id_Empleado == cursoasignatura.Id_Empleado)
                .Where(x => x.Id_Periodo == calendarioCurso.Id_Periodo).First(); ;
            var respuesta = new Validador.BaseRespuesta();
            if (estado==true)
            {
                var DTactual = DateTime.Now;
                var DTSemestreActual = db.SemestresEscolares.Where(x => x.EstadoSemestre == true).Where(x=>x.FchInicio>=DTactual)
                    .Where(x=>x.FchFinal<=DTactual);
                if (DTSemestreActual.Count() == 0)
                {
                    respuesta.Mensaje = "Actualmente se esta en medio semestre!";

                }
                else
                {
              
                    db.CalendarioCursos.Remove(calendarioCurso);
                    db.EmpleadoCalendarios.Remove(Empleadoocupado);
                    await db.SaveChangesAsync();
                    respuesta.Mensaje = "Se borro corectamente!";
                    respuesta.Ok = true;
                }

            }
            else if (estado==false)
            {
                calendarioCurso.Bl_Estado = false;
                db.Entry(calendarioCurso).State = EntityState.Modified;
                db.EmpleadoCalendarios.Remove(Empleadoocupado); 
                await db.SaveChangesAsync();
                respuesta.Mensaje = "Se desactivo corectamente!";
                respuesta.Ok = true;
            }
            await db.SaveChangesAsync();
            return Json(respuesta);
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
