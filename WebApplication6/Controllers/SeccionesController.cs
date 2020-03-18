using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class SeccionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Secciones
        public ActionResult Index()
        {
            return View(db.Secciones.Where(x=>x.Bl_Estado==true).ToList());
        }
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Secciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id_Seccion,Str_Seccion,Bl_Estado,Max_Estudiantes")] Secciones secciones)
        {
            var respuesta = new Validador.BaseRespuesta();
            var ExiteSeccion = db.Secciones.Where(x => x.Str_Seccion == secciones.Str_Seccion).Where(x => x.Bl_Estado == true);
            if (ExiteSeccion.Count() != 0)
            {
                respuesta.Mensaje = "Ya existe esa seccion, pruebe cambiar el nombre";
                return Json(respuesta);
            }
            if (ModelState.IsValid)
            {
                secciones.EstudiantesAula = 0;
                secciones.AulaLLena = true;
                db.Secciones.Add(secciones);
                db.SaveChanges();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se creo el aula " + secciones.Str_Seccion;
                return Json(respuesta);
            }
            respuesta.Mensaje = "Digito campos vacios";
            return Json(respuesta);
        }    

        // GET: Secciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secciones secciones = db.Secciones.Find(id);
            if (secciones == null)
            {
                return HttpNotFound();
            }
            return View(secciones);
        }

        // POST: Secciones/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta();
            Secciones secciones = db.Secciones.Find(id);
            secciones.Bl_Estado = false;
            db.Entry(secciones).State = EntityState.Modified;
            db.SaveChanges();
            respuesta.Ok = true;
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
