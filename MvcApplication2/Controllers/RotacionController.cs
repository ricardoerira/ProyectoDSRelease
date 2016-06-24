using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace MvcApplication2.Controllers
{
    public class RotacionController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Rotacion/

        public ActionResult Index()
        {

            var rotacions = db.Rotacions.Include(r => r.ActividadAcademica).Include(r => r.IPS_ESE);
            return View(rotacions.ToList());
        }

        public ActionResult SeleccionRotacion()
        {
            List<Rotacion> listest=new List<Rotacion>();
            if (User.Identity.IsAuthenticated)
            {

                listest = db.Rotacions.Where(r => r.ActividadAcademica.DepartamentoSalud.user.Equals(User.Identity.Name)).ToList();

            }
            if (listest.Count == 0)
            {
                listest = db.Rotacions.ToList();
            }




            return View(db.Rotacions.ToList());
        }
        public ActionResult SeleccionRotacionCarta()
        {

            return View(db.Rotacions.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConsultaRotacion(int id = 0)
        {
            return RedirectToAction("Estudiante/RotacionEstudiante/" + id);
        }
        public ActionResult ConsultaRotacion()
        {
            return View(db.Rotacions.ToList());
        }
        public ActionResult ConsultaEstudiantes(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            return View(rotacion);
        }
        public ActionResult VistaODS()
        {
            bool estado = User.IsInRole("DocenciaServicio");
            if (!estado)
            {
                return RedirectToAction("../Account/Login");
            }
            else
            {
                return View();


            }

        }

        public DataTable ToDataTable<T>(T entity) where T : class
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            table.Rows.Add(properties.Select(p => p.GetValue(entity, null)).ToArray());
            return table;
        }
        public ActionResult GeneraReporte(int id = 0)
        {




            Rotacion rotacion = db.Rotacions.Find(id);

            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporte.rpt");
            rptH.Load(strRptPath);


            rptH.Database.Tables[0].SetDataSource(db.Estudiantes.ToList());
            rptH.Database.Tables[1].SetDataSource(db.HojaVidas.ToList());
            rptH.Database.Tables[2].SetDataSource(db.Rotacions.ToList());

            rptH.SetParameterValue("Nombre_Doctor", rotacion.IPS_ESE.representante_legal);
            rptH.SetParameterValue("cargo", rotacion.IPS_ESE.cargo_representanteDS);
            rptH.SetParameterValue("clinica", rotacion.IPS_ESE.nombre);
            //rptH.SetParameterValue("presentacion", "A continuación le relaciono las rotaciones de los estudiantes del Programa de " + rotacion.Estudiante.ElementAt(0).Programa.nombre + ", que realizaran su rotación en su institución y los profesores con su horario.");
            //rptH.SetParameterValue("docente", rotacion.Docente.ElementAt(0).HojaVida.primer_nombre + " " + rotacion.Docente.ElementAt(0).HojaVida.primer_apellido + " " + rotacion.Docente.ElementAt(0).HojaVida.segundo_apellido);
            rptH.SetParameterValue("fecha", rotacion.fecha_inicio);






            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeleccionRotacion(string searchString, int id = 0)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var rotaciones = db.Rotacions.Where(r => r.ActividadAcademica.nombre.ToUpper().Contains(searchString.ToUpper())
                    || r.ActividadAcademica.DepartamentoSalud.nombre.ToUpper().Contains(searchString.ToUpper()));
                List<Rotacion> listest = rotaciones.ToList();

                return View(rotaciones.ToList());
            }
            else
            {
                if (id > 0)
                {

                }
                return View(db.Rotacions.ToList());
            }


        }
        //
        // GET: /Rotacion/Details/5

        public ActionResult Details(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            return View(rotacion);
        }

        //
        // GET: /Rotacion/Create




        public ActionResult Create()
        {
            ViewBag.actividadacademicaId = new SelectList(db.ActividadAcademicas, "actividadacademicaId", "nombre");
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre");
            return View();
        }

        //
        // POST: /Rotacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rotacion rotacion)
        {
            if (ModelState.IsValid)
            {
                db.Rotacions.Add(rotacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.actividadacademicaId = new SelectList(db.ActividadAcademicas, "actividadacademicaId", "tipo", rotacion.actividadacademicaId);
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "origen", rotacion.IPS_ESEId);
            return View(rotacion);
        }





        //
        // GET: /Rotacion/Edit/5


        //
        // GET: /Rotacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IPS_ESEId = new SelectList(db.IPS_ESE, "IPS_ESEId", "nombre", rotacion.IPS_ESEId);
            ViewBag.actividadacademicaId = new SelectList(db.ActividadAcademicas, "actividadacademicaId", "nombre", rotacion.actividadacademicaId);

            return View(rotacion);
        }




        //
        // POST: /Rotacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rotacion rotacion)
        {
            if (ModelState.IsValid)
            {
                Rotacion rotacionE = db.Rotacions.Find(rotacion.rotacionId);
                rotacionE.fecha_inicio = rotacion.fecha_inicio;
                rotacionE.fecha_terminacion = rotacion.fecha_terminacion;
                rotacionE.observaciones = rotacion.observaciones;
                rotacionE.servicio = rotacion.servicio;

                db.Entry(rotacionE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SeleccionRotacion");
            }
            return View(rotacion);
        }


        //
        // GET: /Rotacion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            if (rotacion == null)
            {
                return HttpNotFound();
            }
            return View(rotacion);
        }



        //
        // POST: /Rotacion/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rotacion rotacion = db.Rotacions.Find(id);
            db.Rotacions.Remove(rotacion);
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