using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Permisos"))
            {
                var roles = db.Roles;
                return View(roles.ToList());
            }
            return RedirectToRoute("EjemploHome");
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Administrar Permisos"))
            {
                return View();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")] Microsoft.AspNet.Identity.EntityFramework.IdentityRole Roles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(Roles).ToString();
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
