using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{

    [Authorize]
    public class EmpleadoController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Empleado
        public ActionResult Index( int pagina = 1)
        {
            if (Validador.PuedeEntrar2(Session["Permisos"], "Ver Empleados"))
            {
                var cantidadRegistrosPorPagina = 10;

                using (var db2 = new ApplicationDbContext())
                {
                    // Func<Estudiantes, bool> predicado = x => !nameUser.HasValue || Name.Value == x.Id;
                
                    var empleado = db.TbEmpleado.OrderBy(x => x.Id)
                     .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina).ToList();

                    var totalDeRegistros = db.TbEmpleado.Count();
                    var modelo = new IndexViewModel2
                    {
                        Empleados = empleado,
                        PaginaActual = pagina,
                        TotalRegistros = totalDeRegistros,
                        RegistrosPorPagina = cantidadRegistrosPorPagina
                    };
                    return View(modelo);

                }
            }
                return RedirectToRoute("EjemploHome");
        }

        [HttpPost]
        public ActionResult Index(string Codigo, string Name,string LastName, int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10;

                var empleado = db.TbEmpleado
                    .WhereIf(!string.IsNullOrEmpty(Name),x=>x.Nombre==Name)
                    .WhereIf(!string.IsNullOrEmpty(LastName),x=>x.Apellido==LastName)
                    .WhereIf(!string.IsNullOrEmpty(Codigo),u => u.Codigo_Empleado == Codigo).
                    OrderBy(x => x.Id)
                 .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                 .Take(cantidadRegistrosPorPagina).ToList();

                var totalDeRegistros = db.TbEmpleado.WhereIf(!string.IsNullOrEmpty(Name), x => x.Nombre == Name)
                    .WhereIf(!string.IsNullOrEmpty(LastName), x => x.Apellido == LastName)
                    .WhereIf(!string.IsNullOrEmpty(Codigo), u => u.Codigo_Empleado == Codigo).Count();

            var modelo = new IndexViewModel2
            {
                Empleados = empleado,
                PaginaActual = pagina,
                TotalRegistros = totalDeRegistros,
                RegistrosPorPagina = cantidadRegistrosPorPagina
            };
            if (empleado.Count() == 0)
                {
                    ViewBag.UserVacio = "Usuario no encontrado";
                     empleado = db.TbEmpleado.OrderBy(x => x.Id)
                     .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina).ToList();

                    totalDeRegistros = db.TbEmpleado.Count();
                    modelo = new IndexViewModel2
                    {
                        Empleados = empleado,
                        PaginaActual = pagina,
                        TotalRegistros = totalDeRegistros,
                        RegistrosPorPagina = cantidadRegistrosPorPagina
                    };
                    return View(modelo);

                }
                ViewBag.UserVacio = "Usuario Encontrado";
                return View(modelo);
            

        }

        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            bool respues = Validador.PuedeEntrar2(Session["Permisos"], "Ver Empleados");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.TbEmpleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            return RedirectToRoute("EjemploHome");
        }
        public async Task<ActionResult> DetailsModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = await db.TbEmpleado.FindAsync(id);
            if (empleado== null)
            {
                return HttpNotFound();
            }
            return PartialView(empleado);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            bool respues = Validador.PuedeEntrar2(Session["Permisos"], "Administrar Empleados");
            if (respues == true)
            {
                return View();
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Empleado/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellido,Direccion,Correo,FechaNaci,Domicilio,Telefono,Genero,Identificacion,Codigo_Empleado,EstadoCivil,Dt_Contratacion,FirstLogin,BL_ProfesorGuia")] Empleado empleado)
        {

                var exite = db.TbEmpleado.Where(x => x.Correo == empleado.Correo);
                if (exite.Count() != 0)
                {
                    empleado.Correo = "";
                    ViewBag.Existe = "Ya existe un correo en el sistema!";
                    return View(empleado);
                }
                if (ModelState.IsValid)
                {
                    var last = db.TbEmpleado.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (last == null || last.Id == 0)
                    {
                        empleado.Codigo_Empleado = "School-" + DateTime.Now.Year + "-" + 0;

                    }
                    empleado.Codigo_Empleado = "School-" + DateTime.Now.Year + "-" + last.Id;
                    empleado.BL_Estado_Empleado = true;
                    db.TbEmpleado.Add(empleado);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception c)
                {
                    Console.WriteLine(c);
                    throw;
                }
                    return RedirectToAction("Index");
                }

                return View(empleado);

        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            bool respues = Validador.PuedeEntrar2(Session["Permisos"], "Administrar Empleados");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.TbEmpleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            return RedirectToRoute("EjemploHome");

        }

        // POST: Empleado/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,Direccion,Correo,FechaNaci,Domicilio,Telefono,Genero,Identificacion,Codigo_Empleado,EstadoCivil,Dt_Contratacion,FirstLogin,BL_ProfesorGuia")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.BL_Estado_Empleado = true;
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new {id=empleado.Id });
            }
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            bool respues = Validador.PuedeEntrar2(Session["Permisos"], "Administrar Empleados");
            if (respues == true)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.TbEmpleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            return RedirectToRoute("EjemploHome");
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.TbEmpleado.Find(id);
            empleado.BL_Estado_Empleado = false;
            db.Entry(empleado).State = EntityState.Modified;
            db.SaveChanges();
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
        public JsonResult BuscarPersona(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.TbEmpleado.Where(x => x.Codigo_Empleado.Contains(term))
                    .Select(x => x.Codigo_Empleado).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }   
        public JsonResult BuscarPersonaName(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.TbEmpleado.Where(x => x.Nombre.Contains(term))
                    .Select(x => x.Nombre).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }  
        public JsonResult BuscarPersonaLastName(string term)
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var resultado = db2.TbEmpleado.Where(x => x.Apellido.Contains(term))
                    .Select(x => x.Apellido).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Generos()
        {
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var respuesta = new GeneroRespuesta();
                var resultado = db2.TbEmpleado.Where(x => x.Genero == "Masculino")
                    .Count();
                respuesta.Masculino = resultado;
                var resultado2 = db2.TbEmpleado.Where(x => x.Genero == "Femenino")
                    .Count();
                respuesta.Femenino = resultado2;
                return Json(respuesta, JsonRequestBehavior.AllowGet);
            }
           
        }
        public JsonResult Edades()
        {
            
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var res =new  BaseRespuesta();
                var resultado = db2.TbEmpleado.
                    Where(x => DateTime.Now.Year - x.FechaNaci.Year  >=20 && DateTime.Now.Year-x.FechaNaci.Year <= 30)
                    .Count();
                var resultado2 = db2.TbEmpleado.
                    Where(x => DateTime.Now.Year - x.FechaNaci.Year >=31 && DateTime.Now.Year-x.FechaNaci.Year  <= 40)
                    .Count(); 
                var resultado3 = db2.TbEmpleado.
                    Where(x => DateTime.Now.Year - x.FechaNaci.Year  >=41 && DateTime.Now.Year-x.FechaNaci.Year <= 50)
                    .Count();
                var resultado4 = db2.TbEmpleado.
                    Where(x => DateTime.Now.Year - x.FechaNaci.Year >=51 && DateTime.Now.Year-x.FechaNaci.Year  <= 60)
                    .Count();
                var resultado5 = db2.TbEmpleado.
                    Where(x => DateTime.Now.Year - x.FechaNaci.Year >=61 )
                    .Count();

                res.veintetreinta = resultado;
                res.treintacuarenta = resultado2;
                res.cuarentaCincuenta = resultado3;
                res.CincuentaSesenta = resultado4;
                res.SesentaMas = resultado5;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Estudios()
        {
            
            using (ApplicationDbContext db2 = new ApplicationDbContext())
            {
                var res =new  EstudiosRespuesta();
                var resultado = db2.tbEstudios_Maestro.Where(x => x.Str_Tipo_Estudio == "Bachiller").Where(x => x.Bl_Estado_Estudio == true)
                    .Count();
                var resultado2 = db2.tbEstudios_Maestro.Where(x => x.Str_Tipo_Estudio == "Licenciatura").Where(x => x.Bl_Estado_Estudio == true)
                    .Count();
                var resultado3 = db2.tbEstudios_Maestro.Where(x => x.Str_Tipo_Estudio == "Doctorado").Where(x => x.Bl_Estado_Estudio == true)
                    .Count();
                var resultado4 = db2.tbEstudios_Maestro.Where(x => x.Str_Tipo_Estudio == "Tecnico").Where(x => x.Bl_Estado_Estudio == true)
                    .Count(); 
                var resultado5 = db2.tbEstudios_Maestro.Where(x => x.Str_Tipo_Estudio == "Maestria").Where(x => x.Bl_Estado_Estudio == true)
                    .Count();

                res.Bachiller = resultado;
                res.Licenciatura = resultado2;
                res.Doctorado = resultado3;
                res.Tecnico= resultado4;
                res.Maestria = resultado5;

                return Json(res, JsonRequestBehavior.AllowGet);
            }

        }
        public class BaseRespuesta
        {
            public int veintetreinta { get; set; }
            public int treintacuarenta { get; set; }
            public int cuarentaCincuenta{ get; set; }
            public int CincuentaSesenta { get; set; }
            public int SesentaMas{ get; set; }
        }
        public class GeneroRespuesta {
            public int Masculino { get; set; }
            public int Femenino { get; set; }
        }
        public class EstudiosRespuesta {
            public int Bachiller { get; set; }
            public int Licenciatura { get; set; }
            public int Doctorado { get; set; }
            public int Tecnico { get; set; }
            public int Maestria { get; set; }
        }
    }
}