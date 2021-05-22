using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class PermisosYRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static string IdRol = "";
        // GET: PermisosYRoles
        public ActionResult Index(string Id)
        {
            IdRol = Id;
            var tbpermisosyroles = db.tbpermisosyroles.Include(x=>x.Permisos).Include(x=>x.IdentityRole).Where(x=>x.Id_Rol==IdRol);
            return View(tbpermisosyroles.ToList());
        }

        // GET: PermisosYRoles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PermisosYRoles/Create
        public ActionResult Create()
        {
            List<PermisosYRoles> permisosDeRol = db.tbpermisosyroles.Where(x => x.Id_Rol == IdRol).ToList();
            List<Permisos> permisostotales = db.tbPermisos.ToList();
            List<Permisos> permisosFinal= db.tbPermisos.ToList();
            foreach (var item in permisostotales)
            {
                foreach (var item2 in permisosDeRol)
                {
                    if (item.Id_Permiso == item2.Id_Permiso)
                    {
                        permisosFinal.Remove(item);
                    }
                }
            }
            ViewBag.Id_Permiso = new SelectList(permisosFinal, "Id_Permiso", "NombrePermiso");
            ViewBag.Id_Rol = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

        // POST: PermisosYRoles/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id_Permiso")] PermisosYRoles permisosYRoles)
        {
            var respuesta =new Validador.BaseRespuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    permisosYRoles.Id_Rol = IdRol;
                    db.tbpermisosyroles.Add(permisosYRoles);
                    db.SaveChanges();
                    respuesta.Ok = true;
                    respuesta.Mensaje = "Se agrego correctamente el permiso!";
                    return Json(respuesta);
                }

                List<PermisosYRoles> permisosDeRol = db.tbpermisosyroles.Where(x => x.Id_Rol == IdRol).ToList();
                List<Permisos> permisostotales = db.tbPermisos.ToList();
                List<Permisos> permisosFinal = db.tbPermisos.ToList();
                foreach (var item in permisostotales)
                {
                    foreach (var item2 in permisosDeRol)
                    {
                        if (item.Id_Permiso == item2.Id_Permiso)
                        {
                            permisosFinal.Remove(item);
                        }
                    }
                }
                ViewBag.Id_Permiso = new SelectList(permisosFinal, "Id_Permiso", "NombrePermiso");
                respuesta.Mensaje = "Ocurrio un problema en el servidor!";
                return Json(respuesta);



            }
            catch
            {
                return Json(respuesta);
            }
        }

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermisosYRoles tbpermisosyroles = db.tbpermisosyroles.Include(x => x.Permisos).Include(x => x.IdentityRole).Where(x => x.Id == id).First();
            if (tbpermisosyroles == null)
            {
                return HttpNotFound();
            }
            return View(tbpermisosyroles);
        }

        // POST: PermisosYRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           PermisosYRoles permisosYRoles=db.tbpermisosyroles.Find(id);
            db.tbpermisosyroles.Remove(permisosYRoles);
            db.SaveChanges();
            return RedirectToAction("Index","Roles",null);
        }
    }
    }

