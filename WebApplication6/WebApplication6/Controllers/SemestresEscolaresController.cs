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
    public class SemestresEscolaresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SemestresEscolares
        public async Task<ActionResult> Index()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Semestres"))
            {
                ViewBag.AdministrarSemestres = Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres");
                return View(await db.SemestresEscolares.Where(X=>X.EstadoSemestre==true).ToListAsync());
            }
            return RedirectToRoute("EjemploHome");

        }

        // GET: SemestresEscolares/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Semestres"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SemestresEscolares semestresEscolares = await db.SemestresEscolares.FindAsync(id);
                if (semestresEscolares == null)
                {
                    return HttpNotFound();
                }
                return View(semestresEscolares);
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: SemestresEscolares/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres"))
            {
                
                return View();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: SemestresEscolares/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id_Semestre,NombreSemestre,EstadoSemestre,FchInicio,FchFinal")] SemestresEscolares semestresEscolares)
        {
            var respuesta = new Validador.BaseRespuesta();
            var Existe = db.SemestresEscolares.Where(x => x.NombreSemestre == semestresEscolares.NombreSemestre).Where(x => x.EstadoSemestre == true);
            if (Existe.Count()!=0)
            {
                respuesta.Mensaje = "Ya hay un semestre con ese nombre";
                return Json(respuesta);
            }
            var interferencia = db.SemestresEscolares.Where(x => x.FchInicio <= semestresEscolares.FchInicio).Where(x=>x.FchFinal >= semestresEscolares.FchInicio).Where(x=>x.EstadoSemestre==true);

            if (interferencia.Count()!=0)
            {
                respuesta.Mensaje = "Esta fecha interfiere con otro semestre";
                return Json(respuesta);            
            }
            if (semestresEscolares.NombreSemestre!=""||semestresEscolares.FchInicio!=null)
            {
                semestresEscolares.EstadoSemestre = true;
                int Año = semestresEscolares.FchInicio.Year;
                db.SemestresEscolares.Add(semestresEscolares);
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se agrego el semestre: " + semestresEscolares.NombreSemestre + "!";
                return Json(respuesta);
            }
            respuesta.Mensaje = "Digito campos vacios!";
            return Json(respuesta);
        }

        // GET: SemestresEscolares/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SemestresEscolares semestresEscolares = await db.SemestresEscolares.FindAsync(id);
                if (semestresEscolares == null)
                {
                    return HttpNotFound();
                }
                return View(semestresEscolares);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: SemestresEscolares/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Semestre,NombreSemestre,EstadoSemestre,FchInicio,FchFinal")] SemestresEscolares semestresEscolares)
        {
            var respuesta = new Validador.BaseRespuesta();

            if (ModelState.IsValid)
            {
                semestresEscolares.EstadoSemestre = true;
                db.Entry(semestresEscolares).State = EntityState.Modified;
                var Parciales = db.Parciales.Where(x => x.Id_SemestreEscolar == semestresEscolares.Id_Semestre)
                    .Where(x => x.Estado_Parcial == true);
                foreach (var item in Parciales)
                {
                    item.Estado_Parcial = false; 
                    db.Entry(item).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se edito correctamente";
                return Json(respuesta);
            }
            respuesta.Mensaje = "Modelo no valido!";
            return Json(respuesta);
        }

        // GET: SemestresEscolares/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Semestres"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SemestresEscolares semestresEscolares = await db.SemestresEscolares.FindAsync(id);
                if (semestresEscolares == null)
                {
                    return HttpNotFound();
                }
                return View(semestresEscolares);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: SemestresEscolares/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int Id_Semestre,bool estado)
        {
            var respuesta = new Validador.BaseRespuesta();
            SemestresEscolares semestresEscolares = await db.SemestresEscolares.FindAsync(Id_Semestre);
            if (estado==true)
            {
                int DTactual = DateTime.Now.Month;
                int DTInicio = semestresEscolares.FchInicio.Month;
                int DTFinal = semestresEscolares.FchFinal.Month;
                if (DTactual>DTInicio&&DTactual<DTFinal)
                {
                    respuesta.Mensaje = "No puede borrar el semestre del año miesntras esta en curso!";
                    return Json(respuesta);
                }
                db.SemestresEscolares.Remove(semestresEscolares);
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se Borro correctamente!";
                return Json(respuesta);
            }
            else if (estado==false)
            {
                semestresEscolares.EstadoSemestre = false;
                db.Entry(semestresEscolares).State = EntityState.Modified;
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se Borro correctamente!";
                return Json(respuesta);
            }
            respuesta.Mensaje = "Ocurrio un error!";
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
        [HttpGet]
        public async Task<ActionResult> Pasar()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Pasar(int id)
        {
            var respuesta = new Validador.BaseRespuesta();
            var semestres = db.SemestresEscolares.Where(x => x.EstadoSemestre == true);
            var parciales = db.Parciales.Where(x => x.Estado_Parcial == true);
           var inicio= semestres.OrderBy(x => x.FchInicio).First().FchInicio;
           var final = semestres.OrderBy(x => x.FchFinal).First().FchFinal;
            var actual = DateTime.Now;
            if (inicio <= actual  && actual >= final)
            {
                foreach (var item in semestres)
                {
                    item.FchInicio = item.FchInicio.AddYears(1);
                    item.FchFinal = item.FchFinal.AddYears(1);
                    item.EstadoSemestre = false;
                    SemestresEscolares semestres1 = item;
                    db.Entry(item).State = EntityState.Modified;
                    db.SemestresEscolares.Add(semestres1);
                }
                foreach (var item2 in parciales)
                {
                    item2.Dt_Inicio = item2.Dt_Inicio.AddYears(1);
                    item2.Dt_Final = item2.Dt_Final.AddYears(1);
                    item2.Estado_Parcial = false;
                    Parciales parciales1 = item2;
                    db.Entry(item2).State = EntityState.Modified;
                    db.Parciales.Add(parciales1);
                }
                await db.SaveChangesAsync();
                respuesta.Mensaje = "Se curso al siguiente año";
                respuesta.Ok = true;
                return Json(respuesta);

            }
            respuesta.Mensaje = "Se encuentra en un semestre activo";
            return Json(respuesta);
        }
    }
}
