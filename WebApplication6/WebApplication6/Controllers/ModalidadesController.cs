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
    public class ModalidadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modalidades
        public async Task<ActionResult> Index()
        {
            return View(await db.Modalidades.ToListAsync());
        }

        // GET: Modalidades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modalidades modalidades = await db.Modalidades.FindAsync(id);
            if (modalidades == null)
            {
                return HttpNotFound();
            }
            return View(modalidades);
        }

        // GET: Modalidades/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Modalidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id_Modalidad,Str_Modalidad")] Modalidades modalidades)
        {
            if (ModelState.IsValid)
            {
                db.Modalidades.Add(modalidades);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(modalidades);
        }

        // GET: Modalidades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modalidades modalidades = await db.Modalidades.FindAsync(id);
            if (modalidades == null)
            {
                return HttpNotFound();
            }
            return PartialView(modalidades);
        }

        // POST: Modalidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Modalidad,Str_Modalidad")] Modalidades modalidades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modalidades).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(modalidades);
        }

        // GET: Modalidades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Modalidades modalidades = await db.Modalidades.FindAsync(id);
            if (modalidades == null)
            {
                return HttpNotFound();
            }
            return PartialView(modalidades);
        }

        // POST: Modalidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Modalidades modalidades = await db.Modalidades.FindAsync(id);
            db.Modalidades.Remove(modalidades);
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
