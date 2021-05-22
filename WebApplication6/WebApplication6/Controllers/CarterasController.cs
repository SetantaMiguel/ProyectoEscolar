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
    public class CarterasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carteras
        public async Task<ActionResult> Index()
        {
            return View(await db.Carteras.ToListAsync());
        }

        // GET: Carteras/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartera cartera = await db.Carteras.FindAsync(id);
            if (cartera == null)
            {
                return HttpNotFound();
            }
            return View(cartera);
        }

        // GET: Carteras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carteras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdCartera,codigo,Nombre,Apellido")] Cartera cartera)
        {
            if (ModelState.IsValid)
            {
                db.Carteras.Add(cartera);
                await db.SaveChangesAsync();



                db.Entry(cartera).GetDatabaseValues();


                string cajeroCODE = cartera.codigo;

                var empleadoNombreA = db.TbEmpleado.Where(x => x.Codigo_Empleado == cajeroCODE).Select(d => d.Nombre).ToArray();
                var empleadoNombre = empleadoNombreA[0];
                var empleadoApellidoA = db.TbEmpleado.Where(x => x.Codigo_Empleado == cajeroCODE).Select(d => d.Apellido).ToArray();
                var empleadoApellido = empleadoApellidoA[0];
                var cajeroUPDATE = db.Carteras.First(x => x.codigo == cajeroCODE);
                cajeroUPDATE.Nombre = empleadoNombre;
                cajeroUPDATE.Apellido = empleadoApellido;
                db.SaveChanges();







                return RedirectToAction("Index");
            }

            return View(cartera);
        }

        // GET: Carteras/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartera cartera = await db.Carteras.FindAsync(id);
            if (cartera == null)
            {
                return HttpNotFound();
            }
            return View(cartera);
        }

        // POST: Carteras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdCartera,codigo,Nombre,Apellido")] Cartera cartera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartera).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cartera);
        }

        // GET: Carteras/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cartera cartera = await db.Carteras.FindAsync(id);
            if (cartera == null)
            {
                return HttpNotFound();
            }
            return View(cartera);
        }

        // POST: Carteras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cartera cartera = await db.Carteras.FindAsync(id);
            db.Carteras.Remove(cartera);
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
