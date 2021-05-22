using Microsoft.AspNet.Identity;
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
    [Authorize]
    public class AniosACursarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AniosACursars

        public ActionResult Index(string TxtAnio, int pagina = 1)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues= Validador.PuedeEntrar(usuario.Id, "Ver Años");
            if (respues==true)
            {
                var cantidadRegistrosPorPagina = 10;
                if (TxtAnio != null)
                {
                    var añosACursar = db.tbAniosACursar.OrderBy(x => x.Id)
                        .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                        .Take(cantidadRegistrosPorPagina).Where(x => x.Str_Curso.Contains(TxtAnio)).Where(x=>x.Estado==true).ToList();

                    var totalDeRegistros = db.tbAniosACursar.Where(x => x.Str_Curso.Contains(TxtAnio)).Where(x => x.Estado == true).Count();
                    var modelo = new IndexViewModel2();
                    modelo.AniosACursars = añosACursar;
                    modelo.PaginaActual = pagina;
                    modelo.TotalRegistros = totalDeRegistros;
                    modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                    return View(modelo);
                }
                var añosACursar2 = db.tbAniosACursar.Where(x => x.Estado == true).OrderBy(x => x.Id)
                            .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                            .Take(cantidadRegistrosPorPagina).ToList();

                var totalDeRegistros2 = db.tbAniosACursar.Where(x => x.Estado == true).Count();
                var modelo2 = new IndexViewModel2();
                modelo2.AniosACursars = añosACursar2;
                modelo2.PaginaActual = pagina;
                modelo2.TotalRegistros = totalDeRegistros2;
                modelo2.RegistrosPorPagina = cantidadRegistrosPorPagina;
                return View(modelo2);
            }
      
          return RedirectToRoute("EjemploHome");
        }

        // GET: AniosACursars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AniosACursar aniosACursar = db.tbAniosACursar.Find(id);
            if (aniosACursar == null)
            {
                return HttpNotFound();
            }
            return View(aniosACursar);
        }

        [Authorize]
        // GET: AniosACursars/Create
        public ActionResult Create()
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Años");
            if (respues == true)
            {
                return PartialView();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: AniosACursars/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Str_Curso")] AniosACursar aniosACursar)
        {
            var resultado = new Validador.BaseRespuesta();
                
            if (ModelState.IsValid)
            {
                aniosACursar.Estado = true;
                db.tbAniosACursar.Add(aniosACursar);
                db.SaveChanges();
                resultado.Ok = true;
                resultado.Mensaje = "Se agrego el año: " + aniosACursar.Str_Curso+" exitosamente";
                return Json(resultado);
            }

            return PartialView(aniosACursar);
        }

        // GET: AniosACursars/Edit/5
        public ActionResult Edit(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Años");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AniosACursar aniosACursar = db.tbAniosACursar.Find(id);
                if (aniosACursar == null)
                {
                    return HttpNotFound();
                }
                return PartialView(aniosACursar);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: AniosACursars/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Str_Curso")] AniosACursar aniosACursar)
        {
            var resultado = new Validador.BaseRespuesta();
            if (ModelState.IsValid)
            {
                aniosACursar.Estado = true;
                db.Entry(aniosACursar).State = EntityState.Modified;
                db.SaveChanges();
                resultado.Ok = true;
                return Json(resultado);
            }
            return View(aniosACursar);
        }
        [Authorize]
        // GET: AniosACursars/Delete/5
        public ActionResult Delete(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Años");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AniosACursar aniosACursar = db.tbAniosACursar.Find(id);
                if (aniosACursar == null)
                {
                    return HttpNotFound();
                }
                return PartialView(aniosACursar);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: AniosACursars/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var resultado = new Validador.BaseRespuesta();
            AniosACursar aniosACursar = db.tbAniosACursar.Find(id);
            aniosACursar.Estado = false;
            db.Entry(aniosACursar).State = EntityState.Modified;
            await db.SaveChangesAsync();
            resultado.Ok = true;
            return Json(resultado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult BuscarAnio(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.tbAniosACursar.Where(x => x.Str_Curso.Contains(term))
                    .Select(x => x.Str_Curso).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
