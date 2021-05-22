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
                TempData["idCurso"] = idCurso;
                Session["IdCurso"] = IdCurso ;
                var tbCursoAsignaturas = db.tbCursoAsignaturas.Include(c => c.CursoEscolar).Include(c => c.Empleado).Include(c => c.Materias).Where(x => x.Id_Curso == IdCurso)
                    .Where(x=>x.Estado_Asignatura==true);
                ViewBag.NombreCurso = db.tbCursoEscolar.Find(IdCurso).NombredeCurso;
                ViewBag.Id_Curso = IdCurso;
                return PartialView(tbCursoAsignaturas);
            }
            else
            {
                var tbCursoAsignaturas = db.tbCursoAsignaturas.Include(c => c.CursoEscolar).Include(c => c.Empleado).Include(c => c.Materias).Where(x => x.Estado_Asignatura == true);
                return PartialView(await tbCursoAsignaturas.ToListAsync());
            }


        }

        // GET: Curso_Asignaturas/Create
        public ActionResult Create()
        {
            SelectList listItems;
            int id =Convert.ToInt32(TempData.Peek("idCurso")) ;
            List<Curso_Asignaturas> materiasdelcuro = db.tbCursoAsignaturas.Where(x => x.Id_Curso == id).Where(x=>x.Estado_Asignatura==true).ToList();
            var AnioEscolar = db.tbCursoEscolar.Find(idCurso).Id_Año;
            List<Materias> materiastotales = db.tbmaterias.Where(x=>x.Id_AñoEscolar==AnioEscolar).Where(x=>x.EstadoMateria==true).ToList();
            List<Materias> materiastotales2 = db.tbmaterias.Where(x => x.Id_AñoEscolar == AnioEscolar).Where(x => x.EstadoMateria == true).ToList();
            foreach (var item in materiastotales)
            {
                foreach (var item2 in materiasdelcuro)
                {
                    if (item.Id_Materia == item2.Id_Materia)
                    {
                        materiastotales2.Remove(item);
                    }
                }
            }
            listItems = new SelectList(materiastotales2, "Id_Materia", "Nombre_Materia");
            ViewBag.Id_Materia = listItems;
            return PartialView();

        }

        // POST: Curso_Asignaturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id_Curso_Asignatura,Id_Curso,Id_Materia,Id_Empleado")] Curso_Asignaturas curso_Asignaturas)
        {
            var respuesta = new Validador.BaseRespuesta
            {
                Mensaje = "Digito campos vacios"
            };

            if (ModelState.IsValid)
            {
                curso_Asignaturas.Estado_Asignatura = true;
                curso_Asignaturas.Id_Curso = idCurso;
                db.tbCursoAsignaturas.Add(curso_Asignaturas);
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se agrego correctamente la materia al curso";
                return Json(respuesta);
            }
            return Json(respuesta);
        }

        // GET: Curso_Asignaturas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso_Asignaturas curso_Asignaturas = await db.tbCursoAsignaturas.Include(x=>x.Materias).Include(x=>x.CursoEscolar)
                .Where(x=>x.Id_Curso_Asignatura==id).FirstAsync();
            if (curso_Asignaturas == null)
            {
                return HttpNotFound();
            }
            return View(curso_Asignaturas);
        }

        // POST: Curso_Asignaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta
            {
                Mensaje = "Ocurio un error"
            };

            Curso_Asignaturas curso_Asignaturas = await db.tbCursoAsignaturas.FindAsync(id);
            curso_Asignaturas.Estado_Asignatura = false;
            db.Entry(curso_Asignaturas).State = EntityState.Modified;

            await db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.Mensaje = "Se desactivo correctamente";
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
        public JsonResult EmpleadosHabiles(int IdMateria)
        {
            var empleados = db.tbEmpleadoMaterias.Include(x => x.Empleado).Where(x => x.Id_Materia == IdMateria)
                
                .ToList();
            return Json(empleados);
        }

    }
}
