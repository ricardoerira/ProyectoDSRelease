using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class RotacionEstudianteController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /RotacionEstudiante/

        public ActionResult Index(int id = 0)
        {

            var rotacionestudiantes = db.RotacionEstudiantes.Where(r => r.rotacionId == id).Include(r => r.Rotacion);
            List<RotacionEstudiante> re = rotacionestudiantes.ToList();
            foreach(RotacionEstudiante rotacionEstudiante in re)
            {
                if (rotacionEstudiante.estadoSeleccionado)
                {
                    rotacionEstudiante.estadoSeleccionado = false;
                    db.Entry(rotacionEstudiante).State = EntityState.Modified;
                    db.SaveChanges();
               
                }
            }
            ViewBag.nombre = db.Rotacions.Find(id).ActividadAcademica.nombre;
            return View(rotacionestudiantes.ToList());
        }



        [HttpPost]
        public ActionResult Index(List<RotacionEstudiante> rotaciones, int id)
        {
            List<RotacionEstudiante> rotacionestudiantes = db.RotacionEstudiantes.Where(r => r.rotacionId == id).Include(r => r.Rotacion).ToList();

            int i = 0;
            int cont = 0;
            foreach (RotacionEstudiante item in rotacionestudiantes)
            {

                if (rotaciones.ElementAt(i).estadoSeleccionado)
                {

                    item.estadoSeleccionado = rotaciones.ElementAt(i).estadoSeleccionado;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    cont = 1;
                }
                
                i++;
            }
            if (cont == 1)
            {
            rotacionestudiantes = rotacionestudiantes.Where(r => r.estadoSeleccionado == true).ToList();
         


                ViewBag.nombre = db.Rotacions.Find(id).ActividadAcademica.nombre;
                if (Request.Form["submitbutton1"] != null)
                {
                    return RedirectToAction("EditDocente/" + rotacionestudiantes.ElementAt(0).rotacionEstudianteId);
                }
                else if (Request.Form["submitButton2"] != null)
                {
                    return RedirectToAction("Edit/" + rotacionestudiantes.ElementAt(0).rotacionEstudianteId);
                }
            }
            else
            {
                ViewBag.alert = "Debes seleccionar al menos un item";
                return View(rotacionestudiantes.ToList());
            }

            return View(rotacionestudiantes.ToList());
        }
        //
        // GET: /RotacionEstudiante/Details/5

        public ActionResult Details(int id = 0)
        {
            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            if (rotacionestudiante == null)
            {
                return HttpNotFound();
            }
            return View(rotacionestudiante);
        }

        //
        // GET: /RotacionEstudiante/Create

        public ActionResult Create(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            ViewBag.rotacionId = id;
            ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "tipo_documento");
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo_documento");
            return View();
        }

        //
        // POST: /RotacionEstudiante/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RotacionEstudiante rotacionestudiante)
        {
            if (ModelState.IsValid)
            {
                db.RotacionEstudiantes.Add(rotacionestudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", rotacionestudiante.IPS_ESEId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "grupo", rotacionestudiante.rotacionId);
            //    ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "tipo_documento", rotacionestudiante.docenteId);
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo_documento", rotacionestudiante.estudianteId);
            return View(rotacionestudiante);
        }

        //
        // GET: /RotacionEstudiante/Edit/5

        public ActionResult Edit(int id = 0)
        {

            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            List<RotacionDocente> rotacionDocentes = db.RotacionDocentes.Where(r => r.rotacionEstudianteId == rotacionestudiante.rotacionEstudianteId).ToList();

            if (rotacionestudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.rotacionEstudianteId = rotacionestudiante.rotacionEstudianteId;
            List<IPS_ESE> ips=db.IPS_ESE.ToList();

            ips = ips.OrderBy(x => x.nombre)
           .ToList();
            ViewBag.IPS_ESEId = new SelectList(ips, "IPS_ESEId", "nombre", rotacionestudiante.IPS_ESEId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "grupo", rotacionestudiante.rotacionId);
            ViewBag.docenteId = new SelectList(rotacionDocentes, "docenteId", "Docente.HojaVida.primer_nombre", rotacionDocentes.ElementAt(0).docenteId);
            ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo", rotacionestudiante.estudianteId);
            return View(rotacionestudiante);
        }


        public ActionResult EditDocente(int id)
        {

            RotacionEstudiante rotacionestudiante = db.RotacionEstudiantes.Find(id);
            List<RotacionDocente> rotacionDocentes = db.RotacionDocentes.Where(r => r.rotacionEstudianteId == rotacionestudiante.rotacionEstudianteId).ToList();
            List<Docente> docentes = db.Docentes.Include(r => r.HojaVida).ToList();
            docentes = docentes.OrderBy(x => x.HojaVida.primer_nombre)
          .ToList();
            if (rotacionestudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.rotacionEstudianteId = rotacionestudiante.rotacionEstudianteId;
            ViewBag.docenteId = new SelectList(docentes, "docenteId", "HojaVida.primer_nombre", docentes.ElementAt(0).docenteId);
            return View(rotacionDocentes.ToList());
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDocente(RotacionDocente rotacionDocente, FormCollection value, int id)
        {

            if (ModelState.IsValid)
            {
                List<RotacionEstudiante> rotacionestudiantes = db.RotacionEstudiantes.Where(r => r.Rotacion.rotacionId == rotacionDocente.Rotacion.rotacionId).Include(r => r.Rotacion).ToList();
                foreach (RotacionEstudiante item in rotacionestudiantes)
                {
                    if (item.estadoSeleccionado)
                    {
                        rotacionDocente.rotacionEstudianteId = item.rotacionEstudianteId;
                        rotacionDocente.Rotacion= null;
                        int docenteId = Int32.Parse(value["docenteId"]);
                        Docente docente = db.Docentes.Find(docenteId);
                        rotacionDocente.nombre = docente.HojaVida.primer_nombre;
                       

                        db.RotacionDocentes.Add(rotacionDocente);
                        db.SaveChanges();
                    }

                }


                return RedirectToAction("EditDocente/" + rotacionDocente.rotacionEstudianteId);

            }



            return RedirectToAction("EditDocente/" + rotacionDocente.rotacionEstudianteId);

        }


        //
        // POST: /RotacionEstudiante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RotacionEstudiante rotacionestudiante)
        {
            if (ModelState.IsValid)
            {
                Rotacion rotacion = db.Rotacions.Find(rotacionestudiante.rotacionId);
                List<RotacionEstudiante> rotacionestudiantes = db.RotacionEstudiantes.Where(r => r.Rotacion.rotacionId == rotacion.rotacionId).Include(r => r.Rotacion).ToList();
                    foreach (RotacionEstudiante item in rotacionestudiantes)
                    {
                        if (item.estadoSeleccionado)
                        {
                            item.horario = rotacionestudiante.horario;
                            item.IPS_ESEId = rotacionestudiante.IPS_ESEId;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }





             
                return RedirectToAction("Index/" + rotacionestudiante.rotacionId);
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", rotacionestudiante.IPS_ESEId);
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "grupo", rotacionestudiante.rotacionId);
            // ViewBag.docenteId = new SelectList(db.Docentes, "docenteId", "HojaVida.primer_nombre", rotacionestudiante.docenteId);
            //    ViewBag.estudianteId = new SelectList(db.Estudiantes, "estudianteId", "tipo_documento", rotacionestudiante.estudianteId);
            return View(rotacionestudiante);
        }

        //
        // GET: /RotacionEstudiante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RotacionDocente rotacionDocente = db.RotacionDocentes.Find(id);
            if (rotacionDocente == null)
            {
                return HttpNotFound();
            }
            List<RotacionEstudiante> rotacionestudiantes = db.RotacionEstudiantes.Where(r => r.Rotacion.rotacionId == rotacionDocente.Rotacion.rotacionId).Include(r => r.Rotacion).ToList();

            foreach (RotacionEstudiante item in rotacionestudiantes)
            {
                if (item.estadoSeleccionado)
                {
                    List<RotacionDocente> rd = db.RotacionDocentes.Where(r => r.rotacionEstudianteId == item.rotacionEstudianteId).Where(r => r.docenteId == rotacionDocente.docenteId).ToList();
                 if(rd.Count>0)
                 {
                     RotacionDocente rotacionDocenteAux = rd.ElementAt(0);
                     db.RotacionDocentes.Remove(rotacionDocenteAux);
                     db.SaveChanges();
                 }
                    

                }
            }
                      
          
            return RedirectToAction("EditDocente/" + rotacionDocente.rotacionEstudianteId);

        }

        //
        // POST: /RotacionEstudiante/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RotacionDocente rotacionDocente = db.RotacionDocentes.Find(id);

            db.RotacionDocentes.Remove(rotacionDocente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}