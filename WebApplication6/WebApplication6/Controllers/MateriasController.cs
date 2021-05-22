using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class MateriasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Materias
        public ActionResult Index(string txtMateria, int pagina = 1)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar(usuario.Id, "Administrar Materias");
            if (respues == true)
            {
                var cantidadRegistrosPorPagina = 10;
                var materias = db.tbmaterias.Include(x=>x.AniosACursar).OrderBy(x => x.Id_Materia)
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                 .Take(cantidadRegistrosPorPagina).WhereIf(!string.IsNullOrEmpty(txtMateria), x => x.Nombre_Materia.Contains(txtMateria)).Where(x => x.EstadoMateria == true).ToList();

                var totalDeRegistros = db.tbmaterias.Include(x => x.AniosACursar).WhereIf(!string.IsNullOrEmpty(txtMateria), x => x.Nombre_Materia.Contains(txtMateria)).Where(x => x.EstadoMateria == true).Count();
                var modelo = new IndexViewModel2();
                modelo.Materias = materias;
                modelo.PaginaActual = pagina;
                modelo.TotalRegistros = totalDeRegistros;
                modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
                return View(modelo);
            }
            return RedirectToRoute("EjemploHome");

        }

        // GET: Materias/Create
        public ActionResult Create()
        {
            ViewBag.Id_AnioEscolar = new SelectList(db.tbAniosACursar, "Id", "Str_Curso");

            return PartialView();
        }

        // POST: Materias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Codigo_Materia,Nombre_Materia")] Materias materias,int Id_AnioEscolar)
        {
            var respuesta = new Validador.BaseRespuesta();

            if (ModelState.IsValid)
            {
                var existe = db.tbmaterias.Where(x=>x.Nombre_Materia==materias.Nombre_Materia).Where(x=>x.Id_AñoEscolar==Id_AnioEscolar).Where(x=>x.EstadoMateria==true);
                if (existe.Count()!=0)
                {
                    respuesta.Mensaje = "Ya existe esa materia";
                    return Json(respuesta);
                }
                materias.Id_AñoEscolar = Id_AnioEscolar;
                materias.EstadoMateria = true;
                db.tbmaterias.Add(materias);
                db.SaveChanges();
                respuesta.Ok = true;
                respuesta.Mensaje = "La materia "+materias.Nombre_Materia+" se agrego correctamente";
                return Json(respuesta);
            }
            respuesta.Mensaje = "Digito campos vacios";

            return Json(respuesta);
        }

        // GET: Materias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materias materias = db.tbmaterias.Find(id);
            ViewBag.Id_AnioEscolar = materias.Id_AñoEscolar ;
            if (materias == null)
            {
                return HttpNotFound();
            }
            return View(materias);
        }

        // POST: Materias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id_Materia,Codigo_Materia,Nombre_Materia")] Materias materias, int Id_AnioEscolar)
        {
            var respuesta = new Validador.BaseRespuesta();
            if (ModelState.IsValid)
            {
                materias.EstadoMateria = true;
                materias.Id_AñoEscolar = Id_AnioEscolar;
                db.Entry(materias).State = EntityState.Modified;
                db.SaveChanges();
                respuesta.Ok=true;
                respuesta.Mensaje = "Se edito correctamente la materia " + materias.Nombre_Materia + " !";
                return Json(respuesta);
            }
            respuesta.Mensaje = "Digito campos vacios o invalidos!";
            return Json(respuesta);
        }

        // GET: Materias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materias materias = db.tbmaterias.Find(id);
            if (materias == null)
            {
                return HttpNotFound();
            }
            return View(materias);
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var respuesta = new Validador.BaseRespuesta();
            Materias materias = db.tbmaterias.Find(id);
            materias.EstadoMateria = false;
            db.Entry(materias).State = EntityState.Modified;
            db.SaveChanges();
            respuesta.Ok = true;
            respuesta.Mensaje = "Se borro la materia " + materias.Nombre_Materia + " Correctamente";
            return Json(respuesta);
        }

        public JsonResult BuscarMaterias(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.tbmaterias.Where(x => x.Nombre_Materia.Contains(term))
                    .Select(x => x.Nombre_Materia).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

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
