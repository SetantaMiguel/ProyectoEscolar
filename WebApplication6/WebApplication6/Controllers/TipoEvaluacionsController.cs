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
    public class TipoEvaluacionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TipoEvaluacions
        public async Task<ActionResult> Index()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Evaluaciones"))
            {
                ViewBag.AdministrarEvaluaciones = Validador.PuedeEntrar2(Session["Permisos"], "Administrar Evaluaciones");
                return View(await db.TipoEvaluacions.Where(x=>x.bl_EstadoTipo==true).ToListAsync());
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: TipoEvaluacions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Evaluaciones"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TipoEvaluacion tipoEvaluacion = await db.TipoEvaluacions.FindAsync(id);
                if (tipoEvaluacion == null)
                {
                    return HttpNotFound();
                }
                return View(tipoEvaluacion);
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: TipoEvaluacions/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Evaluaciones"))
            {
                return PartialView();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: TipoEvaluacions/Create

        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: TipoEvaluacions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Evaluaciones"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TipoEvaluacion tipoEvaluacion = await db.TipoEvaluacions.FindAsync(id);
                if (tipoEvaluacion == null)
                {
                    return HttpNotFound();
                }
                return View(tipoEvaluacion);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: TipoEvaluacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id_TipoEvaluacion,strTipoEvaluacion,DescripcionTipEvaluacion,Valor")] TipoEvaluacion tipoEvaluacion)
        {
            var resultado = new BaseRespuesta();
            if (ModelState.IsValid)
            {
                tipoEvaluacion.bl_EstadoTipo = true;
                db.Entry(tipoEvaluacion).State = EntityState.Modified;
                 db.SaveChanges();
                resultado.Ok = true;
                return Json(resultado);
            }
            return PartialView(tipoEvaluacion);
        }

        // GET: TipoEvaluacions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Secciones"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TipoEvaluacion tipoEvaluacion = await db.TipoEvaluacions.FindAsync(id);
                if (tipoEvaluacion == null)
                {
                    return HttpNotFound();
                }
                return View(tipoEvaluacion);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: TipoEvaluacions/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var resultado = new BaseRespuesta();
            TipoEvaluacion tipoEvaluacion = await db.TipoEvaluacions.FindAsync(id);
            tipoEvaluacion.bl_EstadoTipo = false;
            db.Entry(tipoEvaluacion).State = EntityState.Modified;
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

        [HttpPost]
        public ActionResult Create(TipoEvaluacion tipoEvaluacion)
        {
            var resultado = new BaseRespuesta
            {
                Ok = false,
                Mensaje = "Fallo algo"
            };
            if (ModelState.IsValid)
            {
                tipoEvaluacion.bl_EstadoTipo = true;
                db.TipoEvaluacions.Add(tipoEvaluacion);
                db.SaveChanges();
                resultado.Mensaje = "El tipo de evaluacion se guardo correctamente";
                resultado.Ok = true;
                return Json(resultado);

            }
            return PartialView(tipoEvaluacion);
        }
        public class BaseRespuesta
        {
            public bool Ok { get; set ; }

            public string Mensaje { get; set; }
        }
    }
}
