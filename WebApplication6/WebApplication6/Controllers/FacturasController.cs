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
using Rotativa;

namespace WebApplication6.Controllers
{
    public class FacturasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Facturas
        public async Task<ActionResult> Index(int id)
        {

            var facturas = db.Facturas.Include(f => f.Caja).Include(f => f.Cajero).Include(f => f.Conceptos).Include(f => f.TipoPago).Include(f => f.Moneda).Where(x => x.Id == id); ;
            return View(await facturas.ToListAsync());
        }


        public async Task<ActionResult> ImprimirFactura(int id)
        {

            var facturas = db.Facturas.Include(f => f.Caja).Include(f => f.Cajero).Include(f => f.Conceptos).Include(f => f.TipoPago).Include(f => f.Moneda).Where(x => x.Id == id);
            return View(await facturas.ToListAsync());
        }




        public ActionResult PrintIndex()
        {
            string idString = Session["idFactura"].ToString();
            int id = int.Parse(idString);
            return new ActionAsPdf("ImprimirFactura", new { id = id }) { FileName = "Factura.pdf" };
        }



        // GET: Facturas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = await db.Facturas.FindAsync(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }












        // GET: Facturas/Create
        public ActionResult Create()
        {
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja");
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "codigo");
            return View();
        }

        // POST: Facturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Factura,FechaFactura,IdCajero,IdCaja,IdEstudiante,IdConcepto,Id_Tipo,Monto,IdMoneda,PagoAbonado,IdEquivalencia,Mora")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Facturas.Add(factura);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", factura.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "codigo", factura.IdCajero);
            return View(factura);
        }

        // GET: Facturas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = await db.Facturas.FindAsync(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", factura.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "codigo", factura.IdCajero);
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Factura,FechaFactura,IdCajero,IdCaja,IdEstudiante,IdConcepto,Id_Tipo,Monto,IdMoneda,PagoAbonado,IdEquivalencia,Mora")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", factura.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "codigo", factura.IdCajero);
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = await db.Facturas.FindAsync(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Factura factura = await db.Facturas.FindAsync(id);
            db.Facturas.Remove(factura);
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
