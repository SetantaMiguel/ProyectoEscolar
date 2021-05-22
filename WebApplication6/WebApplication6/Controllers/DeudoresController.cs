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
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;
using JsonResult = System.Web.Mvc.JsonResult;

namespace WebApplication6.Controllers
{

    public class DeudoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Deudores
        public async Task<ActionResult> Index(string searchString)
        {
            var deudores = db.Deudores.Include(x => x.Estado).Include(x => x.Estudiantes);
            var deudoresactivos = db.Deudores.Where(x => x.IdEstado.Equals(1)).Count();
            var deudoresNO_activos = db.Deudores.Where(x => x.IdEstado.Equals(2)).Count();
            ViewBag.Activos = deudoresactivos;
            ViewBag.NoActivos = deudoresNO_activos;
            //var estudiantes = from s in db.tbestudiantes select s;


            if (!String.IsNullOrEmpty(searchString))
            {

                //int searchString2 = Int32.Parse(searchString);
                deudores = deudores.Where(r => r.Estudiantes.Nombre.Contains(searchString)).Include(d => d.Estudiantes).Include(d => d.Estado);


            }

            
            



            return View(await deudores.ToListAsync());


        }



        // GET: Deudores/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deudores deudores = await db.Deudores.Include(d => d.Estudiantes).SingleOrDefaultAsync(i => i.Id_Deudor == id);
            if (deudores == null)
            {
                return HttpNotFound();
            }
            return View(deudores);
        }

        // GET: Deudores/Create
        public ActionResult Create()
        {
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "NombreCompleto");

            ViewBag.IdEstado = new SelectList(db.Estado, "Id_Estado", "Nombre");


            

            
            return View();

        }





        public JsonResult BuscarPersona(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.tbestudiantes.Where(x => x.Codigo_Estudiante.Contains(term))
                    .Select(x => x.Codigo_Estudiante).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult BuscarPersonaPorNombre(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.tbestudiantes.Where(x => x.Nombre.Contains(term))
                    .Select(x => x.Nombre).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }







        // POST: Deudores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([System.Web.Mvc.Bind(Include = "Id_Deudor,Id_Estudiante,Fecha_Ingreso,Fecha_Pagar,Total_Pagar,IdEstado,Detalles")] Deudores deudores)
        {
            if (ModelState.IsValid)
            {
                db.Deudores.Add(deudores);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", deudores.Id_Estudiante);
            ViewBag.IdEstado = new SelectList(db.Estado, "Id_Estado", "Nombre");
            return View(deudores);
        }

        // GET: Deudores/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deudores deudores = await db.Deudores.FindAsync(id);
            if (deudores == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", deudores.Id_Estudiante);
            return View(deudores);
        }

        // POST: Deudores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([System.Web.Mvc.Bind(Include = "Id_Deudor,Id_Estudiante,Fecha_Ingreso,Fecha_Pagar,Total_Pagar,IdEstado,Detalles")] Deudores deudores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deudores).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Estudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "Nombre", deudores.Id_Estudiante);
            return View(deudores);
        }

        // GET: Deudores/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deudores deudores = await db.Deudores.FindAsync(id);
            if (deudores == null)
            {
                return HttpNotFound();
            }
            return View(deudores);
        }

        // POST: Deudores/Delete/5
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Deudores deudores = await db.Deudores.FindAsync(id);
            db.Deudores.Remove(deudores);
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

        public async Task<ActionResult> Conceptos(int? id)
        {

            int? prueba = id;
            System.Diagnostics.Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
            System.Diagnostics.Debug.WriteLine(prueba);
            Session["idDeudor"] = prueba;
            return RedirectToAction("Create", "DetalleDeudors");
        }

    }


}
