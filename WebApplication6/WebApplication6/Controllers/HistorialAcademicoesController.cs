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
    public class HistorialAcademicoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxid = 0;

        // GET: HistorialAcademicoes
        public ActionResult Index(int id)
        {
            auxid = id;
            ViewBag.id = auxid;
            var tbHistorialAcademico = db.tbHistorialAcademico.Include(h => h.Estudiantes).Where(x => x.Id_Estudiante == id);
            return View(tbHistorialAcademico.ToList());
        }

        // GET: HistorialAcademicoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAcademico historialAcademico = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == id).First();
            if (historialAcademico == null)
            {
                return HttpNotFound();
            }
            return View(historialAcademico);
        }

        // GET: HistorialAcademicoes/Create
        public ActionResult Create()
        {
            ViewBag.id = auxid;
            return View();
        }

        // POST: HistorialAcademicoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cod_Escuela,Id_Estudiante,Str_NombreColegio,Num_AnioCursado")] HistorialAcademico historialAcademico)
        {
            if (ModelState.IsValid)
            {
                historialAcademico.Id_Estudiante = Convert.ToInt32(auxid);
                db.tbHistorialAcademico.Add(historialAcademico);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = historialAcademico.Id_Estudiante });
            }

            return View(historialAcademico);
        }

        // GET: HistorialAcademicoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAcademico historialAcademico = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == id).First();
            if (historialAcademico == null)
            {
                return HttpNotFound();
            }
            return View(historialAcademico);
        }

        // POST: HistorialAcademicoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cod_Escuela,Id_Estudiante,Str_NombreColegio,Num_AnioCursado")] HistorialAcademico historialAcademico)
        {
            if (ModelState.IsValid)
            {
                historialAcademico.Id_Estudiante = Convert.ToInt32(auxid);
                db.Entry(historialAcademico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = historialAcademico.Id_Estudiante });
            }
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", historialAcademico.Id_Estudiante);
            return View(historialAcademico);
        }

        // GET: HistorialAcademicoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAcademico historialAcademico = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == id).First();
            if (historialAcademico == null)
            {
                return HttpNotFound();
            }
            return View(historialAcademico);
        }

        // POST: HistorialAcademicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialAcademico historialAcademico = db.tbHistorialAcademico.Find(id);
            db.tbHistorialAcademico.Remove(historialAcademico);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = historialAcademico.Id_Estudiante });
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
