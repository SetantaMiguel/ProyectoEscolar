using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication6.Models;
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class EstudiantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Estudiantes

        public ActionResult Index(string nameUser, string Nombre_Estudiante, int pagina = 1)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Estudiantes"))
            {
                var cantidadRegistrosPorPagina = 4;

                var estudiantes = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante)
                  .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                .Take(cantidadRegistrosPorPagina).WhereIf(!string.IsNullOrEmpty(nameUser), x => x.Codigo_Estudiante == nameUser).WhereIf(!string.IsNullOrEmpty(Nombre_Estudiante), x => x.Nombre == Nombre_Estudiante)
                .ToList();

                var totalDeRegistros = db.tbestudiantes.WhereIf(!string.IsNullOrEmpty(nameUser), x => x.Codigo_Estudiante == nameUser).WhereIf(!string.IsNullOrEmpty(Nombre_Estudiante), x => x.Nombre == Nombre_Estudiante)
                    .Count();
                var modelo = new IndexViewModel2();
                modelo.Estudiantes = estudiantes;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                return View(modelo);
            }

            return RedirectToRoute("EjemploHome");
        }

        [HttpPost]
        public ViewResult FiltarCodigo(string nameUser)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var query = db.tbestudiantes.Where(x => x.Codigo_Estudiante.Contains(nameUser));

                return View(query.ToList());
            }

        }

        // GET: Estudiantes/Details/5
        public ActionResult Details(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Estudiantes"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Estudiantes estudiantes = db.tbestudiantes.Include(u => u.AniosACursar).Where(u => u.Id_Estudiante == id).First();
                if (estudiantes == null)
                {
                    return HttpNotFound();
                }
                return View(estudiantes);
            }

            return RedirectToRoute("EjemploHome");
        }
        // GET: Estudiantes/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Estudiantes"))
            {
                ViewBag.Id = new SelectList(db.tbAniosACursar, "Id", "Str_Curso");
                return View();
            }

            return RedirectToRoute("EjemploHome");
        }

        // POST: Estudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Estudiante,Nombre,Apellido,Direccion,Correo,FechaNacimiento,Domicilio,Telefono,Genero,Estado_Estudiante,Codigo_Estudiante,Codigo_MINED,Str_Nombre_Padre,Str_Nombre_Madre,Id")] Estudiantes estudiantes, HttpPostedFileBase ImageFile)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Estudiantes"))
            {
                estudiantes.Estado_Estudiante = true;

                using (var ms = new MemoryStream())
                {
                    ImageFile.InputStream.CopyTo(ms);
                    estudiantes.Image = ms.ToArray();
                }

                if (ModelState.IsValid)
                {
                    TempData["UserSave"] = "Guardado";
                    db.tbestudiantes.Add(estudiantes);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Id = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", estudiantes.Id);
                return View(estudiantes);
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: Estudiantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Estudiantes"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Estudiantes estudiantes = db.tbestudiantes.Include(x => x.AniosACursar).Where(x => x.Id_Estudiante == id).First();

                if (estudiantes == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Id = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", estudiantes.Id);
                return View(estudiantes);
            }


            return RedirectToRoute("EjemploHome");
        }

        // POST: Estudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Estudiante,Nombre,Apellido,Direccion,Correo,FechaNacimiento,Domicilio,Telefono,Genero,Estado_Estudiante,Codigo_Estudiante,Codigo_MINED,Str_Nombre_Padre,Str_Nombre_Madre,Id")] Estudiantes estudiantes, HttpPostedFileBase ImageFile)
        {
            Estudiantes _estu = new Estudiantes();
            if (ImageFile == null)
            {
                using (var ms = new MemoryStream())
                {
                    _estu = db.tbestudiantes.Find(estudiantes.Id_Estudiante);
                    estudiantes.Image = _estu.Image;
                }
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    WebImage image = new WebImage(ImageFile.InputStream);
                    estudiantes.Image = image.GetBytes();
                }
            }

            if (ModelState.IsValid)
            {
                estudiantes.Estado_Estudiante = true;

                db.Entry(_estu).State = EntityState.Detached;
                db.Entry(estudiantes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new {id=estudiantes.Id_Estudiante });
            }
            ViewBag.Id = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", estudiantes.Id);
            return View(estudiantes);
        }

        // GET: Estudiantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Estudiantes"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Estudiantes estudiantes = db.tbestudiantes.Include(x => x.AniosACursar).Where(x => x.Id_Estudiante == id).First();
                if (estudiantes == null)
                {
                    return HttpNotFound();
                }
                return View(estudiantes);
            }


            return RedirectToRoute("EjemploHome");
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudiantes estudiantes = db.tbestudiantes.Find(id);
            db.tbestudiantes.Remove(estudiantes);
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


        public ActionResult CargarCurso(int id)
        {
            ViewBag.Id_Año = new SelectList(db.tbCursoEscolar, "Id_Curso", "NombredeCurso");
            return View();
        }

        public ActionResult getImage(int id)
        {

            Estudiantes est = db.tbestudiantes.Find(id);
            byte[] byteImage = est.Image;
            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);
            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;
            return File(memoryStream, "image/jpg");
        }

        public JsonResult BuscarPersona(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.tbestudiantes.Where(x => x.Codigo_Estudiante.Contains(term))
                    .Select(x => x.Codigo_Estudiante).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult BuscarPersonaPorNombre(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.tbestudiantes.Where(x => x.Nombre.Contains(term))
                    .Select(x => x.Nombre).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

    }

}
