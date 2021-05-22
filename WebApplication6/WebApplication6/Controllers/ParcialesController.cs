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
    public class ParcialesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int AuxIdSemestre = 0;
        // GET: Parciales
        public async Task<ActionResult> Index(int IdSemestre)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Semestres"))
            {
                ViewBag.AdministrarSemestres = Validador.PuedeEntrar2(Session["Permisos"],"Administrar Semestres");
                var parciales = db.Parciales.Include(p => p.SemestresEscolares).Where(x=>x.Id_SemestreEscolar==IdSemestre)
                    .Where(x=>x.Estado_Parcial==true);
                if (IdSemestre!=null)
                {
                    AuxIdSemestre = IdSemestre;
                    ViewBag.NombreSemestre = db.SemestresEscolares.Find(IdSemestre).NombreSemestre;
                }
                return View(await parciales.ToListAsync());
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: Parciales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Semestres"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Parciales parciales = await db.Parciales.FindAsync(id);
                if (parciales == null)
                {
                    return HttpNotFound();
                }
                return View(parciales);
            }
            return RedirectToRoute("EjemploHome");

        }

        // GET: Parciales/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres"))
            {
                ViewBag.Id_SemestreEscolar = new SelectList(db.SemestresEscolares, "Id_Semestre", "NombreSemestre");
                ViewBag.InicoSemestre = db.SemestresEscolares.Find(AuxIdSemestre).FchInicio.ToString("dddd, dd MMMM yyyy");
                ViewBag.FinalSemestre = db.SemestresEscolares.Find(AuxIdSemestre).FchFinal.ToString("dddd, dd MMMM yyyy");
                return View();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Parciales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id_Parcial,Nombre_Parcial,Estado_Parcial,Id_SemestreEscolar,Dt_Inicio,Dt_Final")] Parciales parciales)
        {
            var respuesta = new Validador.BaseRespuesta { 
                Mensaje="Digito Valores vacios"
            };

            if (ModelState.IsValid)
            {
                //Saber si exite ya el nombre del parcial
                var ExisteParcial = db.Parciales.Where(x => x.Nombre_Parcial == parciales.Nombre_Parcial).Where(x => x.Estado_Parcial == true);
                if (ExisteParcial.Count() != 0)
                {
                    respuesta.Mensaje = "Ya existe ese nombre de parcial";
                    return Json(respuesta);
                }
                //Saber si las fechas estan en rango
                var RangoSemestreInicio = db.SemestresEscolares.Find(AuxIdSemestre).FchInicio;
                var RangoSemestreFinal = db.SemestresEscolares.Find(AuxIdSemestre).FchFinal;
                if (RangoSemestreInicio <= parciales.Dt_Inicio && parciales.Dt_Inicio <= RangoSemestreFinal)
                {
                    if ( RangoSemestreInicio <= parciales.Dt_Final && parciales.Dt_Final <= RangoSemestreFinal)
                    {

                    }
                    else
                    {
                        respuesta.Mensaje = "Rangos validos: " +
                            RangoSemestreInicio.ToString("dd MMMM yyyy") + " Hasta : " + RangoSemestreFinal.ToString("dd MMMM yyyy");
                        return Json(respuesta);
                    }
                }            
                else
                {
                    respuesta.Mensaje = "Rangos validos: " +
                        RangoSemestreInicio.ToString("dd MMMM yyyy") + " Hasta : " + RangoSemestreFinal.ToString("dd MMMM yyyy");
                    return Json(respuesta);
                }

                var MezclarFechas = db.Parciales.Where(x => x.Dt_Inicio == parciales.Dt_Inicio).Where(x=>x.Estado_Parcial==true);
                var MezclarFechas2 = db.Parciales.Where(x => x.Dt_Final== parciales.Dt_Inicio).Where(x => x.Estado_Parcial == true);   
                var MezclarFechasFinal = db.Parciales.Where(x => x.Dt_Inicio == parciales.Dt_Final).Where(x => x.Estado_Parcial == true);
                var MezclarFechasFinal2 = db.Parciales.Where(x => x.Dt_Final== parciales.Dt_Final).Where(x => x.Estado_Parcial == true);
                if (MezclarFechas.Count()!=0|| MezclarFechas2.Count() != 0 || MezclarFechasFinal.Count()!=0 || MezclarFechasFinal2.Count()!=0)
                {
                    respuesta.Mensaje = "Alguna de la fechas esta en margen de una activa";
                    return Json(respuesta);
                }

                parciales.Id_SemestreEscolar = AuxIdSemestre;
                parciales.Estado_Parcial = true;
                db.Parciales.Add(parciales);
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se agrego el parcial correctamente";
                return Json(respuesta);

            }
            return Json(respuesta);
        }

        // GET: Parciales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Parciales parciales = await db.Parciales.FindAsync(id);
                if (parciales == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Id_SemestreEscolar = new SelectList(db.SemestresEscolares, "Id_Semestre", "NombreSemestre", parciales.Id_SemestreEscolar);
                return View(parciales);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Parciales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Parcial,Nombre_Parcial,Estado_Parcial,Id_SemestreEscolar,Dt_Inicio,Dt_Final")] Parciales parciales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parciales).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_SemestreEscolar = new SelectList(db.SemestresEscolares, "Id_Semestre", "NombreSemestre", parciales.Id_SemestreEscolar);
            return View(parciales);
        }

        // GET: Parciales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Parciales parciales = await db.Parciales.FindAsync(id);
                if (parciales == null)
                {
                    return HttpNotFound();
                }
                return View(parciales);
            }
            return RedirectToRoute("EjemploHome");

        }

        // POST: Parciales/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta { 
             Mensaje="Ocurio un error en el servidor!"
            };
            Parciales parciales = await db.Parciales.FindAsync(id);
            db.Parciales.Remove(parciales);
            await db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.Mensaje = "Se desactivo correctamente";

            return Json(respuesta);
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
