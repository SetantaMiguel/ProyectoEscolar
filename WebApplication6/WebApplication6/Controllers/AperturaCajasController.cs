using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication6.Controllers
{
    public class AperturaCajasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AperturaCajas
        public async Task<ActionResult> Index()
        {
            var aperturaCaja = db.AperturaCaja.Include(a => a.Caja).Include(a => a.Cajero);



            HttpCookie myCookie = new HttpCookie("myCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("machineID", "1");

            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddYears(12);

            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);




            myCookie = Request.Cookies["myCookie"];
            if (myCookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
            }

            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(myCookie.Values["machineID"]))
            {
                string userId = myCookie.Values["machineID"].ToString();
                System.Diagnostics.Debug.WriteLine("cooooooooooooooooooooooooookieeeeeeeeeeeeeee");
                System.Diagnostics.Debug.WriteLine(userId);
            }




            return View(await aperturaCaja.ToListAsync());

            



        }

        // GET: AperturaCajas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AperturaCaja aperturaCaja = await db.AperturaCaja.FindAsync(id);
            if (aperturaCaja == null)
            {
                return HttpNotFound();
            }
            return View(aperturaCaja);
        }

        // GET: AperturaCajas/Create
        public ActionResult Create()
        {

            
            var usuario = db.Users.Find(User.Identity.GetUserId());
            var empleado = db.TbEmpleado.Find(usuario.Id_Empleado);
            var empleadoGuardar = empleado.Id;
            Session["empleado"] = empleadoGuardar;


            
            HttpCookie myCookie = new HttpCookie("myCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("machineID", "1");

            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddYears(12);

            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);

    






            myCookie = Request.Cookies["myCookie"];
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

            TempData["NombreCajero"] = cajeroNombre;
           
            
            int cajaINT = int.Parse(cajaString);
            ViewBag.Caja = Session["caja"].ToString();
             
            ViewBag.IdCaja = new SelectList(db.Caja.Where(x => x.IdCaja == cajaINT), "IdCaja", "NumCaja");
            ViewBag.IdCajero = new SelectList(db.Cajero.Include(x=>x.Empleado).Where(x => x.codigo == empleadoCodigo), "IdCajero", "Empleado.Ncompleto");




            
            return View();
        }

        // POST: AperturaCajas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IdApertura,IdCaja,IdCajero,Fecha,TotalCordoba,TotalDolar,Estado")] AperturaCaja aperturaCaja)
        {
            if (ModelState.IsValid)
            {
                
                
                db.AperturaCaja.Add(aperturaCaja);
                await db.SaveChangesAsync();
                db.Entry(aperturaCaja).GetDatabaseValues();



                int aperturaCajaID = aperturaCaja.IdApertura;
                var estadoCaja = db.AperturaCaja.First(x => x.IdApertura == aperturaCajaID);
                estadoCaja.Estado = true;
                db.SaveChanges();
                return RedirectToAction("InsertarPago", "Pago");
            }

            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", aperturaCaja.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "Nombre", aperturaCaja.IdCajero);
            return View(aperturaCaja);
        }

        // GET: AperturaCajas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AperturaCaja aperturaCaja = await db.AperturaCaja.FindAsync(id);
            if (aperturaCaja == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", aperturaCaja.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "Nombre", aperturaCaja.IdCajero);
            return View(aperturaCaja);
        }

        // POST: AperturaCajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IdApertura,IdCaja,IdCajero,Fecha,TotalCordoba,TotalDolar")] AperturaCaja aperturaCaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aperturaCaja).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", aperturaCaja.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "Nombre", aperturaCaja.IdCajero);
            return View(aperturaCaja);
        }

        // GET: AperturaCajas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AperturaCaja aperturaCaja = await db.AperturaCaja.FindAsync(id);
            if (aperturaCaja == null)
            {
                return HttpNotFound();
            }
            return View(aperturaCaja);
        }

        // POST: AperturaCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AperturaCaja aperturaCaja = await db.AperturaCaja.FindAsync(id);
            db.AperturaCaja.Remove(aperturaCaja);
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
