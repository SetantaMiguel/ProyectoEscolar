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
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Ver Cursos");
            if (respues == true)
            {
                if (TempData["ErrorCreation"] != null)
                {
                    ViewBag.ErrorCreation = true;
                }
                var cantidadRegistrosPorPagina = 8;
                var cursoEscolars = db.tbCursoEscolar.Include(x => x.Secciones).Include(x => x.AniosACursar).Include(x => x.Modalidades).OrderBy(x => x.Id_Año)
                          .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                          .Take(cantidadRegistrosPorPagina).ToList();

                var totalDeRegistros = db.tbAniosACursar.Count();
                var modelo = new IndexViewModel2();
                modelo.CursoEscolars = cursoEscolars;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                return View(modelo);
            }
            return RedirectToRoute("EjemploHome");
        }
        public ActionResult IndexCursoAño(int IdAño)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Ver Cursos");
            if (respues == true)
            { 
                var tbCursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(c => c.Secciones).Include(x => x.Modalidades).Where(x=>x.Id_Año==IdAño);
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
            ViewBag.NoPermiso=true;
            return PartialView();
        }
        // GET: CursoEscolars/Details/5
        public ActionResult Details(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Ver Cursos");
            if (respues == true)
            { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CursoEscolar cursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(x => x.Modalidades).Include(c => c.Secciones).Where(x=>x.Id_Curso==id).First();
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
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Crear Curso");
            if (respues == true)
            { 
                var aulas = db.Secciones;
                ViewBag.Id_Año = new SelectList(db.tbAniosACursar, "Id", "Str_Curso");
                ViewBag.Id_Modalidad = new SelectList(db.Modalidades, "Id_Modalidad", "Str_Modalidad");
                ViewBag.Id_Seccion = new SelectList(aulas, "Id_Seccion", "Str_Seccion");
                return PartialView();
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
            var aux = db.tbCursoEscolar.Where(x => x.Id_Modalidad == cursoEscolar.Id_Modalidad).Where(x=>x.Id_Seccion==cursoEscolar.Id_Seccion);
            if (aux.Count()!=0)
            {
                resultado.Ok = false;
                resultado.Mensaje = "Ya hay un aula en esa modalidad";
                return Json(resultado);
            }
            if (ModelState.IsValid&&aux.Count()==0)
            {

                string modalidadname = db.Modalidades.Find(cursoEscolar.Id_Modalidad).Str_Modalidad;
                var aux2 = db.tbCursoEscolar.Where(x => x.Id_Modalidad == cursoEscolar.Id_Modalidad).Where(x => x.Id_Año == cursoEscolar.Id_Año);

                string año = db.tbAniosACursar.Find(cursoEscolar.Id_Año).Str_Curso;
                string[] array = { "A", "B", "C", "D", "E","F" };
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
                cursoEscolar.NombredeCurso = año + " " + letras +" "+modalidadname +" "+ DateTime.Now.Year.ToString();
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
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Crear Curso");
            if (respues == true)
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CursoEscolar cursoEscolar = db.tbCursoEscolar.Include(c => c.AniosACursar).Include(x => x.Modalidades).Include(c => c.Secciones).Where(x => x.Id_Curso == id).First();
            db.Secciones.Find(cursoEscolar.Id_Seccion).AulaLLena = true;
            db.tbCursoEscolar.Find(id).Bl_Estado=false;
            db.tbCursoEscolar.Remove(cursoEscolar);
            db.SaveChanges();
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
