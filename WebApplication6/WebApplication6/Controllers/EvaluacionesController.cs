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
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{
    public class EvaluacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evaluaciones
        public async Task<ActionResult> Index(int idCurso,int pagina=1)
        {
            int cantidadRegistrosPorPagina = 100;
            TempData["idCurso"] = idCurso;
            int idParcialActivo= db.Parciales.Where(x => x.Estado_Parcial == true).Where(x => x.Dt_Inicio <= DateTime.Now && DateTime.Now <= x.Dt_Final).Select(x => x.Id_Parcial).FirstOrDefault();

            var evaluaciones = await db.Evaluaciones.OrderBy(x => x.Id_Evaluacion)
              .Skip((pagina - 1) * cantidadRegistrosPorPagina)
              .Take(cantidadRegistrosPorPagina).Include(e => e.CursoEscolar).Include(e => e.Materias).Include(e => e.TipoEvaluacion)
            .WhereIf(idCurso != null, x => x.Id_Curso == idCurso)
            .Where(x=>x.Id_Parciales==idParcialActivo).ToListAsync();

            var totalDeRegistros = db.Evaluaciones.WhereIf(idCurso != null, x => x.Id_Curso == idCurso).Count();
            var modelo = new IndexViewModel2();
            ViewBag.id = idCurso;
            modelo.Evaluaciones = evaluaciones;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            return View(modelo);

        }
        [HttpPost]
        public async Task<ActionResult> Index(int? idCurso, int? idMateria,int pagina=1)
        {
            int cantidadRegistrosPorPagina = 10;
            int idParcialActivo = db.Parciales.Where(x => x.Estado_Parcial == true).Where(x => x.Dt_Inicio <= DateTime.Now && DateTime.Now <= x.Dt_Final).Select(x => x.Id_Parcial).FirstOrDefault();

            var evaluaciones = await db.Evaluaciones.OrderBy(x => x.Id_Evaluacion)
              .Skip((pagina - 1) * cantidadRegistrosPorPagina)
              .Take(cantidadRegistrosPorPagina).Include(e => e.CursoEscolar).Include(e => e.Materias).Include(e => e.TipoEvaluacion)
            .WhereIf(idCurso != null, x => x.Id_Curso == idCurso).WhereIf(idMateria!=null,x=>x.Id_Materia==idMateria)
            .Where(x => x.Id_Parciales == idParcialActivo).ToListAsync();

            var totalDeRegistros = db.Evaluaciones.WhereIf(idCurso != null, x => x.Id_Curso == idCurso).Where(x => x.Id_Materia == idMateria).Count();
            var modelo = new IndexViewModel2();
            ViewBag.id = idCurso;
            modelo.Evaluaciones = evaluaciones;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            return View(modelo);

        }

        // GET: Evaluaciones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluaciones evaluaciones = await db.Evaluaciones.Include(x => x.CursoEscolar).Include(x => x.TipoEvaluacion).Include(x => x.Materias).Where(x => x.Id_Evaluacion == id).FirstAsync();
            if (evaluaciones == null)
            {
                return HttpNotFound();
            }
            return View(evaluaciones);
        }

        // GET: Evaluaciones/Create
        public ActionResult Create(int idCurso)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId()).Id_Empleado;

            var lista = db.tbCursoAsignaturas.Include(x => x.Materias).Where(x => x.Id_Empleado == usuario).Where(x => x.Estado_Asignatura == true).ToList();
            if (lista.Count != 0)
            {
                ViewBag.HayClase = true;
            }
            ViewBag.Id_Curso = new SelectList(db.tbCursoEscolar.Where(x => x.Id_Curso == idCurso), "Id_Curso", "NombredeCurso");
            ViewBag.Id_Materia = new SelectList(lista, "Materias.Id_Materia", "Materias.Nombre_Materia");
            ViewBag.Id_TipoEvaluacion = new SelectList(db.TipoEvaluacions.Where(x => x.bl_EstadoTipo == true), "Id_TipoEvaluacion", "strTipoEvaluacion");
            ViewBag.Id_Parciales = new SelectList(db.Parciales.Where(x => x.Estado_Parcial == true).Where(x => x.Dt_Inicio <= DateTime.Now && DateTime.Now <= x.Dt_Final), "Id_Parcial", "Nombre_Parcial");
            return View();
        }

        // POST: Evaluaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Evaluaciones evaluaciones)
        {
            if (ModelState.IsValid)
            {
                db.Evaluaciones.Add(evaluaciones);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { idCurso = evaluaciones.Id_Curso });
            }


            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia", evaluaciones.Id_Materia);
            ViewBag.Id_TipoEvaluacion = new SelectList(db.TipoEvaluacions, "Id_TipoEvaluacion", "strTipoEvaluacion", evaluaciones.Id_TipoEvaluacion);
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

            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia", evaluaciones.Id_Materia);
            ViewBag.Id_TipoEvaluacion = new SelectList(db.TipoEvaluacions, "Id_TipoEvaluacion", "strTipoEvaluacion", evaluaciones.Id_TipoEvaluacion);
            return View(evaluaciones);
        }

        // POST: Evaluaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Evaluacion,Id_Materia,Id_CursoA,Fecha_Comienzo,Fecha_Final,Descripcion,ValorTotal,BL_Aprobado,Num_Evaluacion,Id_TipoEvaluacion")] Evaluaciones evaluaciones)
        {
            if (ModelState.IsValid)
            {

                var antiguaEvaluacion = db.Evaluaciones.Find(evaluaciones.Id_Evaluacion);

                antiguaEvaluacion.Fecha_Comienzo = evaluaciones.Fecha_Comienzo;
                antiguaEvaluacion.Fecha_Final = evaluaciones.Fecha_Final;
                antiguaEvaluacion.Descripcion = evaluaciones.Descripcion;
                antiguaEvaluacion.Id_TipoEvaluacion = evaluaciones.Id_TipoEvaluacion;



                db.Entry(antiguaEvaluacion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = antiguaEvaluacion.Id_Evaluacion });
            }

            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id_Materia", "Codigo_Materia", evaluaciones.Id_Materia);
            ViewBag.Id_TipoEvaluacion = new SelectList(db.TipoEvaluacions, "Id_TipoEvaluacion", "strTipoEvaluacion", evaluaciones.Id_TipoEvaluacion);
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta()
            {
                Mensaje = "Ocurrio un error en el servidor"
            };

            Evaluaciones evaluaciones = await db.Evaluaciones.FindAsync(id);
            db.Evaluaciones.Remove(evaluaciones);

            respuesta.Ok = true;
            respuesta.Mensaje = "Se elimino correctamente";

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
        [HttpPost]
        public JsonResult Valor(int? id)
        {
            if (id!=null)
            {
                var puntos = db.TipoEvaluacions.Find(id).Valor;
                return Json(puntos);
            }

            return Json("Digite un valor");
        }

        public JsonResult CantidadClases(int idCurso)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var EvaluacionesTotales = db2.Evaluaciones.Where(x => x.Id_Curso == idCurso);
                var Materias = db.tbCursoAsignaturas.Include(x=>x.Materias).Where(x => x.Id_Curso == idCurso).ToList();
                List<TotalClases> respuesta= new List<TotalClases>();
           
                foreach (var item in Materias)
                {
                    var Aux = new TotalClases();
                    Aux.Materia = item.Materias.Nombre_Materia;
                    Aux.Total = EvaluacionesTotales.Where(X => X.Id_Materia == item.Id_Materia).Count();
                    respuesta.Add(Aux);
                }
                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Valorar(int id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());

            bool respues = Validador.PuedeEntrar(usuario.Id, "Valorar Evaluaciones");
            if (respues == true)
            {
                Evaluaciones evaluaciones = db.Evaluaciones.Where(x => x.Id_Evaluacion == id).First();
                return PartialView(evaluaciones);
            }
            else
            {
                ViewBag.PermisoDenegado = true;
                return PartialView();
            }

        }
        [HttpPost]
        public ActionResult Valorar(bool res,int id)
        {
            var respuesta = new Validador.BaseRespuesta()
            {
                Mensaje = "Ocurio un error en el servidor"
            };
            Evaluaciones evaluaciones = db.Evaluaciones.Where(x => x.Id_Evaluacion == id).First();
            if (res == true)
            {
                var Estudiantes = db.CursoEstudiantes.Where(x=>x.Id_Curso==evaluaciones.Id_Curso);
                foreach (var item in Estudiantes)
                {
                    EvaluacionesEstudiantes evaluacionesEstudiantes = new EvaluacionesEstudiantes() {
                        Id_Estudiante = item.Id_Estudiante,
                        Id_Evaluaciones = evaluaciones.Id_Evaluacion, Resultado = 0,EstadoEvaluado=false
                    };
                    db.EvaluacionesEstudiantes.Add(evaluacionesEstudiantes);
                }
                evaluaciones.BL_Aprobado = true;
                db.Entry(evaluaciones).State = EntityState.Modified;
            }
            else if (res == false)
            {
                db.Evaluaciones.Remove(evaluaciones);
            }
            else
            {
                
            }
            db.SaveChanges();
            respuesta.Ok = true;
            respuesta.Mensaje = "Evaluacion valorada exitosamente";
            return Json(respuesta);
        }

        public JsonResult Materias()
        {
            int id = Convert.ToInt32(TempData["idCurso"]);
            var materias = db.tbCursoAsignaturas.Include(x=>x.Materias).Where(x => x.Id_Curso== id).ToList();
            return Json(materias);
        }

        public class TotalClases{
            public string Materia { get; set; }
            public int Total { get; set; }
        }
    }
}
