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
using Microsoft.AspNet.Identity;

namespace WebApplication6.Controllers
{
    public class EvaluacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxidCurso;
        private static int auxidCurso2;
        // GET: Evaluaciones
        public async Task<ActionResult> Index(int idCurso)
        {
            auxidCurso = idCurso;
            ViewBag.IdCurso = idCurso;
            var evaluaciones = db.Evaluaciones.Include(e => e.Curso_Asignaturas).Include(x=>x.TipoEvaluacion).Include(e => e.Materias).Where(x=>x.Curso_Asignaturas.Id_Curso==idCurso).Where(x=>x.BL_Aprobado==false);
            return View(await evaluaciones.ToListAsync());
        }

        // GET: Evaluaciones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluaciones evaluaciones = await db.Evaluaciones.FindAsync(id);
            if (evaluaciones == null)
            {
                return HttpNotFound();
            }
            return View(evaluaciones);
        }

        // GET: Evaluaciones/Create
        public ActionResult Create()
        {


            List<Curso_Asignaturas> curso_Asignaturas = db.tbCursoAsignaturas.Include(c => c.CursoEscolar).Where(x => x.Id_Curso == auxidCurso).ToList();
            ViewBag.Id_CursoA = new SelectList(curso_Asignaturas, "Id_Curso_Asignatura", "CursoEscolar.NombredeCurso");
            ViewBag.Id_TipoEvaluacion = new SelectList(db.TipoEvaluacions, "Id_TipoEvaluacion", "strTipoEvaluacion");
            ViewBag.Id_Materia = new SelectList(db.tbCursoAsignaturas.Include(c => c.CursoEscolar).Include(x=>x.Materias).Where(x => x.Id_Curso == auxidCurso).ToList(), "Materias.Id", "Materias.Nombre_Materia");
            return View();
        }

        // POST: Evaluaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Evaluacion,Id_Materia,Id_CursoA,Fecha_Comienzo,Fecha_Final,Descripcion,ValorTotal,Id_TipoEvaluacion")] Evaluaciones evaluaciones,int Num_Evaluacion)
        {
            
            if (ModelState.IsValid)
            {
                evaluaciones.Num_Evaluacion = Num_Evaluacion;
                db.Evaluaciones.Add(evaluaciones);
                await db.SaveChangesAsync();
                var estudiantes = db.CursoEstudiantes.Where(x => x.Id_Curso == auxidCurso);
                return RedirectToAction("Index",new { idCurso=auxidCurso });
            }
            ViewBag.Id_TipoEvaluacion = new SelectList(db.TipoEvaluacions, "Id_TipoEvaluacion", "strTipoEvaluacion");
            ViewBag.Id_CursoA = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", evaluaciones.Id_CursoA);
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id", "Codigo_Materia", evaluaciones.Id_Materia);
            return View(evaluaciones);
        }

        // GET: Evaluaciones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluaciones evaluaciones = await db.Evaluaciones.FindAsync(id);
            if (evaluaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_CursoA = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", evaluaciones.Id_CursoA);
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id", "Codigo_Materia", evaluaciones.Id_Materia);
            return View(evaluaciones);
        }

        // POST: Evaluaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Evaluacion,Id_Materia,Id_CursoA,Fecha_Comienzo,Fecha_Final,Descripcion,ValorTotal")] Evaluaciones evaluaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evaluaciones).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_CursoA = new SelectList(db.tbCursoAsignaturas, "Id_Curso_Asignatura", "Id_Curso_Asignatura", evaluaciones.Id_CursoA);
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id", "Codigo_Materia", evaluaciones.Id_Materia);
            return View(evaluaciones);
        }

        // GET: Evaluaciones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluaciones evaluaciones = await db.Evaluaciones.FindAsync(id);
            if (evaluaciones == null)
            {
                return HttpNotFound();
            }
            return View(evaluaciones);
        }

        // POST: Evaluaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Evaluaciones evaluaciones = await db.Evaluaciones.FindAsync(id);
            db.Evaluaciones.Remove(evaluaciones);
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

        public ActionResult AprobarCurso(int? id, bool? estado)
        {
            if (id!=null)
            {
             auxidCurso2 = Convert.ToInt32(id);  
    
            }
            if (Validador.PuedeEntrar(User.Identity.GetUserId(), "Aprobar Cursos")&&estado!=null)
            {
                
              
                if (estado == true)
                {
                    Evaluaciones evaluaciones = db.Evaluaciones.Find(auxidCurso2);
                    evaluaciones.BL_Aprobado = true;
                    if (ModelState.IsValid)
                    {
                        db.Entry(evaluaciones).State = EntityState.Modified;
                        db.SaveChanges();
                        var EstudiantesCurso = db.CursoEstudiantes.Where(x => x.Id_Curso == auxidCurso);
                        foreach (var item in EstudiantesCurso)
                        {
                            EvaluacionesEstudiantes evaluacionesEstudiantes = new EvaluacionesEstudiantes();
                            evaluacionesEstudiantes.Id_Estudiante = item.Id_Estudiante;
                            evaluacionesEstudiantes.Id_Evaluaciones = auxidCurso2;
                            evaluacionesEstudiantes.Resultado = 0;
                            db.EvaluacionesEstudiantes.Add(evaluacionesEstudiantes);
                        }
                        db.SaveChanges();

                    }
                    return RedirectToAction("Index", new { idCurso = auxidCurso });
                }
                else
                {
                    Evaluaciones evaluaciones = db.Evaluaciones.Find(auxidCurso2);
                    db.Evaluaciones.Remove(evaluaciones);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idCurso = auxidCurso });
                }

            }
            return PartialView();
        }


    }
}
