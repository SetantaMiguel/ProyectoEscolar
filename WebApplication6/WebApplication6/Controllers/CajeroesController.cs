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

namespace WebApplication6.Controllers
{
    public class CajeroesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cajeroes
        public async Task<ActionResult> Index()
        {
            var cajero = db.Cajero.Include(x=>x.Empleado);
            return View(await cajero.ToListAsync());
        }

        // GET: Cajeroes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cajero cajero = await db.Cajero.FindAsync(id);
            if (cajero == null)
            {
                return HttpNotFound();
            }
            return View(cajero);
        }

        // GET: Cajeroes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cajeroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdCajero,NumCajero,codigo")] Cajero cajero)
        {
            if (cajero.codigo != null)
            {
                var idEmpleado = db.TbEmpleado.Where(x => x.Codigo_Empleado == cajero.codigo).FirstOrDefault().Id;
                cajero.Id_Empleado = idEmpleado;
                db.Cajero.Add(cajero);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(cajero);
        }

        // GET: Cajeroes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cajero cajero = await db.Cajero.FindAsync(id);
            if (cajero == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCajero = new SelectList(db.AperturaCaja, "Id", "Id", cajero.IdCajero);
            return View(cajero);
        }

        // POST: Cajeroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdCajero,NumCajero,Nombre,Apellido")] Cajero cajero)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cajero).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdCajero = new SelectList(db.AperturaCaja, "Id", "Id", cajero.IdCajero);
            return View(cajero);
        }

        // GET: Cajeroes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cajero cajero = await db.Cajero.FindAsync(id);
            if (cajero == null)
            {
                return HttpNotFound();
            }
            return View(cajero);
        }

        // POST: Cajeroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cajero cajero = await db.Cajero.FindAsync(id);
            db.Cajero.Remove(cajero);
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
