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
    public class HistorialAñosCursadoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxid = 0;
        private static int estid = 0;

        // GET: HistorialAñosCursado
        public ActionResult Index(int id, int id_est)
        {
            auxid = id;
            estid = id_est;
            ViewBag.Id = id;
            ViewBag.nom = db.tbestudiantes.Find(id_est).NombreCompleto;
            var tbHistorialAniosCursado = db.tbHistorialAniosCursado.Include(h => h.AniosACursar).Include(h => h.HistorialAcademico).Include(h=>h.HistorialAcademico.Estudiantes).Where(x => x.Id_Escuela== id);
            return View(tbHistorialAniosCursado.ToList());
        }

        // GET: HistorialAñosCursado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAñosCursado historialAñosCursado = db.tbHistorialAniosCursado.Include(h => h.AniosACursar).Include(h => h.HistorialAcademico).First();
            if (historialAñosCursado == null)
            {
                return HttpNotFound();
            }
            return View(historialAñosCursado);
        }

        // GET: HistorialAñosCursado/Create
        public ActionResult Create()
        {
            ViewBag.Id = auxid;
            var tbHistorialAniosCursado = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == auxid).First();
            if (tbHistorialAniosCursado == null)
            {

            }
            else
            {
                ViewBag.nom = tbHistorialAniosCursado.Estudiantes.Nombre;
                ViewBag.Id_Anio = new SelectList(db.tbAniosACursar, "Id", "Str_Curso");

            }
            return View();
        }

        // POST: HistorialAñosCursado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Escuela,Id_Anio,FechFinaly")] HistorialAñosCursado historialAñosCursado)
        {
            ViewBag.Id = auxid;
            bool cursado = false;
            int i = 1;
            var años = db.tbHistorialAniosCursado.Include(x => x.HistorialAcademico).Where(x => x.Id_Escuela == auxid);
            HistorialAcademico años2 = db.tbHistorialAcademico.Where(x => x.Id == auxid).First();
            if (años2.Num_AnioCursado > 0)
            {
                if (años.Count() == 0)
                {
                    if (ModelState.IsValid)
                    {
                        var añosnulo = db.tbHistorialAniosCursado.Include(x => x.HistorialAcademico).Where(x => x.HistorialAcademico.Id_Estudiante == estid);
                        foreach (var item in añosnulo)
                        {
                            if (item.Id_Anio == historialAñosCursado.Id_Anio)
                            {
                                cursado = true;
                            }
                        }
                        if (cursado == false)
                        {
                            historialAñosCursado.Id_Escuela = Convert.ToInt32(auxid);
                            db.tbHistorialAniosCursado.Add(historialAñosCursado);
                            db.SaveChanges();
                            i = 0;
                            return RedirectToAction("Index", new { id = historialAñosCursado.Id_Escuela, id_est = estid });
                        }

                    }
                }
                else
                {

                    foreach (var item in años)
                    {

                        if (item.Id_Anio == historialAñosCursado.Id_Anio)
                        {
                            cursado = true;
                        }


                        if (i <= años2.Num_AnioCursado)
                        {
                            i++;
                        }

                    }
                }

                if (i <= años2.Num_AnioCursado)
                {
                    if (ModelState.IsValid)
                    {
                        if (cursado == false)
                        {
                            historialAñosCursado.Id_Escuela = Convert.ToInt32(auxid);
                            db.tbHistorialAniosCursado.Add(historialAñosCursado);
                            db.SaveChanges();
                            i = 0;
                            return RedirectToAction("Index", new { id = historialAñosCursado.Id_Escuela, id_est = estid });
                        }
                        else
                        {

                            ViewBag.Error = "Ya tiene registro de ese año";
                            cursado = false;
                        }

                    }
                }
                else
                {
                    ViewBag.Error = "Error en creacion";
                }

            }


            var tbHistorialAniosCursado = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == auxid).First();
            ViewBag.nom = tbHistorialAniosCursado.Estudiantes.Nombre;

            ViewBag.Id_Anio = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", historialAñosCursado.Id_Anio);
            return View(historialAñosCursado);
        }

        // GET: HistorialAñosCursado/Edit/5
        public ActionResult Edit(int? id)
        {
            var tbHistorialAniosCursado = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == auxid).First();
            ViewBag.nom = tbHistorialAniosCursado.Estudiantes.Nombre;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAñosCursado historialAñosCursado = db.tbHistorialAniosCursado.Include(x => x.AniosACursar).Include(x => x.HistorialAcademico).Where(x => x.Id == id).First();

            if (historialAñosCursado == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Anio = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", historialAñosCursado.Id_Anio);
            return View(historialAñosCursado);
        }

        // POST: HistorialAñosCursado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Escuela,Id_Anio,FechFinaly,HistorialAcademico;")] HistorialAñosCursado historialAñosCursado)
        {
            bool cursado = false;
            var tbHistorialAniosCursado = db.tbHistorialAcademico.Include(x => x.Estudiantes).Where(x => x.Id == auxid).First();
            ViewBag.nom = tbHistorialAniosCursado.Estudiantes.Nombre;
            if (ModelState.IsValid)
            {
                var añosnulo = db.tbHistorialAniosCursado.Include(x => x.HistorialAcademico).Where(x => x.HistorialAcademico.Id_Estudiante == estid);
                foreach (var item in añosnulo)
                {
                    if (item.Id_Anio == historialAñosCursado.Id_Anio)
                    {
                        cursado = true;
                    }
                }
                if (cursado == false)
                {
                    historialAñosCursado.Id_Escuela = Convert.ToInt32(auxid);
                    db.Entry(historialAñosCursado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = historialAñosCursado.Id_Escuela, id_est = estid });

                }
                else
                {

                    ViewBag.Error = "Ya curso este año";
                }


            }
            ViewBag.Id_Anio = new SelectList(db.tbAniosACursar, "Id", "Str_Curso", historialAñosCursado.Id_Anio);
            HistorialAñosCursado historial = db.tbHistorialAniosCursado.Include(x => x.AniosACursar).Include(x => x.HistorialAcademico).Where(x => x.Id == historialAñosCursado.Id).First();
            return View(historial);
        }

        // GET: HistorialAñosCursado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialAñosCursado historialAñosCursado = db.tbHistorialAniosCursado.Include(x => x.AniosACursar).Include(x => x.HistorialAcademico).Where(x => x.Id == id).First();
            if (historialAñosCursado == null)
            {
                return HttpNotFound();
            }
            return View(historialAñosCursado);
        }

        // POST: HistorialAñosCursado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialAñosCursado historialAñosCursado = db.tbHistorialAniosCursado.Find(id);
            db.tbHistorialAniosCursado.Remove(historialAñosCursado);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = historialAñosCursado.Id_Escuela, id_est = estid });
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
