using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class PagoController : Controller
    {
        private static string val;
        private static string fechaHoy;

        private static int trueIDPAGO;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private int idConceptoDeudor;
        private string idEstudianteDeudor;
        private decimal montoDeudor;
        private decimal pagoAbonadoDeudor;
        private bool esDeudor;


        // GET: Pago
        public async Task<ActionResult> Index(string DropCajeros, string DropCajas)
        {
            var dropcajerosList = new SelectList(db.Cajero, "IdCajero", "codigo").ToList();
            var placeholder = new SelectListItem {Text = "--Seleccionar--", Value = null};
            dropcajerosList.Insert(0, placeholder);
            ViewBag.DropCajeros = dropcajerosList;

            var dropcajaList = new SelectList(db.Caja, "IdCaja", "NumCaja").ToList();

            dropcajaList.Insert(0, placeholder);
            ViewBag.DropCajas = dropcajaList;


            var pago = db.Pago.Include(p => p.Caja).Include(p => p.Cajero);
            if (DropCajeros == "--Seleccionar--" && DropCajas == "--Seleccionar--")
            {
                pago = db.Pago.Include(p => p.Caja).Include(p => p.Cajero);
                return View(await pago.ToListAsync());
            }

            if (DropCajeros != "--Seleccionar--" && DropCajas == "--Seleccionar--")
            {
                var DropCajerosINT = int.Parse(DropCajeros);
                //DropCajeros = Session["cajero"].ToString();
                Debug.WriteLine("IT HITS??????????????");
                Debug.WriteLine(DropCajeros);

                pago = db.Pago.Include(p => p.Caja).Include(p => p.Cajero)
                    .Where(x => x.IdCajero.Equals(DropCajerosINT));
                return View(await pago.ToListAsync());
            }

            if (DropCajas != "--Seleccionar--" && DropCajeros == "--Seleccionar--")
            {
                var DropCajasINT = int.Parse(DropCajas);
                pago = db.Pago.Include(p => p.Caja).Include(p => p.Cajero).Where(x => x.IdCaja.Equals(DropCajasINT));
                return View(await pago.ToListAsync());
            }

            if (DropCajeros != null && DropCajas != null)
            {
                Debug.WriteLine("cajeros");
                Debug.WriteLine(DropCajeros);


                Debug.WriteLine("cajas");
                Debug.WriteLine(DropCajas);


                var DropCajerosINT = int.Parse(DropCajeros);
                var DropCajasINT = int.Parse(DropCajas);
                pago = db.Pago.Include(p => p.Caja).Include(p => p.Cajero).Where(x =>
                    x.IdCajero.Equals(DropCajerosINT) && x.IdCaja.Equals(DropCajasINT));
                return View(await pago.ToListAsync());
            }


            return View(await pago.ToListAsync());
        }


        // GET: Pago/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pago = await db.Pago.FindAsync(id);
            if (pago == null) return HttpNotFound();
            return View(pago);
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
            var usuario = db.Users.Find(User.Identity.GetUserId());
            var cajeroINT = usuario.Id_Empleado ;
            var empleadoCodigo = db.TbEmpleado.Where(x => x.Id == cajeroINT).Select(x => x.Codigo_Empleado).First();
            var cajeroNombreA = db.TbEmpleado.Where(x => x.Id == cajeroINT).Select(x => x.Nombre).ToArray();
            var cajeroApellidoA = db.TbEmpleado.Where(x => x.Id == cajeroINT).Select(x => x.Apellido).ToArray();
            var cajeroNombre = cajeroNombreA[0];
            var cajeroApellido = cajeroApellidoA[0];

            Debug.WriteLine("CAJERO NOMBRE EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Debug.WriteLine(cajeroNombre);

            TempData["NombreCajero"] = cajeroNombre;


          //  ViewBag.Caja = Session["caja"].ToString();
            fechaHoy = DateTime.Now.ToString("yyyy/MM/dd");
            ViewBag.Carnet = val;
            ViewBag.Fecha = fechaHoy;
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja");

            ViewBag.IdCajero= new SelectList(db.Cajero.Include(x=>x.Empleado).Where(x => x.codigo == empleadoCodigo), "IdCajero",
                "Empleado.NCompleto");

            ViewBag.IdEstudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "NombreCompleto");
            return View();
        }



        



        // GET: DetallePago/Create
        public ActionResult CreateDetallePago()
        {
            ViewBag.Why = trueIDPAGO;

            Session["idFactura"] = trueIDPAGO;

            ViewBag.IdPago = new SelectList(db.Pago, "Id_Pago", "Id_Pago");
            ViewBag.IdConceptoPago = new SelectList(db.ConceptosPago, "Id_Concepto", "NombreConcepto");
            ViewBag.IdTipo = new SelectList(db.TipoPago, "Id_TipoPago", "Nombre");
            ViewBag.IdMoneda = new SelectList(db.Moneda, "Id_Moneda", "Nombre");
            ViewBag.IdEquivalencia = new SelectList(db.Equivalencia, "Id_Equivalencia", "Monto");
            ViewBag.IdEstado = new SelectList(db.Estado, "Id_Estado", "Nombre");
            return View();
        }


        //GET: INTRODUCIR ID 
        public ActionResult InsertarPago()
        {
            var fechaHoy = DateTime.Today;
            var verificarApertura = db.AperturaCaja.Any(x => x.Fecha == fechaHoy);

            if (verificarApertura) return View();

            return RedirectToAction("Create", "AperturaCajas");
        }


        //POST: INTRODUCIR ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InsertarPago(string carnet)
        {
            val = carnet;
            ViewBag.Carnet = val;
            Session["carnet"] = val;
            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHH");
            Debug.WriteLine(val);




            return RedirectToAction("Create", "Pago");
        }


        // POST: Pago/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id_Pago,FechaTransaccion,IdCajero,IdCaja,IdEstudiante")]
            Pago pago)
        {
            if (ModelState.IsValid)
            {
                var PagoId = new Pago(); //no Id yet;
                var NuevaIdPago = new DetallePago();


                db.Pago.Add(pago);
                await db.SaveChangesAsync();

                db.Entry(pago).GetDatabaseValues();


                trueIDPAGO = pago.Id_Pago;
                Session["idFactura"] = trueIDPAGO;

                ViewBag.Why = trueIDPAGO;

                Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHH");
                Debug.WriteLine(trueIDPAGO);

                return RedirectToAction("CreateDetallePago", "Pago");
            }


            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", pago.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "codigo", pago.IdCajero);
            ViewBag.IdEstudiante = new SelectList(db.tbestudiantes, "Id_Estudiante", "NombreCompleto");
            return View(pago);
        }



        //FACTURA : GET
        public ActionResult Factura()
        {
            return View();
        }


        /*
        public ActionResult ImprimirFactura(int? id)
        {
            id = trueIDPAGO;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            var impresion = from c in db.Facturas where c.Id_Factura == id select c;

            return new RazorPDF.PdfResult(impresion, "Factura");
        }
        */




        // POST: DetallePagoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDetallePago(
            [Bind(Include =
                "IdPago,IdConcepto,Id_Tipo,Monto,IdMoneda,PagoAbonado,IdEquivalencia,Balance,IdEstado,Mora,Cambio,Total")]
            DetallePago detallePago)
        {
            if (ModelState.IsValid)
            {
                


                db.DetallePago.Add(detallePago);
                await db.SaveChangesAsync();

                //Guardar el concepto ingresado
                db.Entry(detallePago).GetDatabaseValues();

                idConceptoDeudor = detallePago.IdConcepto;
                pagoAbonadoDeudor = detallePago.PagoAbonado;
                montoDeudor = detallePago.Monto;




                //Copiar el pago a una factura
                var tipoPago = detallePago.Id_Tipo;
                var equivalencia = detallePago.IdEquivalencia;
                var moneda = detallePago.IdMoneda;
                var cambio = detallePago.Cambio;
                var total = detallePago.Total;
                var mora = detallePago.Mora;

                int cajaINT = int.Parse(Session["caja"].ToString());
                int empleadoINT = Int32.Parse((Session["empleado"].ToString()));
                var cajeroCodigo = db.TbEmpleado.Where(x => x.Id == empleadoINT).Select(x => x.Codigo_Empleado).First();
                var cajeroTrue = db.Cajero.Where(x => x.codigo == cajeroCodigo).Select(x => x.IdCajero).First();
                string estudiante = Session["carnet"].ToString();
                

                
                





                //Validar si estudiante es un deudor, usando el string val de Pago/insertarPago()

                var idEstudianteArray = db.tbestudiantes.Where(x => x.Codigo_Estudiante == val)
                    .Select(d => d.Id_Estudiante).ToArray();
                var idEstudiante = idEstudianteArray[0];

                var idEstudianteExiste = db.Deudores.Where(x => x.Id_Estudiante == idEstudiante).Any();
                if (idEstudianteExiste)
                {
                    Debug.WriteLine("ESTUDIANTE EXISTE");
                    var idDeudorArray = db.Deudores.Where(x => x.Id_Estudiante == idEstudiante).Select(d => d.Id_Deudor)
                        .ToArray();
                    var idDeudor = idDeudorArray[0];
                    var idConceptoExiste = db.DetalleDeudor.Where(x => x.IdDeudor == idDeudor && x.IdConceptop == idConceptoDeudor).Any();

                    if (idConceptoExiste)
                    {
                        Debug.WriteLine("CONCEPTO EXISTE");
                        esDeudor = true;
                        var montoAbonadoPrevio = db.DetalleDeudor.Where(x => x.IdDeudor == idDeudor && x.IdConceptop == idConceptoDeudor).Select(x => x.Total_Pagado).First();
                        var totalDeberDeudor = montoDeudor - (pagoAbonadoDeudor + montoAbonadoPrevio);
                        var mytab = db.DetalleDeudor.First(g =>
                            g.IdDeudor == idDeudor && g.IdConceptop == idConceptoDeudor);
                        mytab.Total_Pagado = (pagoAbonadoDeudor + montoAbonadoPrevio);

                        mytab.Total_Deber = totalDeberDeudor;


                        db.SaveChanges();

                        var totalACTUALIZAR = db.Deudores.Where(g =>
                            g.Id_Deudor == idDeudor).Select(x => x.Total_Pagar).First();
                        var mytab2 = db.Deudores.First(g =>
                            g.Id_Deudor == idDeudor);
                        mytab2.Total_Pagar = totalACTUALIZAR - (pagoAbonadoDeudor + montoAbonadoPrevio);
                        db.SaveChanges();

                        var mytab3 = db.Deudores.First(g =>
                            g.Id_Deudor == idDeudor);
                        var totalACTUALIZAR2 = db.Deudores.Where(g =>
                            g.Id_Deudor == idDeudor).Select(x => x.Total_Pagar).First();
                        if (totalACTUALIZAR2 == 0)
                        {
                            mytab3.IdEstado = 2;
                        }
                        db.SaveChanges();

                        





                        /*
                        using (db)
                        {
                            Factura nuevaFactura = new Factura();
                            nuevaFactura.Id = trueIDPAGO;
                            nuevaFactura.FechaFactura = DateTime.Now;
                            nuevaFactura.IdCaja = cajaINT;
                            nuevaFactura.IdCajero = cajeroTrue;
                            nuevaFactura.IdEstudiante = estudiante;
                            nuevaFactura.PagoAbonado = pagoAbonadoDeudor;
                            nuevaFactura.IdMoneda = moneda;
                            nuevaFactura.IdEquivalencia = equivalencia;
                            nuevaFactura.IdConcepto = idConceptoDeudor;
                            nuevaFactura.Id_Tipo = tipoPago;
                            nuevaFactura.Cambio = cambio;
                            nuevaFactura.Total = total;
                            db.Facturas.Add(nuevaFactura);
                            db.SaveChanges();


                            
                        }

    */


                    }

                    ;
                }


                ;
                using (db)
                {
                    Factura nuevaFactura = new Factura();
                    nuevaFactura.Id = trueIDPAGO;
                    nuevaFactura.FechaFactura = DateTime.Now;
                    nuevaFactura.IdCaja = cajaINT;
                    nuevaFactura.IdCajero = cajeroTrue;
                    nuevaFactura.IdEstudiante = estudiante;
                    nuevaFactura.PagoAbonado = pagoAbonadoDeudor;
                    nuevaFactura.IdMoneda = moneda;
                    nuevaFactura.IdEquivalencia = equivalencia;
                    nuevaFactura.IdConcepto = idConceptoDeudor;
                    nuevaFactura.Id_Tipo = tipoPago;
                    nuevaFactura.Mora = mora;
                    nuevaFactura.Cambio = cambio;
                    nuevaFactura.Total = total;
                    db.Facturas.Add(nuevaFactura);
                    db.SaveChanges();
                }

                return RedirectToAction("CreateDetallePago", "Pago");
            }

            ViewBag.IdPago = new SelectList(db.Pago, "Id_Pago", "Id_Pago");
            ViewBag.IdConceptoPago = new SelectList(db.ConceptosPago, "Id_Concepto", "NombreConcepto");
            ViewBag.IdTipo = new SelectList(db.TipoPago, "Id_TipoPago", "Nombre");
            ViewBag.IdMoneda = new SelectList(db.Moneda, "Id_Moneda", "Nombre");
            ViewBag.IdEquivalencia = new SelectList(db.Equivalencia, "Id_Equivalencia", "Monto");
            ViewBag.IdEstado = new SelectList(db.Estado, "Id_Estado", "Nombre");


            return RedirectToAction("CreateDetallePago", "Pago");
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDetalleFactura(
            [Bind(Include =
                "IdPago,IdConcepto,Id_Tipo,Monto,IdMoneda,PagoAbonado,IdEquivalencia,Balance,IdEstado,Mora")]
            DetallePago detallePago)
        {
            if (ModelState.IsValid)
            {



                db.DetallePago.Add(detallePago);
                await db.SaveChangesAsync();

                //Guardar el concepto ingresado
                db.Entry(detallePago).GetDatabaseValues();

                idConceptoDeudor = detallePago.IdConcepto;
                pagoAbonadoDeudor = detallePago.PagoAbonado;
                montoDeudor = detallePago.Monto;




                //Copiar el pago a una factura
                var tipoPago = detallePago.Id_Tipo;
                var equivalencia = detallePago.IdEquivalencia;
                var moneda = detallePago.IdMoneda;

                int cajaINT = int.Parse(Session["caja"].ToString());
                int empleadoINT = Int32.Parse((Session["empleado"].ToString()));
                var cajeroCodigo = db.TbEmpleado.Where(x => x.Id == empleadoINT).Select(x => x.Codigo_Empleado).First();
                var cajeroTrue = db.Cajero.Where(x => x.codigo == cajeroCodigo).Select(x => x.IdCajero).First();
                string estudiante = Session["carnet"].ToString();









                //Validar si estudiante es un deudor, usando el string val de Pago/insertarPago()

                var idEstudianteArray = db.tbestudiantes.Where(x => x.Codigo_Estudiante == val)
                    .Select(d => d.Id_Estudiante).ToArray();
                var idEstudiante = idEstudianteArray[0];

                var idEstudianteExiste = db.Deudores.Where(x => x.Id_Estudiante == idEstudiante).Any();
                if (idEstudianteExiste)
                {
                    Debug.WriteLine("ESTUDIANTE EXISTE");
                    var idDeudorArray = db.Deudores.Where(x => x.Id_Estudiante == idEstudiante).Select(d => d.Id_Deudor)
                        .ToArray();
                    var idDeudor = idDeudorArray[0];
                    var idConceptoExiste = db.DetalleDeudor.Where(x => x.IdDeudor == idDeudor && x.IdConceptop == idConceptoDeudor).Any();

                    if (idConceptoExiste)
                    {
                        Debug.WriteLine("CONCEPTO EXISTE");
                        esDeudor = true;
                        var montoAbonadoPrevio = db.DetalleDeudor.Where(x => x.IdDeudor == idDeudor && x.IdConceptop == idConceptoDeudor).Select(x => x.Total_Pagado).First();
                        var totalDeberDeudor = montoDeudor - (pagoAbonadoDeudor + montoAbonadoPrevio);
                        var mytab = db.DetalleDeudor.First(g =>
                            g.IdDeudor == idDeudor && g.IdConceptop == idConceptoDeudor);
                        mytab.Total_Pagado = (pagoAbonadoDeudor + montoAbonadoPrevio);

                        mytab.Total_Deber = totalDeberDeudor;
                        db.SaveChanges();






                        using (db)
                        {
                            Factura nuevaFactura = new Factura();
                            nuevaFactura.Id = trueIDPAGO;
                            nuevaFactura.FechaFactura = DateTime.Now;
                            nuevaFactura.IdCaja = cajaINT;
                            nuevaFactura.IdCajero = cajeroTrue;
                            nuevaFactura.IdEstudiante = estudiante;
                            nuevaFactura.PagoAbonado = pagoAbonadoDeudor;
                            nuevaFactura.IdMoneda = moneda;
                            nuevaFactura.IdEquivalencia = equivalencia;
                            nuevaFactura.IdConcepto = idConceptoDeudor;
                            nuevaFactura.Id_Tipo = tipoPago;
                            db.Facturas.Add(nuevaFactura);
                            db.SaveChanges();



                        }




                    }

                    ;
                }


                ;
                using (db)
                {
                    Factura nuevaFactura = new Factura();
                    nuevaFactura.Id = trueIDPAGO;
                    nuevaFactura.FechaFactura = DateTime.Now;
                    nuevaFactura.IdCaja = cajaINT;
                    nuevaFactura.IdCajero = cajeroTrue;
                    nuevaFactura.IdEstudiante = estudiante;
                    nuevaFactura.PagoAbonado = pagoAbonadoDeudor;
                    nuevaFactura.IdMoneda = moneda;
                    nuevaFactura.IdEquivalencia = equivalencia;
                    nuevaFactura.IdConcepto = idConceptoDeudor;
                    nuevaFactura.Id_Tipo = tipoPago;
                    db.Facturas.Add(nuevaFactura);
                    db.SaveChanges();
                }

                return RedirectToAction("PrintIndex", "Facturas", new { id = trueIDPAGO });
            }

            ViewBag.IdPago = new SelectList(db.Pago, "Id_Pago", "Id_Pago");
            ViewBag.IdConceptoPago = new SelectList(db.ConceptosPago, "Id_Concepto", "NombreConcepto");
            ViewBag.IdTipo = new SelectList(db.TipoPago, "Id_TipoPago", "Nombre");
            ViewBag.IdMoneda = new SelectList(db.Moneda, "Id_Moneda", "Nombre");
            ViewBag.IdEquivalencia = new SelectList(db.Equivalencia, "Id_Equivalencia", "Monto");
            ViewBag.IdEstado = new SelectList(db.Estado, "Id_Estado", "Nombre");


            return RedirectToAction("PrintIndex", "Facturas", new { id = trueIDPAGO});
        }









        [HttpPost]
        public JsonResult MostrarMonto(int conceptoP)
        {
            

            
            var MontoG = db.ConceptosPago.Where(x => x.Id_Concepto == conceptoP).Select(d => d.Monto).ToArray();
            var montoG2 = MontoG[0];

            Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHH");
            Debug.WriteLine(montoG2);

            return Json(montoG2);
        }


        [HttpPost]
        public JsonResult ObtenerEquivalencia(int monedaP, decimal pagoAbonadoP)
        {
            var equivalencia = pagoAbonadoP;
            if (monedaP.Equals(2))
            {
                var fechaHoy = DateTime.Today;

                var equivalenciaRateA = db.Equivalencia.Where(x => x.Fecha == fechaHoy).Select(d => d.Monto).ToArray();
                var equivalenciaRate = equivalenciaRateA[0];
                equivalencia = equivalenciaRate * pagoAbonadoP;
            }

            return Json(equivalencia);
        }



        [HttpPost]
        public JsonResult ObtenerTotal(decimal efectivoP, decimal pagoAbonadoP)
        {
            var totalP = efectivoP + pagoAbonadoP;


            return Json(totalP);
        }



        [HttpPost]
        public JsonResult ObtenerCambio(decimal efectivoP, decimal pagoAbonadoP)
        {
            var equivalencia = efectivoP - pagoAbonadoP;


            return Json(equivalencia);
        }


        [HttpPost]
        public JsonResult ObtenerMora(int conceptoP, decimal montoP)
        {
            decimal resultado = 0;
            var fechaConcepto = db.ConceptosPago.Where(x => x.Id_Concepto == conceptoP).Select(d => d.fechaMora)
                .ToArray();
            var fechaCaducar = fechaConcepto[0];

            if (DateTime.Today > fechaCaducar)
            {
                resultado = 0;
                resultado = decimal.Multiply(montoP, 10) / 100;
            }


            var idEstudianteArray = db.tbestudiantes.Where(x => x.Codigo_Estudiante == val).Select(d => d.Id_Estudiante)
                .ToArray();
            var idEstudiante = idEstudianteArray[0];

            var idEstudianteExiste = db.Deudores.Where(x => x.Id_Estudiante == idEstudiante).Any();
            if (idEstudianteExiste)
            {
                Debug.WriteLine("ESTUDIANTE EXISTE EN MORA");
                var idDeudorArray = db.Deudores.Where(x => x.Id_Estudiante == idEstudiante).Select(d => d.Id_Deudor)
                    .ToArray();
                var idDeudor = idDeudorArray[0];
                var idConceptoExiste = db.DetalleDeudor.Where(x => x.IdDeudor == idDeudor && x.IdConceptop == conceptoP)
                    .Any();

                if (idConceptoExiste)
                {
                    Debug.WriteLine("CONCEPTO EXISTE EN MORA");
                    resultado = 0;
                }
            }

            return Json(resultado);
        }


        [HttpPost]
        public JsonResult MostrarBalance(decimal montoP, decimal pagoAbonadoP)
        {
            var balance = montoP - pagoAbonadoP;
            Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
            Debug.WriteLine(balance);


            return Json(balance);
        }


        public JsonResult GetCajeroDrop(string conceptoP)
        {
            var DropCajeros = conceptoP;
            Session["cajero"] = DropCajeros;
            Debug.WriteLine("2222222222222222222222222222222222222222");
            Debug.WriteLine(DropCajeros);


            return Json(conceptoP);
        }

        [HttpPost]
        public JsonResult getCajaDrop(string conceptoP)
        {
            var DropCajas = conceptoP;
            //Session["caja"] = DropCajeros;

            return Json(conceptoP);
        }


        // GET: Pago/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pago = await db.Pago.FindAsync(id);
            if (pago == null) return HttpNotFound();
            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", pago.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "Nombre", pago.IdCajero);
            return View(pago);
        }

        // POST: Pago/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id_Pago,FechaTransaccion,IdCajero,IdCaja,IdEstudiante")]
            Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdCaja = new SelectList(db.Caja, "IdCaja", "NumCaja", pago.IdCaja);
            ViewBag.IdCajero = new SelectList(db.Cajero, "IdCajero", "Nombre", pago.IdCajero);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var pago = await db.Pago.FindAsync(id);
            if (pago == null) return HttpNotFound();
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var pago = await db.Pago.FindAsync(id);
            db.Pago.Remove(pago);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}