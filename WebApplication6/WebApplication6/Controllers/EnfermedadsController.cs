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
    public class EnfermedadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Enfermedads
        public ActionResult Index()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Enfermeria"))
            {
                return View(db.tbEnfermedad.ToList());

            }
            return RedirectToRoute("EjemploHome");
        }
        // GET: Enfermedads/Details/5
        public ActionResult Details(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Enfermeria"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Enfermedad enfermedad = db.tbEnfermedad.Find(id);
                if (enfermedad == null)
                {
                    return HttpNotFound();
                }
                return View(enfermedad);
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: Enfermedads/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Enfermeria"))
            { 
                return View();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Enfermedads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Enfermedad,Str_NombreEnfermedad,Str_Descripcion")] Enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.tbEnfermedad.Add(enfermedad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(enfermedad);
        }

        // GET: Enfermedads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Enfermeria"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Enfermedad enfermedad = db.tbEnfermedad.Find(id);
                if (enfermedad == null)
                {
                    return HttpNotFound();
                }
                return View(enfermedad);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Enfermedads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Enfermedad,Str_NombreEnfermedad,Str_Descripcion")] Enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enfermedad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enfermedad);
        }

        // GET: Enfermedads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Enfermeria"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Enfermedad enfermedad = db.tbEnfermedad.Find(id);
                if (enfermedad == null)
                {
                    return HttpNotFound();
                }
                return View(enfermedad);
            }
            return RedirectToRoute("EjemploHome");

        }

        // POST: Enfermedads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enfermedad enfermedad = db.tbEnfermedad.Find(id);
            db.tbEnfermedad.Remove(enfermedad);
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
