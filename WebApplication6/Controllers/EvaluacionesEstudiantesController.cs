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
    public class EvaluacionesEstudiantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxCurso;
        private static int auxEst;

        // GET: EvaluacionesEstudiantes
        public async Task<ActionResult> Index(int idEst=0,int idCursoA=0,int num=0)
        {
            auxCurso = idCursoA;
            auxEst = idEst;
            var evaluacionesEstudiantes = db.EvaluacionesEstudiantes.Include(e => e.Estudiantes).Include(e => e.Evaluaciones).Include(x=>x.Evaluaciones.Materias)
                .WhereIf(idEst!=0,x=>x.Id_Estudiante==idEst).WhereIf(idCursoA!=0,x=>x.Evaluaciones.Id_CursoA==idCursoA)
                .WhereIf(num!=0,x=>x.Evaluaciones.Num_Evaluacion==num);
            return View(await evaluacionesEstudiantes.ToListAsync());
        }

        // GET: EvaluacionesEstudiantes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluacionesEstudiantes evaluacionesEstudiantes = await db.EvaluacionesEstudiantes.FindAsync(id);
            if (evaluacionesEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(evaluacionesEstudiantes);
        }

        // GET: EvaluacionesEstudiantes/Create
        public ActionResult Create()
        {
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre");
            ViewBag.Id_Evaluaciones = new SelectList(db.Evaluaciones, "Id_Evaluacion", "Descripcion");
            return View();
        }

        // POST: EvaluacionesEstudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_EvaluacionesEstudiantes,Id_Evaluaciones,Id_Estudiante,Resultado")] EvaluacionesEstudiantes evaluacionesEstudiantes)
        {
            if (ModelState.IsValid)
            {
                db.EvaluacionesEstudiantes.Add(evaluacionesEstudiantes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", evaluacionesEstudiantes.Id_Estudiante);
            ViewBag.Id_Evaluaciones = new SelectList(db.Evaluaciones, "Id_Evaluacion", "Descripcion", evaluacionesEstudiantes.Id_Evaluaciones);
            return View(evaluacionesEstudiantes);
        }

        // GET: EvaluacionesEstudiantes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluacionesEstudiantes evaluacionesEstudiantes = await db.EvaluacionesEstudiantes.Include(x => x.Estudiantes).Include(x => x.Evaluaciones)
                .Where(x => x.Id_EvaluacionesEstudiantes == id).FirstAsync();        
            if (evaluacionesEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(evaluacionesEstudiantes);
        }

        // POST: EvaluacionesEstudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_EvaluacionesEstudiantes,Id_Evaluaciones,Id_Estudiante,Resultado")] EvaluacionesEstudiantes evaluacionesEstudiantes,int NParcial)
        {
            if (ModelState.IsValid)
            {
                EvaluacionesEstudiantes evaluacionesEstudiantes2 = db.EvaluacionesEstudiantes.Include(x => x.Evaluaciones).Where(x => x.Id_EvaluacionesEstudiantes == evaluacionesEstudiantes.Id_EvaluacionesEstudiantes).First() ;
                evaluacionesEstudiantes2.Resultado = evaluacionesEstudiantes.Resultado;
                db.Entry(evaluacionesEstudiantes2).State = EntityState.Modified;
                await db.SaveChangesAsync();
                var nota = db.Notas.Where(x => x.Id_Curso == evaluacionesEstudiantes2.Evaluaciones.Id_CursoA).First();
                switch (NParcial)
                {
                    case 1:
                        nota.Nota1 += evaluacionesEstudiantes2.Resultado;
                    break;
                    case 2:
                        nota.Nota2 += evaluacionesEstudiantes2.Resultado;
                        break;
                    case 3:
                        nota.Nota3 += evaluacionesEstudiantes2.Resultado;
                        break;
                    case 4:
                        nota.Nota4 += evaluacionesEstudiantes2.Resultado;
                        break;
                }
                db.Entry(nota).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index",new {idEst=auxEst,idCursoA=auxCurso });
            }
            return View(evaluacionesEstudiantes);
        }

        // GET: EvaluacionesEstudiantes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluacionesEstudiantes evaluacionesEstudiantes = await db.EvaluacionesEstudiantes.FindAsync(id);
            if (evaluacionesEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(evaluacionesEstudiantes);
        }

        // POST: EvaluacionesEstudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EvaluacionesEstudiantes evaluacionesEstudiantes = await db.EvaluacionesEstudiantes.FindAsync(id);
            db.EvaluacionesEstudiantes.Remove(evaluacionesEstudiantes);
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
