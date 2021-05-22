using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using System.IO;

using System.Data.SqlClient;

namespace WebApplication6.Controllers
{
    public class ConceptosPagoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: ConceptosPagoes
        public async Task<ActionResult> Index()
        {
            if (DateTime.Now.Month==12&&DateTime.Now.Day==31)
            {
                await ActualizarConceptos();
            }
            
            return View(await db.ConceptosPago.Where(x=>x.Estado_Concepto==true).ToListAsync());
        }

        // GET: ConceptosPagoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConceptosPago conceptosPago = await db.ConceptosPago.FindAsync(id);
            if (conceptosPago == null)
            {
                return HttpNotFound();
            }
            return View(conceptosPago);
        }


        // GET: ConceptosPagoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConceptosPagoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Concepto,NombreConcepto,Monto,fechaMora")] ConceptosPago conceptosPago)
        {
            if (ModelState.IsValid)
            {
                conceptosPago.Estado_Concepto = true;
                db.ConceptosPago.Add(conceptosPago);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(conceptosPago);
        }

        // GET: ConceptosPagoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConceptosPago conceptosPago = await db.ConceptosPago.FindAsync(id);
            if (conceptosPago == null)
            {
                return HttpNotFound();
            }
            return View(conceptosPago);
        }

        // POST: ConceptosPagoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ConceptosPago conceptosPago)
        {
            if (ModelState.IsValid)
            {
                conceptosPago.Estado_Concepto = true;
                db.Entry(conceptosPago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(conceptosPago);
        }

        // GET: ConceptosPagoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConceptosPago conceptosPago = await db.ConceptosPago.FindAsync(id);
            if (conceptosPago == null)
            {
                return HttpNotFound();
            }
            return View(conceptosPago);
        }

        // POST: ConceptosPagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ConceptosPago conceptosPago = await db.ConceptosPago.FindAsync(id);
            conceptosPago.Estado_Concepto = false;
            db.Entry(conceptosPago).State = EntityState.Modified;
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

        public async Task<ActionResult> ActualizarConceptos()
        {
            var TodosConceptos = db.ConceptosPago.Where(x => x.Estado_Concepto == true);

            foreach (var item in TodosConceptos)
            {
                item.fechaMora.AddYears(1);
                db.Entry(item).State = EntityState.Modified;
            }
            await db.SaveChangesAsync();
            return View();
        }
    }
}
