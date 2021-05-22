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
    public class DetallePagoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DetallePagoes
        public async Task<ActionResult> Index()
        {
            return View(await db.DetallePago.ToListAsync());
        }

        // GET: DetallePagoes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePago detallePago = await db.DetallePago.FindAsync(id);
            if (detallePago == null)
            {
                return HttpNotFound();
            }
            return View(detallePago);
        }

        // GET: DetallePagoes/Create
        public ActionResult Create()
        {

            


            return View();
        }

        // POST: DetallePagoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdPago,IdConcepto,Id_Tipo,Monto,IdMoneda,PagoAbonado,IdEquivalencia,Balance,IdEstado,Moira")] DetallePago detallePago)
        {
            if (ModelState.IsValid)
            {



                db.DetallePago.Add(detallePago);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(detallePago);
        }

        // GET: DetallePagoes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePago detallePago = await db.DetallePago.FindAsync(id);
            if (detallePago == null)
            {
                return HttpNotFound();
            }
            return View(detallePago);
        }

        // POST: DetallePagoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdPago,IdConcepto,Id_Tipo,Monto,IdMoneda,PagoAbonado,IdEquivalencia,Balance,IdEstado,Moira")] DetallePago detallePago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(detallePago);
        }

        // GET: DetallePagoes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePago detallePago = await db.DetallePago.FindAsync(id);
            if (detallePago == null)
            {
                return HttpNotFound();
            }
            return View(detallePago);
        }

        // POST: DetallePagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DetallePago detallePago = await db.DetallePago.FindAsync(id);
            db.DetallePago.Remove(detallePago);
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
