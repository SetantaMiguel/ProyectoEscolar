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
    public class ProfesorGuiasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int AuxIdCurso;
        // GET: ProfesorGuias
        public async Task<ActionResult> Index(int IdCurso)
        {
            AuxIdCurso = IdCurso;
            var profesorGuias = db.ProfesorGuias.Include(p => p.CursoEscolar).Include(p => p.Empleado).Where(x => x.Id_Curso == IdCurso).Where(x => x.Estado_Profesor == true);
            return PartialView(await profesorGuias.ToListAsync());
        }

        // GET: ProfesorGuias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfesorGuia profesorGuia = await db.ProfesorGuias.FindAsync(id);
            if (profesorGuia == null)
            {
                return HttpNotFound();
            }
            return View(profesorGuia);
        }

        // GET: ProfesorGuias/Create
        public ActionResult Create()
        {
            //todos los guias de este curso
            List<ProfesorGuia> EmpleadoGuia = db.ProfesorGuias.ToList();
            List<Empleado> Empleados = db.TbEmpleado.Where(x=>x.BL_Estado_Empleado==true).Where(x=>x.BL_ProfesorGuia==true).ToList();
            List<Empleado> Empleados2 = db.TbEmpleado.Where(x => x.BL_Estado_Empleado == true).Where(x => x.BL_ProfesorGuia == true).ToList();

            foreach (var item in Empleados)
            {
                if (EmpleadoGuia != null)
                {
                    foreach (var item2 in EmpleadoGuia)
                    {
                        if (item.Id == item2.Id_Empleado)
                        {
                            Empleados2.Remove(item);
                        }
                    }
                }
            }

            ViewBag.Id_Empleado = new SelectList(Empleados2, "Id", "NCompleto  ");
            return View();
        }

        // POST: ProfesorGuias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_ProfesorGuia,Id_Empleado,Id_Curso,Estado_Profesor")] ProfesorGuia profesorGuia)
        {
            if (ModelState.IsValid)
            {
                profesorGuia.Estado_Profesor = true;
                profesorGuia.Id_Curso = AuxIdCurso;
                db.ProfesorGuias.Add(profesorGuia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { IdCurso = AuxIdCurso });
            }
            ViewBag.Id_Empleado = new SelectList(db.TbEmpleado, "Id", "Nombre", profesorGuia.Id_Empleado);
            return View(profesorGuia);
        }


        // GET: ProfesorGuias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfesorGuia profesorGuia = await db.ProfesorGuias.Include(x=>x.Empleado).Include(x=>x.CursoEscolar).Where(x=>x.Id_ProfesorGuia==id).FirstOrDefaultAsync();
            if (profesorGuia == null)
            {
                return HttpNotFound();
            }
            return View(profesorGuia);
        }

        // POST: ProfesorGuias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProfesorGuia profesorGuia = await db.ProfesorGuias.FindAsync(id);
            db.ProfesorGuias.Remove(profesorGuia);
            await db.SaveChangesAsync();
            return RedirectToAction("Index",new { IdCurso = AuxIdCurso });
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
