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
    public class PeriodosEscolaresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PeriodosEscolares
        public async Task<ActionResult> Index(string DiaSemana=null)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"],"Administrar Periodos"))
            {
                return View(await db.PeriodosEscolares.OrderBy(x=>x.DiaSemana).WhereIf(!string.IsNullOrEmpty(DiaSemana),x=>x.DiaSemana==DiaSemana)
                    .Where(x=>x.Bl_EstadoPeriodo==true).ToListAsync());
            }
            return RedirectToRoute("EjemploHome");
        }
        // GET: PeriodosEscolares/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodosEscolares periodosEscolares = await db.PeriodosEscolares.FindAsync(id);
            if (periodosEscolares == null)
            {
                return HttpNotFound();
            }
            return View(periodosEscolares);
        }

        // GET: PeriodosEscolares/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: PeriodosEscolares/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id_Periodo,DiaSemana,InicioPeriodo,FinPeriodo,Nombre_Periodo")] PeriodosEscolares periodosEscolares)
        {
            periodosEscolares.Bl_EstadoPeriodo = true;
            var respuesta = new Validador.BaseRespuesta();
            if (ModelState.IsValid)
            {
                var existe = db.PeriodosEscolares.Where(x => x.InicioPeriodo == periodosEscolares.InicioPeriodo).Where(x => x.FinPeriodo == periodosEscolares.FinPeriodo).Where(x => x.DiaSemana == periodosEscolares.DiaSemana).Where(x=>x.Bl_EstadoPeriodo==true);
                if (existe.Count()!=0)
                {
                    respuesta.Mensaje = "Ya existe esos tiempos en un periodo para ese dia";
                    return Json(respuesta);
                }
                string[] arrayName = { "Primera Hora", "Segunda Hora", "Tercera Hora", "Cuarta Hora", "Quinta Hora", "Sexta Hora", "Septima Hora", "Octava Hora" };
                //generar nombre del periodo
                var periodos = db.PeriodosEscolares.Where(x => x.DiaSemana == periodosEscolares.DiaSemana).Where(x=>x.Bl_EstadoPeriodo==true);
                if (periodos.Count() == 0)
                {
                    periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[0];
                }
                else
                {
                    switch (periodos.Count())
                    {
                        case 1:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[1];
                            break;
                        case 2:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[2];
                            break;
                        case 3:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[3];
                            break;
                        case 4:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[4];
                            break;
                        case 5:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[5];
                            break;
                        case 6:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[6];
                            break;
                        case 7:
                            periodosEscolares.Nombre_Periodo = periodosEscolares.DiaSemana + " " + arrayName[7];
                        break;
                    }
                }
                db.PeriodosEscolares.Add(periodosEscolares);
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se agrego correcatamente el periodo";
                return Json(respuesta);

            }

            respuesta.Mensaje="Digito campos vacios!";
            return Json(respuesta);
        }

        // GET: PeriodosEscolares/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodosEscolares periodosEscolares = await db.PeriodosEscolares.FindAsync(id);
            if (periodosEscolares == null)
            {
                return HttpNotFound();
            }
            return View(periodosEscolares);
        }

        // POST: PeriodosEscolares/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Periodo,Nombre_Periodo,DiaSemana,InicioPeriodo,FinPeriodo,Bl_EstadoPeriodo")] PeriodosEscolares periodosEscolares)
        {
            var respuesta = new Validador.BaseRespuesta();
            
            if (ModelState.IsValid)
            {
                db.Entry(periodosEscolares).State = EntityState.Modified;
                await db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.Mensaje = "Se edito correctamente!";
                return Json(respuesta);
            }
            respuesta.Mensaje = "Digito campos vacios";
            return Json(respuesta);
        }

        // GET: PeriodosEscolares/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodosEscolares periodosEscolares = await db.PeriodosEscolares.FindAsync(id);
            if (periodosEscolares == null)
            {
                return HttpNotFound();
            }
            return View(periodosEscolares);
        }

        // POST: PeriodosEscolares/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta();
            PeriodosEscolares periodosEscolares = await db.PeriodosEscolares.FindAsync(id);
            periodosEscolares.Bl_EstadoPeriodo = false;
            db.Entry(periodosEscolares).State = EntityState.Modified;
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
        [HttpGet]

        [HttpPost]
        public async Task<ActionResult> DesactivarHorrario(int Anio)
        {
            var CalendarioDeAño =await db.PeriodosEscolares.Where(x => x.Bl_EstadoPeriodo == true).ToListAsync();

            return View();
        }
    }
}
