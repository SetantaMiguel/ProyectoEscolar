using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{

    public class EmpleadoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Empleado
        public ActionResult Index(string nameUser, int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;

            using (var db2 = new ApplicationDbContext())
            {
                // Func<Estudiantes, bool> predicado = x => !nameUser.HasValue || Name.Value == x.Id;

                var empleado = db.TbEmpleado.Include(e => e.User).OrderBy(x => x.Id)
                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                 .Take(cantidadRegistrosPorPagina).ToList();

                var totalDeRegistros = db.TbEmpleado.Count();
                var modelo = new IndexViewModel2();
                modelo.Empleados = empleado;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                return View(modelo);


            }

        }
        [HttpPost]
        public ActionResult Index(string nameUser)
        {

            if (nameUser == "")
            {
                var tbusuarios = db.TbEmpleado;
                return View(tbusuarios.ToList());
            }
            else
            {
                var tbusuario = db.TbEmpleado.Where(u => u.Codigo_Empleado == nameUser);
                if (tbusuario.Count() == 0)
                {
                    ViewBag.UserVacio = "Usuario no encontrado";
                    var tbusuarios = db.TbEmpleado;
                    return View(tbusuarios.ToList());

                }
                ViewBag.UserVacio = "Usuario Encontrado";
                return View(tbusuario.ToList());
            }

        }

        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.TbEmpleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }
        public async Task<ActionResult> DetailsModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = await db.TbEmpleado.FindAsync(id);
            if (empleado== null)
            {
                return HttpNotFound();
            }
            return PartialView(empleado);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Direccion,Correo,FechaNaci,Domicilio,Telefono,Genero,Identificacion,Codigo_Empleado,EstadoCivil,Dt_Contratacion")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.TbEmpleado.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.TbEmpleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Direccion,Correo,FechaNaci,Domicilio,Telefono,Genero,Identificacion,Codigo_Empleado,EstadoCivil,Dt_Contratacion")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.TbEmpleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.TbEmpleado.Find(id);
            db.TbEmpleado.Remove(empleado);
            db.SaveChanges();
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
        public JsonResult BuscarPersona(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.TbEmpleado.Where(x => x.Codigo_Empleado.Contains(term))
                    .Select(x => x.Codigo_Empleado).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
