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
    public class EvaluacionesEstudiantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EvaluacionesEstudiantes
        public async Task<ActionResult> Index(int? idCurso,int idEst=0,int idMat=0,int idParcial=0)
        {
            ViewBag.idCurso = idCurso;
            if (idMat!=0)
            {
                ViewBag.idMat = idMat;
            }
            ViewBag.NMateria = db.tbmaterias.Find(idMat).Nombre_Materia;
            if (idParcial!=0)
            {
                var ParcialActivo =   db.Parciales.Find(idParcial);
                    var EstadoAprobado = await  db.Notas.Where(x => x.Id_Parcial == idParcial).Where(x => x.Id_Estudiante == idEst).Where(x => x.Id_Materia == idMat)
                    .FirstOrDefaultAsync();
                ViewBag.ParcialAprobado = EstadoAprobado.Bl_Aprobado;
                ViewBag.NotaFinal = EstadoAprobado.Nota;
                ViewBag.NParcial = ParcialActivo.Nombre_Parcial ;
            }
            else
            {
                ViewBag.NParcial = "Todos los parciales ";
            }
            ViewBag.NEstudiante = db.tbestudiantes.Find(idEst).NombreCompleto;
            var evaluacionesEstudiantes = db.EvaluacionesEstudiantes.Include(e => e.Estudiantes).Include(e => e.Evaluaciones).Include(x=>x.Evaluaciones.Materias).Include(x=>x.Evaluaciones.TipoEvaluacion)
                .WhereIf(idEst!=0,x=>x.Id_Estudiante==idEst).WhereIf(idMat!=0,x=>x.Evaluaciones.Id_Materia==idMat)
                .WhereIf(idParcial!=0,x=>x.Evaluaciones.Id_Parciales==idParcial);
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
            EvaluacionesEstudiantes evaluacionesEstudiantes = await db.EvaluacionesEstudiantes.Include(x=>x.Evaluaciones).Include(x=>x.Estudiantes)
                .Include(x=>x.Evaluaciones.TipoEvaluacion).
                Where(x=>x.Id_EvaluacionesEstudiantes==id).FirstOrDefaultAsync();
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
        public async Task<ActionResult> Edit([Bind(Include = "Id_EvaluacionesEstudiantes,Resultado")] EvaluacionesEstudiantes evaluacionesEstudiantes)
        {
            if (ModelState.IsValid)
            {
                var evaluacionesEstudiantes1 = db.EvaluacionesEstudiantes.Find(evaluacionesEstudiantes.Id_EvaluacionesEstudiantes);

                evaluacionesEstudiantes1.EstadoEvaluado = true;
                evaluacionesEstudiantes1.Resultado = evaluacionesEstudiantes.Resultado;

                //CambiaR la nota
                var ParcialActivo= db.Parciales.Where(x => x.Estado_Parcial == true).Where(x => x.Dt_Inicio <= DateTime.Now && DateTime.Now <= x.Dt_Final).Select(x => x.Id_Parcial).FirstOrDefault();
                var materia = db.Evaluaciones.Find(evaluacionesEstudiantes1.Id_Evaluaciones).Id_Materia;
                var Estudiante = db.Notas.Where(x => x.Id_Estudiante == evaluacionesEstudiantes1.Id_Estudiante).Where(x => x.Id_Materia == materia).
                    Where(x => x.Id_Parcial == ParcialActivo).FirstOrDefault();

                Estudiante.Nota += evaluacionesEstudiantes1.Resultado;

                db.Entry(Estudiante).State = EntityState.Modified;

                db.Entry(evaluacionesEstudiantes1).State = EntityState.Modified;
                
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Notas", new { idCurso=Estudiante.Id_CursoEscolar,idMat=Estudiante.Id_Materia}) ;
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
