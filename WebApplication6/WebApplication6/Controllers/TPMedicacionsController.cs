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
    [Authorize]
    public class TPMedicacionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TPMedicacions
        public ActionResult Index()
        {
            return View(db.tbTPmedicacion.ToList());
        }

        // GET: TPMedicacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPMedicacion tPMedicacion = db.tbTPmedicacion.Find(id);
            if (tPMedicacion == null)
            {
                return HttpNotFound();
            }
            return View(tPMedicacion);
        }

        // GET: TPMedicacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TPMedicacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Str_Tipo")] TPMedicacion tPMedicacion)
        {
            if (ModelState.IsValid)
            {
                db.tbTPmedicacion.Add(tPMedicacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tPMedicacion);
        }

        // GET: TPMedicacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPMedicacion tPMedicacion = db.tbTPmedicacion.Find(id);
            if (tPMedicacion == null)
            {
                return HttpNotFound();
            }
            return View(tPMedicacion);
        }

        // POST: TPMedicacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Str_Tipo")] TPMedicacion tPMedicacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tPMedicacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tPMedicacion);
        }

        // GET: TPMedicacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TPMedicacion tPMedicacion = db.tbTPmedicacion.Find(id);
            if (tPMedicacion == null)
            {
                return HttpNotFound();
            }
            return View(tPMedicacion);
        }

        // POST: TPMedicacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TPMedicacion tPMedicacion = db.tbTPmedicacion.Find(id);
            db.tbTPmedicacion.Remove(tPMedicacion);
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
