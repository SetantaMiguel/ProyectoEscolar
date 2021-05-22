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
    public class EnfermedadEstudiantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxid = 0;
        // GET: EnfermedadEstudiantes
        public ActionResult Index(int id)
        {
            var enfermedadEstudiantes = db.EnfermedadEstudiantes.Include(e => e.Enfermedad).Include(e => e.Estudiantes).Include(e => e.TPMedicacion).Where(X => X.Id_Estudiante == id);
            if (enfermedadEstudiantes.Count() == 0)
            {
                auxid = id;
                ViewBag.id = auxid;
            }
            else
            {
                auxid = id;
                ViewBag.id = auxid;
                ViewBag.nom = enfermedadEstudiantes.First().Estudiantes.Nombre;
            }

            return View(enfermedadEstudiantes.ToList());
        }
        public ActionResult IndexEstudiante(int id)
        {
            var enfermedadEstudiantes = db.EnfermedadEstudiantes.Include(e => e.Enfermedad).Include(e => e.Estudiantes).Include(e => e.TPMedicacion);

            return View("Index", enfermedadEstudiantes.ToList());
        }
        // GET: EnfermedadEstudiantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnfermedadEstudiantes enfermedadEstudiantes = db.EnfermedadEstudiantes.Include(e => e.Enfermedad).Include(e => e.Estudiantes).Include(e => e.TPMedicacion).Where(x => x.Id == id).First();
            if (enfermedadEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(enfermedadEstudiantes);
        }

        // GET: EnfermedadEstudiantes/Create
        public ActionResult Create()
        {
            ViewBag.Id_Enfermedad = new SelectList(db.tbEnfermedad, "Id_Enfermedad", "Str_NombreEnfermedad");
            ViewBag.Id_Medicacion = new SelectList(db.tbTPmedicacion, "Id", "Str_Tipo");
            ViewBag.Id = auxid;
            return View();
        }

        // POST: EnfermedadEstudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Medicacion,Id_Estudiante,Id_Enfermedad,Id_Medicacion")] EnfermedadEstudiantes enfermedadEstudiantes)
        {
            if (ModelState.IsValid)
            {
                enfermedadEstudiantes.Id_Estudiante = Convert.ToInt32(auxid);
                if (enfermedadEstudiantes.Id_Estudiante > 0)
                {
                    db.EnfermedadEstudiantes.Add(enfermedadEstudiantes);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = enfermedadEstudiantes.Id_Estudiante });
                }
            }

            ViewBag.Id_Enfermedad = new SelectList(db.tbEnfermedad, "Id_Enfermedad", "Str_NombreEnfermedad", enfermedadEstudiantes.Id_Enfermedad);
            ViewBag.Id_Medicacion = new SelectList(db.tbTPmedicacion, "Id", "Str_Tipo", enfermedadEstudiantes.Id_Medicacion);
            return View(enfermedadEstudiantes);
        }

        // GET: EnfermedadEstudiantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EnfermedadEstudiantes enfermedadEstudiantes = db.EnfermedadEstudiantes.Find(id);
            if (enfermedadEstudiantes == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Enfermedad = new SelectList(db.tbEnfermedad, "Id_Enfermedad", "Str_NombreEnfermedad", enfermedadEstudiantes.Id_Enfermedad);
            ViewBag.Id_Medicacion = new SelectList(db.tbTPmedicacion, "Id", "Str_Tipo", enfermedadEstudiantes.Id_Medicacion);
            return View(enfermedadEstudiantes);
        }

        // POST: EnfermedadEstudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Medicacion,Id_Estudiante,Id_Enfermedad,Id_Medicacion")] EnfermedadEstudiantes enfermedadEstudiantes)
        {
            if (ModelState.IsValid)
            {
                enfermedadEstudiantes.Id_Estudiante = Convert.ToInt32(auxid);
                if (enfermedadEstudiantes.Id_Estudiante > 0)
                {
                    db.Entry(enfermedadEstudiantes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = enfermedadEstudiantes.Id_Estudiante });
                }
            }
            ViewBag.Id_Enfermedad = new SelectList(db.tbEnfermedad, "Id_Enfermedad", "Str_NombreEnfermedad", enfermedadEstudiantes.Id_Enfermedad);
            ViewBag.Id_Medicacion = new SelectList(db.tbTPmedicacion, "Id", "Str_Tipo", enfermedadEstudiantes.Id_Medicacion);
            return View(enfermedadEstudiantes);
        }

        // GET: EnfermedadEstudiantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnfermedadEstudiantes enfermedadEstudiantes = db.EnfermedadEstudiantes.Include(e => e.Enfermedad).Include(e => e.Estudiantes).Include(e => e.TPMedicacion).Where(X => X.Id == id).First();

            if (enfermedadEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(enfermedadEstudiantes);
        }

        // POST: EnfermedadEstudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnfermedadEstudiantes enfermedadEstudiantes = db.EnfermedadEstudiantes.Find(id);
            db.EnfermedadEstudiantes.Remove(enfermedadEstudiantes);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = enfermedadEstudiantes.Id_Estudiante });
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