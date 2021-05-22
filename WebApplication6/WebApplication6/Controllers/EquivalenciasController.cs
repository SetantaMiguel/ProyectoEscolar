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
    public class EquivalenciasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Equivalencias
        public async Task<ActionResult> Index()
        {
            return View(await db.Equivalencia.ToListAsync());
        }

        // GET: Equivalencias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equivalencia equivalencia = await db.Equivalencia.FindAsync(id);
            if (equivalencia == null)
            {
                return HttpNotFound();
            }
            return View(equivalencia);
        }

        // GET: Equivalencias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equivalencias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Equivalencia,Monto,Fecha")] Equivalencia equivalencia)
        {
            if (ModelState.IsValid)
            {
                db.Equivalencia.Add(equivalencia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equivalencia);
        }

        // GET: Equivalencias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equivalencia equivalencia = await db.Equivalencia.FindAsync(id);
            if (equivalencia == null)
            {
                return HttpNotFound();
            }
            return View(equivalencia);
        }

        // POST: Equivalencias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Equivalencia,Monto,Fecha")] Equivalencia equivalencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equivalencia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equivalencia);
        }

        // GET: Equivalencias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equivalencia equivalencia = await db.Equivalencia.FindAsync(id);
            if (equivalencia == null)
            {
                return HttpNotFound();
            }
            return View(equivalencia);
        }

        // POST: Equivalencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Equivalencia equivalencia = await db.Equivalencia.FindAsync(id);
            db.Equivalencia.Remove(equivalencia);
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
