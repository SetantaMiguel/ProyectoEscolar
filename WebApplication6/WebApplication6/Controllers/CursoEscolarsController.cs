using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class CursoEscolarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CursoEscolars

        public ActionResult Index(int pagina = 1)
        {

            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Cursos"))
            {
                if (TempData["ErrorCreation"] != null)
                {
                    ViewBag.ErrorCreation = true;
                }
                var cantidadRegistrosPorPagina = 8;
                var cursoEscolars = db.tbCursoEscolar.Include(x => x.Secciones).Include(x => x.AniosACursar).Include(x => x.Modalidades)
                    .Where(x=>x.Bl_Estado==true)
                    .OrderBy(x => x.Id_Año)
                          .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                          .Take(cantidadRegistrosPorPagina).ToList();

                var totalDeRegistros = db.tbCursoEscolar.Where(x => x.Bl_Estado == true).Count();
                var modelo = new IndexViewModel2
                {
                    CursoEscolars = cursoEscolars,
                    PaginaActual = pagina,
                    TotalRegistros = totalDeRegistros,
                    RegistrosPorPagina = cantidadRegistrosPorPagina
                };
                return View(modelo);
            }
            return RedirectToRoute("EjemploHome");
        }
        public ActionResult IndexCursoAño(int IdAño)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Cursos"))
            {
                var tbCursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(c => c.Secciones).Include(x => x.Modalidades).Where(x => x.Id_Año == IdAño).Where(x => x.Bl_Estado == true);
                try
                {
                    ViewBag.Anio = tbCursoEscolar.First().AniosACursar.Str_Curso;
                }
                catch (Exception)
                {
                    ViewBag.Anio = db.tbAniosACursar.Find(IdAño).Str_Curso;
                    return PartialView(tbCursoEscolar);

                }
                return PartialView(tbCursoEscolar.ToList());
            }
            ViewBag.NoPermiso = true;
            return PartialView();
        }
        // GET: CursoEscolars/Details/5
        public ActionResult Details(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Cursos"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.VerNotas = Validador.PuedeEntrar2(Session["Permisos"], "Ver Notas");
                ViewBag.AdministrarCursos = Validador.PuedeEntrar2(Session["Permisos"], "Administrar Cursos");
                CursoEscolar cursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(x => x.Modalidades).Include(c => c.Secciones).Where(x => x.Id_Curso == id).First();
                if (cursoEscolar == null)
                {
                    return HttpNotFound();
                }
                return View(cursoEscolar);
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: CursoEscolars/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Cursos"))
            {
                var existenSemestres = db.SemestresEscolares.Where(x=>x.EstadoSemestre==true);
                if (existenSemestres.Count()>=2)
                {
                    var aulas = db.Secciones;
                    ViewBag.Id_Año = new SelectList(db.tbAniosACursar, "Id", "Str_Curso");
                    ViewBag.Id_Modalidad = new SelectList(db.Modalidades, "Id_Modalidad", "Str_Modalidad");
                    ViewBag.Id_Seccion = new SelectList(aulas, "Id_Seccion", "Str_Seccion");
                    return PartialView();
                }
                else
                {
                    ViewBag.ExistenSemestres = false;
                    return PartialView();
                }
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: CursoEscolars/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id_Curso,Id_Modalidad,Bl_Estado,Id_Año,Id_Seccion")] CursoEscolar cursoEscolar)
        {
            var resultado = new Validador.BaseRespuesta();
            var aux = db.tbCursoEscolar.Where(x => x.Id_Modalidad == cursoEscolar.Id_Modalidad).Where(x => x.Id_Seccion == cursoEscolar.Id_Seccion)
                .Where(x=>x.Bl_Estado==true);
            if (aux.Count() != 0)
            {
                resultado.Ok = false;
                resultado.Mensaje = "Ya hay un aula en esa modalidad";
                return Json(resultado);
            }
            if (ModelState.IsValid && aux.Count() == 0)
            {

                string modalidadname = db.Modalidades.Find(cursoEscolar.Id_Modalidad).Str_Modalidad;
                var aux2 = db.tbCursoEscolar.Where(x => x.Id_Modalidad == cursoEscolar.Id_Modalidad).Where(x => x.Id_Año == cursoEscolar.Id_Año)
                    .Where(x => x.Bl_Estado == true);

                string año = db.tbAniosACursar.Find(cursoEscolar.Id_Año).Str_Curso;
                string[] array = { "A", "B", "C", "D", "E", "F" };
                var curso = db.tbCursoEscolar.Where(x => x.Id_Año == cursoEscolar.Id_Año).ToList();
                string letras = "A";
                var modalidades = db.Modalidades.ToList();
                switch (aux2.Count() + 1)
                {
                    case 1:
                        letras = "A";
                        break;
                    case 2:
                        letras = "B";
                        break;
                    case 3:
                        letras = "C";
                        break;
                    case 4:
                        letras = "D";
                        break;
                    case 5:
                        letras = "E";
                        break;
                    case 6:
                        letras = "F";
                        break;
                    default:

                        break;
                }
                cursoEscolar.NombredeCurso = año + " " + letras + " " + modalidadname + " " + DateTime.Now.Year.ToString();
                cursoEscolar.Bl_Estado = true;
                db.tbCursoEscolar.Add(cursoEscolar);
                db.Secciones.Find(cursoEscolar.Id_Seccion).AulaLLena = false;
                db.SaveChanges();

                resultado.Ok = true;
                resultado.Mensaje = "Se creo correctamente el curso";
                return Json(resultado);
            }
            return PartialView(cursoEscolar);
        }

        // GET: CursoEscolars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Cursos"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CursoEscolar cursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(c => c.Secciones).Where(x => x.Id_Curso == id).First();
                if (cursoEscolar == null)
                {
                    return HttpNotFound();
                }
                return PartialView(cursoEscolar);

            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: CursoEscolars/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta();
            CursoEscolar cursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(x => x.Modalidades).Include(c => c.Secciones).Where(x => x.Id_Curso == id).First();
            var seccion = db.Secciones.Find(cursoEscolar.Id_Seccion);
            seccion.AulaLLena = false;
            seccion.EstudiantesAula = 0;
            db.Entry(seccion).State = EntityState.Modified;
            //Ahora liberar a los estudiantes
            var estudiantesCurso = db.CursoEstudiantes.Where(x => x.Id_Curso == id);
            foreach (var item in estudiantesCurso)
            {
                db.CursoEstudiantes.Remove(item);
            }
            db.tbCursoEscolar.Find(id).Bl_Estado = false;
            db.Entry(cursoEscolar).State = EntityState.Modified;
           var profesorGuia = db.ProfesorGuias.Where(x => x.Id_Curso == cursoEscolar.Id_Curso).FirstOrDefault();
            if (profesorGuia!=null)
            {
             db.ProfesorGuias.Remove(profesorGuia);

            }
            db.SaveChanges();
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
    }
}
