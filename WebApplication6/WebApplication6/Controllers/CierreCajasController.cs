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
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace WebApplication6.Controllers
{
    public class CierreCajasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CierreCajas
        public async Task<ActionResult> Index()
        {
            var cierreCajas = db.CierreCajas.Include(c => c.Caja).Include(c => c.Cajero);
            return View(await cierreCajas.ToListAsync());
        }

        // GET: CierreCajas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CierreCaja cierreCaja = await db.CierreCajas.FindAsync(id);
            if (cierreCaja == null)
            {
                return HttpNotFound();
            }
            return View(cierreCaja);
        }

        // GET: CierreCajas/Create
        public ActionResult Create()
        {


            var usuario = db.Users.Find(User.Identity.GetUserId());
            var empleado = db.TbEmpleado.Find(usuario.Id_Empleado);
            var empleadoGuardar = empleado.Id;
            Session["empleado"] = empleadoGuardar;


            /*
            HttpCookie myCookie = new HttpCookie("myCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("machineID", "1");

            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddYears(12);

            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);

    */






            var myCookie = Request.Cookies["myCookie"];
            if (myCookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
            }

            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(myCookie.Values["machineID"]))
            {
                var userId = "";
                userId = myCookie.Values["machineID"].ToString();
                System.Diagnostics.Debug.WriteLine("cooooooooooooooooooooooooookieeeeeeeeeeeeeee");
                System.Diagnostics.Debug.WriteLine(userId);
                Session["caja"] = userId;
            }









            var cajaString = Session["caja"].ToString();
            var cajeroINT = int.Parse(Session["empleado"].ToString());
            var empleadoCodigo = db.TbEmpleado.Where(x => x.Id == cajeroINT).Select(x => x.Codigo_Empleado).First();
            var cajeroNombreA = db.TbEmpleado.Where(x => x.Id == cajeroINT).Select(x => x.Nombre).ToArray();
            var cajeroApellidoA = db.TbEmpleado.Where(x => x.Id == cajeroINT).Select(x => x.Apellido).ToArray();
            var cajeroNombre = cajeroNombreA[0];
            var cajeroApellido = cajeroApellidoA[0];

            Debug.WriteLine("CAJERO NOMBRE EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Debug.WriteLine(cajeroNombre);

            TempData["NombreCajero"] = cajeroNombre;


            int cajaINT = int.Parse(cajaString);
            ViewBag.Caja = Session["caja"].ToString();

            ViewBag.IdCaja = new SelectList(db.Caja.Where(x => x.IdCaja == cajaINT), "IdCaja", "NumCaja");
            ViewBag.IdCajero = new SelectList(db.Cajero.Where(x => x.codigo == empleadoCodigo), "IdCajero", "NombreCompleto");





            return View();
        }

        // POST: CierreCajas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Cierre,Id_Caja,Id_Cajero,Hora_Cierre,Total_Cordobas,Total_Dolar")] CierreCaja cierreCaja)
        {
            if (ModelState.IsValid)
            {


                db.CierreCajas.Add(cierreCaja);
                await db.SaveChangesAsync();
                db.Entry(cierreCaja).GetDatabaseValues();



                int aperturaCajaID = cierreCaja.Id_Caja;
                var estadoCaja = db.CierreCajas.First(x => x.Id_Caja == aperturaCajaID);
                estadoCaja.Estado = true;
                db.SaveChanges();
                return RedirectToAction("InsertarPago", "Pago");
            }

            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", cierreCaja.Id_Caja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "Nombre", cierreCaja.Id_Cajero);
            return View(cierreCaja);
        }

        // GET: CierreCajas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CierreCaja cierreCaja = await db.CierreCajas.FindAsync(id);
            if (cierreCaja == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Caja = new SelectList(db.Caja, "IdCaja", "NumCaja", cierreCaja.Id_Caja);
            ViewBag.Id_Cajero = new SelectList(db.Cajero, "IdCajero", "Nombre", cierreCaja.Id_Cajero);
            return View(cierreCaja);
        }

        // POST: CierreCajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Cierre,Id_Caja,Id_Cajero,Hora_Cierre,Total_Cordobas,Total_Dolar")] CierreCaja cierreCaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cierreCaja).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Caja = new SelectList(db.Caja, "IdCaja", "NumCaja", cierreCaja.Id_Caja);
            ViewBag.Id_Cajero = new SelectList(db.Cajero, "IdCajero", "Nombre", cierreCaja.Id_Cajero);
            return View(cierreCaja);
        }

        // GET: CierreCajas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CierreCaja cierreCaja = await db.CierreCajas.FindAsync(id);
            if (cierreCaja == null)
            {
                return HttpNotFound();
            }
            return View(cierreCaja);
        }

        // POST: CierreCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CierreCaja cierreCaja = await db.CierreCajas.FindAsync(id);
            db.CierreCajas.Remove(cierreCaja);
            await db.SaveChangesAsync();
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
    }
}
