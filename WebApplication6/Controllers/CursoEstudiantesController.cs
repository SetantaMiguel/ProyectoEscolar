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
using WebApplication6.viewModels;

namespace WebApplication6.Controllers
{   [Authorize]
    public class CursoEstudiantesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private static int idaux = 0;
        private static int idaux2 = 0;
        // GET: CursoEstudiantes
        public async Task<ActionResult> Index(int id=0)
        {
            if (id!=0)
            {
                idaux = id;
                var cursoEstudiantes = db.CursoEstudiantes.Include(c => c.CursoEscolar).Include(c => c.Estudiantes).Where(x => x.Id_Estudiante == id);
                return View(await cursoEstudiantes.ToListAsync());
            }
            return View();
        }
        public ActionResult IndexCurso(int IdCurso=0 ,int pagina = 1)
        {
            idaux2 = IdCurso;
            var cantidadRegistrosPorPagina = 12;
            var cursoEstudiantes1 = db.CursoEstudiantes.Include(c => c.CursoEscolar).Include(c => c.Estudiantes).Where(x=>x.Id_Curso==IdCurso).OrderBy(x => x.Id)
            .Skip((pagina - 1) * cantidadRegistrosPorPagina)
            .Take(cantidadRegistrosPorPagina).ToList();

            var totalDeRegistros2 = db.CursoEstudiantes.Where(x => x.Id_Curso == IdCurso).Count();
            var modelo2 = new IndexViewModel2();
            modelo2.CursoEstudiantes = cursoEstudiantes1;
            modelo2.PaginaActual = pagina;
            modelo2.TotalRegistros = totalDeRegistros2;
            modelo2.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo2.Controller = idaux2;
            return View("IndexCurso", modelo2);

        }
            // GET: CursoEstudiantes/Details/5
            public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoEstudiantes cursoEstudiantes = await db.CursoEstudiantes.FindAsync(id);
            if (cursoEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(cursoEstudiantes);
        }

        // GET: CursoEstudiantes/Create
        public ActionResult Create()
        {
            int idaño = db.tbestudiantes.Find(idaux).Id;
            var Cursos = db.tbCursoEscolar.Where(x => x.Id_Año == idaño).ToList();
    
            ViewBag.Id_Curso = new SelectList(Cursos, "Id_Curso", "NombredeCurso");
            if (Cursos.Count==0)
            {
                ViewBag.NoCurso = true;
            }
            return View();
        }

        // POST: CursoEstudiantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Id_Curso,Id_Estudiante")] CursoEstudiantes cursoEstudiantes)
        {
            if (ModelState.IsValid)
            {
                //busco al curso escolar
                var aux = db.tbCursoEscolar.Find(cursoEstudiantes.Id_Curso);
                //busco la seccion
                Secciones seccion= db.Secciones.Find(aux.Id_Seccion);
                //aumento los estudiantes en la seccion
                seccion.EstudiantesAula += 1;
                //verifico si todavia se puede inscribir al estudiante sin violar el maximo de la seccion
                if (seccion.EstudiantesAula<=seccion.Max_Estudiantes)
                {
                    //Cambio a las seccion
                    db.Entry(seccion).State = EntityState.Modified;
                    //asigno al estudiante
                    cursoEstudiantes.Id_Estudiante = idaux;
                    //agrego al estudiante al curso y guardo al curso
                    db.CursoEstudiantes.Add(cursoEstudiantes);
                    await db.SaveChangesAsync();
                    //busco sus materias
                    var materias = db.tbCursoAsignaturas.Where(x => x.Id_Curso == cursoEstudiantes.Id_Curso);
           
                    foreach (var item in materias)
                    {
                        Notas notas = new Notas()
                        {
                            Id_Estudiante = cursoEstudiantes.Id_Estudiante,
                            Id_Curso = item.Id_Curso_Asignatura,
                            Nota1 = 0,
                            Nota2 = 0,
                            Nota3 = 0,
                            Nota4 = 0,
                            promedio = 0,
                            Bl_Aprobado = false
                        };
                        //guardo sus materias
                        db.Notas.Add(notas);
                    }
                    //guardo la bd
                    await db.SaveChangesAsync();

                    return RedirectToAction("Details", "Estudiantes", new { id = idaux });
                }
                else
                {

                    ViewBag.ErrorPorLLeno = true;
                }

            }

            ViewBag.Id_Curso = new SelectList(db.tbCursoEscolar, "Id_Curso", "Str_Modalidad", cursoEstudiantes.Id_Curso);
            return View(cursoEstudiantes);
        }
        // GET: CursoEstudiantes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CursoEstudiantes cursoEstudiantes = await db.CursoEstudiantes.FindAsync(id);
            if (cursoEstudiantes == null)
            {
                return HttpNotFound();
            }
            return View(cursoEstudiantes);
        }

        // POST: CursoEstudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //busco al curso del estuante, luego al curso completo y a la seccion
            CursoEstudiantes cursoEstudiantes = await db.CursoEstudiantes.FindAsync(id);
            var aux = db.tbCursoEscolar.Find(cursoEstudiantes.Id_Curso);
            Secciones seccion = db.Secciones.Find(aux.Id_Seccion);
            Curso_Asignaturas curso_Asignaturas = db.tbCursoAsignaturas.Where(x=>x.Id_Curso==cursoEstudiantes.Id_Curso).FirstOrDefault();
            //disminuyo la seccion y guardo el cambio
            seccion.EstudiantesAula -= 1;
            db.Entry(seccion).State = EntityState.Modified;
            //busco notas conforme a las materias del curso
            var notas = db.Notas.Where(x=>x.Id_Estudiante==cursoEstudiantes.Id_Estudiante).ToList();
            if (notas.Count!=0)
            {
                foreach (var item in notas)
                {
                    db.Notas.Remove(item);
                }
            }

            db.CursoEstudiantes.Remove(cursoEstudiantes);
            await db.SaveChangesAsync();
            if (idaux2!=null&&idaux2!=0)
            {
                return RedirectToAction("IndexCurso", "CursoEstudiantes", new { id = idaux,IdCurso=idaux2 });
            }
            return RedirectToAction("Details", "Estudiantes", new { id = idaux });
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
