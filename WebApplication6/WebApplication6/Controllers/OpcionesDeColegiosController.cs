using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class OpcionesDeColegiosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OpcionesDeColegios
        public async Task<ActionResult> Index()
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Colegio");
            if (respues == true)
            {
                return PartialView(await db.OpcionesDeColegios.ToListAsync());
            }
            return View();
        }

        // GET: OpcionesDeColegios/Details/5
        public ActionResult Details(int? id=1)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDeColegio opcionesDeColegio = db.OpcionesDeColegios.Find(id);
            if (opcionesDeColegio == null)
            {
                return HttpNotFound();
            }
            return PartialView(opcionesDeColegio);
        }

        // GET: OpcionesDeColegios/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: OpcionesDeColegios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MaximoEstudiantes,NombreColegio")] OpcionesDeColegio opcionesDeColegio)
        {
            if (ModelState.IsValid)
            {
                db.OpcionesDeColegios.Add(opcionesDeColegio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return PartialView(opcionesDeColegio);
        }

        // GET: OpcionesDeColegios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Colegio");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                OpcionesDeColegio opcionesDeColegio = await db.OpcionesDeColegios.FindAsync(id);
                if (opcionesDeColegio == null)
                {
                    return HttpNotFound();
                }
                return PartialView(opcionesDeColegio);
            }
            return View();

        }

        // POST: OpcionesDeColegios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NombreColegio")] OpcionesDeColegio opcionesDeColegio)
        {
            var respuesta = new Validador.BaseRespuesta() {
                Mensaje = "Digito Campos vacios"
            };
            if (ModelState.IsValid)
            {
                db.Entry(opcionesDeColegio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se cambio el nombre correctamente";
                return Json(respuesta);
            }
            return Json(respuesta);
        }

        // GET: OpcionesDeColegios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDeColegio opcionesDeColegio = await db.OpcionesDeColegios.FindAsync(id);
            if (opcionesDeColegio == null)
            {
                return HttpNotFound();
            }
            return PartialView(opcionesDeColegio);
        }

        // POST: OpcionesDeColegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OpcionesDeColegio opcionesDeColegio = await db.OpcionesDeColegios.FindAsync(id);
            db.OpcionesDeColegios.Remove(opcionesDeColegio);
            await db.SaveChangesAsync();
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
