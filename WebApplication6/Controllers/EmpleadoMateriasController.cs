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
    [Authorize]
    public class EmpleadoMateriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxId = 0;
        // GET: EmpleadoMaterias
        public async Task<ActionResult> Index(int IdEmpleado)
        {
            auxId = IdEmpleado;
            var tbEmpleadoMaterias = db.tbEmpleadoMaterias.Include(e => e.Empleado).Include(e => e.Materias).Where(x=>x.Id_Empleado==IdEmpleado);
            return PartialView(await tbEmpleadoMaterias.ToListAsync());
        }

        // GET: EmpleadoMaterias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoMaterias empleadoMaterias = await db.tbEmpleadoMaterias.FindAsync(id);
            if (empleadoMaterias == null)
            {
                return HttpNotFound();
            }
            return View(empleadoMaterias);
        }
        public async Task<ActionResult> DetailsModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoMaterias empleadoMaterias = await db.tbEmpleadoMaterias.FindAsync(id);
            if (empleadoMaterias == null)
            {
                return HttpNotFound();
            }
            return View(empleadoMaterias);
        }

        // GET: EmpleadoMaterias/Create
        public ActionResult Create()
        {
            SelectList listItems;
            List<EmpleadoMaterias> materias = db.tbEmpleadoMaterias.Where(x=>x.Id_Empleado==auxId).ToList();
            List<Materias> materiastotales = db.tbmaterias.ToList();
            List<Materias> materiastotales2 = db.tbmaterias.ToList();
            foreach (var item in materiastotales)
            {
                foreach (var item2 in materias)
                {
                    if (item.Id==item2.Id_Materia)
                    {
                        materiastotales2.Remove(item);
                    }
                }
            }
            listItems = new SelectList(materiastotales2, "Id", "Nombre_Materia");
            ViewBag.Id_Materia = listItems;
            return PartialView();
        }

        // POST: EmpleadoMaterias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_EmpleadoMateria,Id_Empleado,Id_Materia")] EmpleadoMaterias empleadoMaterias)
        {
            if (ModelState.IsValid)
            {
                empleadoMaterias.Id_Empleado = auxId;
                db.tbEmpleadoMaterias.Add(empleadoMaterias);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","Empleado",new { id=auxId});
            }

            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id", "Codigo_Materia", empleadoMaterias.Id_Materia);
            return View(empleadoMaterias);
        }

        // GET: EmpleadoMaterias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoMaterias empleadoMaterias = await db.tbEmpleadoMaterias.FindAsync(id);
            if (empleadoMaterias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id", "Nombre_Materia", empleadoMaterias.Id_Materia);
            return  PartialView(empleadoMaterias);
        }

        // POST: EmpleadoMaterias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_EmpleadoMateria,Id_Empleado,Id_Materia")] EmpleadoMaterias empleadoMaterias)
        {
            if (ModelState.IsValid)
            {
                empleadoMaterias.Id_Empleado = auxId;
                db.Entry(empleadoMaterias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Materia = new SelectList(db.tbmaterias, "Id", "Nombre_Materia", empleadoMaterias.Id_Materia);
            return View(empleadoMaterias);
        }

        // GET: EmpleadoMaterias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpleadoMaterias empleadoMaterias = await db.tbEmpleadoMaterias.Include(x=>x.Empleado).Include(x=>x.Materias).Where(x=>x.Id_EmpleadoMateria==id).FirstAsync();
            if (empleadoMaterias == null)
            {
                return HttpNotFound();
            }
            return PartialView(empleadoMaterias);
        }

        // POST: EmpleadoMaterias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmpleadoMaterias empleadoMaterias = await db.tbEmpleadoMaterias.FindAsync(id);
            db.tbEmpleadoMaterias.Remove(empleadoMaterias);
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
