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
            var cantidadRegistrosPorPagina = 4;

            using (var db2 = new ApplicationDbContext())
            {
                if (TempData["UserSave"]!=null)
                {
                    ViewBag.UserSave = TempData["UserSave"];
                }
                if (nameUser == null && Nombre_Estudiante == null || nameUser == "" && Nombre_Estudiante == "")
                {
                    var estudiantes = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante)
                        .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                        .Take(cantidadRegistrosPorPagina).ToList();

                    var totalDeRegistros = db.tbestudiantes.Count();
                    var modelo = new IndexViewModel2();
                    modelo.Estudiantes = estudiantes;
                    modelo.PaginaActual = pagina;
                    modelo.TotalRegistros = totalDeRegistros;
                    modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                    return View(modelo);


                }
                else if(nameUser!="")
                {
                    if (Nombre_Estudiante=="")
                    {
                        var estudiantes = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante).Where(x => x.Codigo_Estudiante.Contains(nameUser))
                         .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                         .Take(cantidadRegistrosPorPagina).ToList();
                        if (estudiantes.Count == 1)
                        {
                            var totalDeRegistros = db.tbestudiantes.Where(x => x.Codigo_Estudiante.Contains(nameUser)).Count();

                            var modelo = new IndexViewModel2();
                            modelo.Estudiantes = estudiantes;
                            modelo.PaginaActual = pagina;
                            modelo.TotalRegistros = totalDeRegistros;
                            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                            return View(modelo);
                        }
                        else
                        {
                            var estudiantes2 = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante)
                            .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                            .Take(cantidadRegistrosPorPagina).ToList();

                            var totalDeRegistros = db.tbestudiantes.Count();
                            var modelo = new IndexViewModel2();
                            modelo.Estudiantes = estudiantes2;
                            modelo.PaginaActual = pagina;
                            modelo.TotalRegistros = totalDeRegistros;
                            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                            ViewBag.UserVacio = "Usuario no Encontrado";
                            return View(modelo);
                        }
                    }
                    else
                    {
                        var estudiantes = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante).Where(x => x.Codigo_Estudiante.Contains(nameUser)).Where(x=>x.Nombre.Contains(Nombre_Estudiante))
                        .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                        .Take(cantidadRegistrosPorPagina).ToList();
                        if (estudiantes.Count == 1)
                        {
                            var totalDeRegistros = db.tbestudiantes.Where(x => x.Codigo_Estudiante == nameUser).Where(x=>x.Nombre==Nombre_Estudiante).Count();

                            var modelo = new IndexViewModel2();
                            modelo.Estudiantes = estudiantes;
                            modelo.PaginaActual = pagina;
                            modelo.TotalRegistros = totalDeRegistros;
                            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                            return View(modelo);
                        }
                        else
                        {
                            var estudiantes2 = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante)
                            .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                            .Take(cantidadRegistrosPorPagina).ToList();

                            var totalDeRegistros = db.tbestudiantes.Count();
                            var modelo = new IndexViewModel2();
                            modelo.Estudiantes = estudiantes2;
                            modelo.PaginaActual = pagina;
                            modelo.TotalRegistros = totalDeRegistros;
                            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                            ViewBag.UserVacio = "Usuario no Encontrado";
                            return View(modelo);
                        }
                    }


                }
                else if (Nombre_Estudiante!="")
                {
                    var estudiantes = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante).Where(x => x.Nombre.Contains(Nombre_Estudiante))
                         .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                         .Take(cantidadRegistrosPorPagina).ToList();
                    if (estudiantes.Count == 1)
                    {
                        var totalDeRegistros = db.tbestudiantes.Where(x => x.Nombre.Contains(Nombre_Estudiante)).Count();

                        var modelo = new IndexViewModel2();
                        modelo.Estudiantes = estudiantes;
                        modelo.PaginaActual = pagina;
                        modelo.TotalRegistros = totalDeRegistros;
                        modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                        return View(modelo);
                    }
                    else
                    {
                        var estudiantes2 = db.tbestudiantes.Include(e => e.AniosACursar).OrderBy(x => x.Id_Estudiante)
                        .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                        .Take(cantidadRegistrosPorPagina).ToList();

                        var totalDeRegistros = db.tbestudiantes.Count();
                        var modelo = new IndexViewModel2();
                        modelo.Estudiantes = estudiantes2;
                        modelo.PaginaActual = pagina;
                        modelo.TotalRegistros = totalDeRegistros;
                        modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                         ViewBag.UserVacio = "Usuario no Encontrado";
                        return View(modelo);
                    }
                }
            }
            return View();
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
        // GET: Estudiantes/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.tbAniosACursar, "Id", "Str_Curso");
            return View();
        }

        // POST: Estudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Estudiante,Nombre,Apellido,Direccion,Correo,FechaNacimiento,Domicilio,Telefono,Genero,Estado_Estudiante,Codigo_Estudiante,Codigo_MINED,Str_Nombre_Padre,Str_Nombre_Madre,Id")] Estudiantes estudiantes, HttpPostedFileBase ImageFile)
        {
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

        // GET: Estudiantes/Edit/5
        public ActionResult Edit(int? id)
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
                db.Entry(_estu).State = EntityState.Detached;
                db.Entry(estudiantes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", estudiantes.Id);
            return View(estudiantes);
        }

        // GET: Estudiantes/Delete/5
        public ActionResult Delete(int? id)
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
