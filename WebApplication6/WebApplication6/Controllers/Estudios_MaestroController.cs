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

namespace WebApplication6.Controllers
{
    public class Estudios_MaestroController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int Aux = 0;
        // GET: Estudios_Maestro

        public ActionResult Index(int id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Empleados");
            if (respues == true)
            { 
                var tbEstudios_Maestro = db.tbEstudios_Maestro.Include(e => e.Empleado).Where(x => x.Id_Empleado == id);
                Aux = id;
                ViewBag.aux = Aux;
                return View(tbEstudios_Maestro.ToList());
            
            }
            return RedirectToRoute("EjemploHome");
        }
        // GET: Estudios_Maestro/Details/5
        public ActionResult Details(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Empleados");
            if (respues == true)
            { 
                    if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Estudios_Maestro estudios_Maestro = db.tbEstudios_Maestro.Find(id);
                if (estudios_Maestro == null)
                {
                    return HttpNotFound();
                }
                return View(estudios_Maestro);
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: Estudios_Maestro/Create
        public ActionResult Create()
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Empleados");
            if (respues == true)
            {
                ViewBag.Id_Empleado = Aux;
                return View();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Estudios_Maestro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Estudio,Str_Tipo_Estudio,Str_Nombre_Estudio,Str_Centro_Estudio,Bl_Estado_Estudio,Fch_Finalizacion,Id_Empleado")] Estudios_Maestro estudios_Maestro)
        {
            if (ModelState.IsValid)
            {
                estudios_Maestro.Id_Empleado = Aux;
                db.tbEstudios_Maestro.Add(estudios_Maestro);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Aux });
            }

            ViewBag.Id_Empleado = new SelectList(db.TbEmpleado, "Id", "Codigo_Empleado", estudios_Maestro.Id_Empleado);
            return View(estudios_Maestro);
        }

        // GET: Estudios_Maestro/Edit/5
        public ActionResult Edit(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Empleados");
            if (respues == true)
            { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Estudios_Maestro estudios_Maestro = db.tbEstudios_Maestro.Find(id);
                if (estudios_Maestro == null)
                {
                    return HttpNotFound();
                }
                return View(estudios_Maestro);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Estudios_Maestro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Estudio,Str_Tipo_Estudio,Str_Nombre_Estudio,Str_Centro_Estudio,Bl_Estado_Estudio,Fch_Finalizacion,Id_Empleado")] Estudios_Maestro estudios_Maestro)
        {
            if (ModelState.IsValid)
            {
                estudios_Maestro.Id_Empleado = Aux;
                db.Entry(estudios_Maestro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Aux });
            }
            return View(estudios_Maestro);
        }

        // GET: Estudios_Maestro/Delete/5
        public ActionResult Delete(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Empleados");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Estudios_Maestro estudios_Maestro = db.tbEstudios_Maestro.Find(id);
                if (estudios_Maestro == null)
                {
                    return HttpNotFound();
                }
                return View(estudios_Maestro);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Estudios_Maestro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudios_Maestro estudios_Maestro = db.tbEstudios_Maestro.Find(id);
            db.tbEstudios_Maestro.Remove(estudios_Maestro);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = Aux });
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
