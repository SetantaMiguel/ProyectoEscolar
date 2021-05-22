using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;


namespace WebApplication6.Controllers
{   [Authorize]    
    public class ControlDisciplinariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int auxid = 0;

        // GET: ControlDisciplinarios
        public ActionResult Index(string MesFiltro, string AnioFiltro,int id, int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 8;
            auxid = id;
            ViewBag.id = id;
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id,"Ver Historial Diciplinario");

            if (respues == true)
            {

                if (ViewBag.ErrorFiltro == false || ViewBag.ErrorFiltro == null)
                {
                    int mes = 0;
                    if (!string.IsNullOrEmpty(MesFiltro))
                    {
                        mes = Convert.ToInt32(MesFiltro);
                    }

                    var ListaDisciplinaria = db.tbcontrolDisciplinario.Include(x => x.Estudiantes).Include(x => x.IdentityUser.Empleado).WhereIf(id != 0, x => x.Id_Estudiantes == id)
                      .WhereIf(mes!=0,x=>x.Fecha_Emision.Month==mes)
                      .WhereIf(!string.IsNullOrEmpty(AnioFiltro),x=>x.Fecha_Emision.Year.ToString()==AnioFiltro)
                     .OrderByDescending(x => x.Fecha_Emision)
                     .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina).ToList();

                    var totalDeRegistros = db.tbcontrolDisciplinario.WhereIf(!string.IsNullOrEmpty(id.ToString()), x => x.Id_Estudiantes == id)
                      .WhereIf(!string.IsNullOrEmpty(MesFiltro), x => x.Fecha_Emision.Month == mes)
                      .WhereIf(!string.IsNullOrEmpty(AnioFiltro), x => x.Fecha_Emision.Year.ToString() == AnioFiltro).Count();
                    var modelo = new IndexViewModel2();
                    modelo.controlDisciplinarios = ListaDisciplinaria;
                    modelo.PaginaActual = pagina;
                    modelo.TotalRegistros = totalDeRegistros;
                    modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                    if (!string.IsNullOrEmpty(id.ToString())&&id!=0)
                    {
                        ViewBag.ModeloEstudiante = true;
                        ViewBag.name = db.tbestudiantes.Find(id).Nombre + " " + db.tbestudiantes.Find(id).Apellido;
                    }

                    return View(modelo);
                   
                }
            }
            else
            {
                return RedirectToRoute("EjemploHome");
            }



            return View();
        }



        // GET: ControlDisciplinarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlDisciplinario controlDisciplinario = db.tbcontrolDisciplinario.Find(id);
            if (controlDisciplinario == null)
            {
                return HttpNotFound();
            }
            return View(controlDisciplinario);
        }

        // GET: ControlDisciplinarios/Create
        public ActionResult Create()
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Crear Falta Disciplinaria");
            if (respues == true)
            {
                ViewBag.id = auxid;
                return PartialView();
            }
            else
            {
                return RedirectToRoute("EjemploHome");
            }

        }

        // POST: ControlDisciplinarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id_Control,Fecha_Emision,Descripcion")] ControlDisciplinario controlDisciplinario)
        {
            var resuldo =new Validador.BaseRespuesta();
            if (ModelState.IsValid)
            {
                
                controlDisciplinario.Id = User.Identity.GetUserId();
                controlDisciplinario.Id_Estudiantes = Convert.ToInt32(auxid);
                if (controlDisciplinario.Id_Estudiantes > 0)
                {
                    db.tbcontrolDisciplinario.Add(controlDisciplinario);
                    db.SaveChanges();
                    resuldo.Ok=true;
                    resuldo.Mensaje = "Se creo el control correctamente";
                    return Json(resuldo);
                }
            }

            return View(controlDisciplinario);
        }

        // GET: ControlDisciplinarios/Edit/5
        public ActionResult Edit(int? id)
        {

            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Editar Falta Disciplinaria");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ControlDisciplinario controlDisciplinario = db.tbcontrolDisciplinario.Find(id);
                if (controlDisciplinario == null)
                {
                    return HttpNotFound();
                }
                return PartialView(controlDisciplinario);
            }
            else
            {
                return RedirectToRoute("EjemploHome");
            }

        }

        // POST: ControlDisciplinarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Control,Fecha_Emision,Descripcion")] ControlDisciplinario controlDisciplinario)
        {
            if (ModelState.IsValid)
            {
                controlDisciplinario.Id = User.Identity.GetUserId();
                controlDisciplinario.Id_Estudiantes = Convert.ToInt32(auxid);
                if (controlDisciplinario.Id_Estudiantes > 0)
                {
                    db.Entry(controlDisciplinario).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessEdit"] = true;
                    return RedirectToAction("Index", new { id = controlDisciplinario.Id_Estudiantes });
                }
            }
            return View(controlDisciplinario);
        }

        // GET: ControlDisciplinarios/Delete/5
        public ActionResult Delete(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Borrar Falta Diciplinario");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ControlDisciplinario controlDisciplinario = db.tbcontrolDisciplinario.Include(x => x.Estudiantes).Where(x => x.Id_Control == id).First();

                if (controlDisciplinario == null)
                {
                    return HttpNotFound();
                }
                return PartialView(controlDisciplinario);
            }
            else
            {
                return RedirectToRoute("EjemploHome");
            }

        }

        // POST: ControlDisciplinarios/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta();
            ControlDisciplinario controlDisciplinario = db.tbcontrolDisciplinario.Find(id);
            db.tbcontrolDisciplinario.Remove(controlDisciplinario);
            db.SaveChanges();
            respuesta.Ok = true;
            respuesta.Mensaje = "Se borro correctamente la falta ";
            if (auxid==0)
            {
                return Json(respuesta);
            }
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
