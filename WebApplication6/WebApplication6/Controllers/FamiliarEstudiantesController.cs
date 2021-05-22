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
    public class FamiliarEstudiantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int Aux = 0;

        // GET: FamiliarEstudiantes
        public ActionResult Index(int id)
        {
            var familiarEstudiantes = db.FamiliarEstudiantes.Include(x => x.Estudiantes).Where(x => x.Id_Estudiante == id).Where(x => x.BL_Estado_Familiar == true);
            if (familiarEstudiantes.Count()==0)
            {
                Aux = id;
                ViewBag.IdEs = id;

            }
            else {
                ViewBag.IdEs = id;
                ViewBag.nom = familiarEstudiantes.First().Estudiantes.Nombre;
                Aux = id;
            }

            return View(familiarEstudiantes.ToList());
        }

        // GET: FamiliarEstudiantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamiliarEstudiante familiarEstudiante = db.FamiliarEstudiantes.Include(x => x.Estudiantes).Where(x => x.Id == id).First();
            if (familiarEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(familiarEstudiante);
        }

        // GET: FamiliarEstudiantes/Create
        public ActionResult Create()
        {
            ViewBag.IdEs = Aux;
            return View();
        }

        // POST: FamiliarEstudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Direccion,Correo,FechaNaci,Domicilio,Telefono,Genero,Identificacion,EstadoCivil,BL_Estado_Familiar,Id_Estudiante,BL_Responsable")] FamiliarEstudiante familiarEstudiante, int? id)
        {
            if (ModelState.IsValid)
            {
                familiarEstudiante.Id_Estudiante = Convert.ToInt32(id);
                familiarEstudiante.BL_Estado_Familiar=true;
                var exist = db.FamiliarEstudiantes.Where(x => x.Id_Estudiante == id).Where(x => x.BL_Responsable == true).ToList();
                if (exist.Count == 0)
                {
                    db.FamiliarEstudiantes.Add(familiarEstudiante);
                    db.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = familiarEstudiante.Id_Estudiante });
                }
                else if (familiarEstudiante.BL_Responsable==false)
                {
                    db.FamiliarEstudiantes.Add(familiarEstudiante);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = familiarEstudiante.Id_Estudiante });
                }
                else
                {
                    ViewBag.ErrorMes = "Ya se encuentra un responsable activo";
                }
            }

            return View(familiarEstudiante);
        }

        // GET: FamiliarEstudiantes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamiliarEstudiante familiarEstudiante = db.FamiliarEstudiantes.Include(x => x.Estudiantes).Where(x => x.Id == id).First();
            if (familiarEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(familiarEstudiante);
        }

        // POST: FamiliarEstudiantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Direccion,Correo,FechaNaci,Domicilio,Telefono,Genero,Identificacion,EstadoCivil,BL_Estado_Familiar,Id_Estudiante,BL_Responsable")] FamiliarEstudiante familiarEstudiante)
        {

            if (ModelState.IsValid)
            {
                familiarEstudiante.Id_Estudiante = Convert.ToInt32(Aux);
                db.FamiliarEstudiantes.Add(familiarEstudiante);
                db.Entry(familiarEstudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = familiarEstudiante.Id_Estudiante });
            }
            return View(familiarEstudiante);

        }

        // GET: FamiliarEstudiantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FamiliarEstudiante familiarEstudiante = db.FamiliarEstudiantes.Include(x => x.Estudiantes).Where(x => x.Id == id).First();
            if (familiarEstudiante == null)
            {
                return HttpNotFound();
            }
            return View(familiarEstudiante);
        }

        // POST: FamiliarEstudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FamiliarEstudiante familiarEstudiante = db.FamiliarEstudiantes.Find(id);
            familiarEstudiante.BL_Estado_Familiar = false;
            familiarEstudiante.BL_Responsable= false;
            db.FamiliarEstudiantes.Add(familiarEstudiante);
            db.Entry(familiarEstudiante).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = familiarEstudiante.Id_Estudiante });
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
