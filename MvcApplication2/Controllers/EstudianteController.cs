﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Data.Entity.Validation;
using MvcApplication2.Util;

namespace MvcApplication2.Controllers
{
    public class EstudianteController : Controller
    {
        private UsersContext2 db = new UsersContext2();


        public ActionResult BuscarEnVacuna(Estudiante estudiante)
        {
            var estudiantes = from b in db.Estudiantes
                              select b;

            foreach (var b in estudiantes)
            {
                if (b.num_documento.Equals(estudiante.num_documento) || b.codigo.Equals(estudiante.codigo))
                {
                    estudiante = b;
                }

            }
            if (estudiante.estudianteId == 0)
            {
                return View(estudiante);
            }
            else
            {
                return RedirectToAction("../Estudiante/CarnetVacunacionDS/" + estudiante.estudianteId);
            }
        }


        public ActionResult BuscarEnDepartamento()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarEnDepartamento(Estudiante estudiante)
        {
            var estudiantes = from b in db.Estudiantes
                              select b;

            foreach (var b in estudiantes)
            {
                if (b.codigo.Equals(estudiante.codigo))
                {
                    estudiante = b;
                }

            }
            if (estudiante.estudianteId == 0)
            {
                ViewBag.AlertMessage = "Codigo incorrecto.";
                return View();
            }
            else
            {
                //return RedirectToAction("../Estudiante/ReporteEstudianteA/"  + estudiante.estudianteId);
                return RedirectToAction("../Estudiante/PersonalesDpto/" + estudiante.estudianteId);


            }
        }



        public ActionResult EstadoHVdepto(string num_documento, string programaId, string estado_HV)
        {
            int did = 0;
            bool b = false;
            if (!String.IsNullOrEmpty(programaId))
            {
                did = Int32.Parse(programaId);
                b = estado_HV.Equals("True") ? true : false;

            }

            var estudiantes = new List<Estudiante>();
            var estudiantesaux = new List<Estudiante>();
            if (!String.IsNullOrEmpty(num_documento))
            {
                estudiantes = db.Estudiantes.Where(s => s.num_documento.Equals(num_documento)).Where(s => s.programaId == did).Where(s => s.HojaVida.estado_HV == b).ToList();

            }
            else
            {
                if (!String.IsNullOrEmpty(programaId))
                {
                    estudiantes = db.Estudiantes.Where(s => s.programaId == did).Where(s => s.HojaVida.estado_HV == b).ToList();
                    estudiantesaux = db.Estudiantes.Where(s => s.programaId == did).ToList();

                }
            }






            ViewBag.busqueda = estudiantes.Count() + " / " + estudiantesaux.Count();


            ViewBag.Programas = new SelectList(db.Programas, "programaId", "nombre");

            return View(estudiantes.ToList());
        }

        public ActionResult EstadoHV(string num_documento, string programaId, string estado_HV)
        {
            int did = 0;
            bool b = false;
            if (!String.IsNullOrEmpty(programaId))
            {
                did = Int32.Parse(programaId);
                b = estado_HV.Equals("True") ? true : false;

            }

            var estudiantes = new List<Estudiante>();
            var estudiantesaux = new List<Estudiante>();
            if (!String.IsNullOrEmpty(num_documento))
            {
                estudiantes = db.Estudiantes.Where(s => s.num_documento.Equals(num_documento)).Where(s => s.programaId == did).Where(s => s.HojaVida.estado_HV == b).ToList();

            }
            else
            {
                if (!String.IsNullOrEmpty(programaId))
                {
                    estudiantes = db.Estudiantes.Where(s => s.programaId == did).Where(s => s.HojaVida.estado_HV == b).ToList();
                    estudiantesaux = db.Estudiantes.Where(s => s.programaId == did).ToList();

                }
            }






            ViewBag.busqueda = estudiantes.Count() + " / " + estudiantesaux.Count();


            ViewBag.Programas = new SelectList(db.Programas, "programaId", "nombre");

            return View(estudiantes.ToList());
        }
        //
        // GET: /Estudiante/
        //public ActionResult Buscar(Estudiante estudiante)
        //{
        //    var estudiantes = from b in db.Estudiantes
        //                      select b;

        //    foreach (var b in estudiantes)
        //    {
        //        if (b.num_documento.Equals(estudiante.num_documento) || b.codigo.Equals(estudiante.codigo))
        //        {
        //            estudiante = b;
        //        }

        //    }
        //    if (estudiante.estudianteId == 0)
        //    {
        //        return View(estudiante);
        //    }
        //    else
        //    {
        //        return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);
        //    }


        //public ActionResult Buscar(Estudiante estudiante)
        //{
        //    var estudiantes = from b in db.Estudiantes
        //                      select b;

        //    foreach (var b in estudiantes)
        //    {
        //        if (b.codigo.Equals(estudiante.codigo))
        //        {
        //            estudiante = b;
        //        }

        //    }
        //    if (estudiante.estudianteId == 0)
        //    {
        //        ViewBag.AlertMessage = "Código incorrecto.";
        //        return View(estudiante);
        //    }
        //    //else
        //    //{
        //    //    return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);
        //    //}

        //    else
        //    {
        //        Estudiante e1 = estudiante;
        //        int id = e1.programaId;
        //        ViewBag.AlertMessage = null;

        //        if (id != 33 && id != 34 && id != 35 && id != 55 && id != 56 && id != 57 && id != 58 && id != 59 && id != 60 && id != 61)
        //        {
        //            return RedirectToAction("../Estudiante/PersonalesResidentesDS/" + e1.estudianteId);
        //        }
        //        else
        //        {
        //            return RedirectToAction("../Estudiante/PersonalesDS/" + e1.estudianteId);

        //        }

        //    }
        //}

        public ActionResult Buscar()
        {

            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Buscar(Estudiante estudiante)
        {
            var estudiantes = from b in db.Estudiantes
                              select b;

            foreach (var b in estudiantes)
            {
                if (b.codigo.Equals(estudiante.codigo))
                {
                    estudiante = b;
                }

            }



            if (estudiante.estudianteId != 0)
            {
                Estudiante e1 = estudiante;
                int id = e1.programaId;
                ViewBag.AlertMessage = null;

                if (id != 33 && id != 34 && id != 35 && id != 55 && id != 56 && id != 57 && id != 58 && id != 59 && id != 60 && id != 61)
                {
                    return RedirectToAction("../Estudiante/PersonalesResidentesDS/" + e1.estudianteId);
                }
                else
                {
                    return RedirectToAction("../Estudiante/PersonalesDS/" + e1.estudianteId);

                }
            }

            else
            {
                ViewBag.AlertMessage = "Codigo incorrecto.";
                return View();
            }
        }



        public ActionResult Soporte(int id = 0)
        {
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%", "/");

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }


            ViewBag.imagen1 = imagen;


            return View(estudiante);
        }

        public ActionResult SoporteDS(int id = 0)
        {
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad" };



            ViewBag.imagen1 = imagen;


            return View(estudiante);
        }


        public ActionResult SoporteResidente(int id = 0)
        {
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad" };



            ViewBag.imagen1 = imagen;


            return View(estudiante);
        }

        public ActionResult SoporteResidenteDS(int id = 0)
        {
            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%2F", "/");

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            string[] documentos = { "doc_identidad" };



            ViewBag.imagen1 = imagen;


            return View(estudiante);
        }

        public Boolean ValidarVacunas(Estudiante estudiante)
        {
            for (var i = 0; i < estudiante.HojaVida.Vacunas.Count; i++)
            {

                if (estudiante.HojaVida.Vacunas.ElementAt(i).fecha_vacunacion < DateTime.Now.Date.AddMonths(-20))
                {
                    {



                        if (estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Hepatitis B Dosis 1") || estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Triple Viral Dosis 1") || estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Varicela Dosis 1") || estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Toxoide Tetánico Dosis 1")
                              || estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Fiebre Amarilla Dosis 1") || estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Influenza Dosis 1") || estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("DTP Acelular Dosis 1"))
                        {
                            return false;
                        }
                        else
                        {
                            if (estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Hepatitis B Dosis 2") && (estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion.AddMonths(1) < DateTime.Now.Date && estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion > DateTime.Now.Date.AddMonths(-20)))
                            {


                                return false;
                            }
                            else if (estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Toxoide Tetánico Dosis 2") && (estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion.AddMonths(1) < DateTime.Now.Date && estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion > DateTime.Now.Date.AddMonths(-20)))
                            {
                                return false;

                            }
                            else if (estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Toxoide Tetánico Dosis 3") && (estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion.AddMonths(6) < DateTime.Now.Date && estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion > DateTime.Now.Date.AddMonths(-20)))
                            {
                                return false;

                            }
                            else if (estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Toxoide Tetánico Dosis 4") && (estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion.AddMonths(12) < DateTime.Now.Date && estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion > DateTime.Now.Date.AddMonths(-20)))
                            {
                                return false;

                            }
                            else if (estudiante.HojaVida.Vacunas.ElementAt(i).nombre_generico.Equals("Toxoide Tetánico Dosis 5") && (estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion.AddMonths(12) < DateTime.Now.Date && estudiante.HojaVida.Vacunas.ElementAt(i - 1).fecha_vacunacion > DateTime.Now.Date.AddMonths(-20)))
                            {
                                return false;

                            }



                        }
                    }

                }





            }
            return true;


        }
        public Boolean validarCampos(Estudiante estudiante, bool ds)
        {
            HojaVida hv = db.HojaVidas.Find(estudiante.hojaVidaId);
            Estudiante e = db.Estudiantes.Find(estudiante.estudianteId);
            Familia f = db.Familias.Find(hv.familiaId);


            if ((hv.municipio_procedencia != null) && (hv.direccion_manizales != null) &&
                (hv.hemoclasificacion != "Sin Asignar") &&
                (hv.num_celular != 0) &&
                (hv.estado_civil != "Sin Asignar") && (f.primer_nombre_acudiente != null) &&
                (f.primer_apellido_acudiente != null) && (f.direccion_acudiente != null) &&
                (f.telefono_acudiente != 0))
            {
                Estudiante estudianteAux = db.Estudiantes.Find(estudiante.estudianteId);
                string[] documentos = null;
                if (ds)
                    documentos = Constantes.documentos_estudianteResidentes;
                else
                    documentos = Constantes.documentos_estudiante;


                if (validaDocumentos(estudiante, documentos))
                {
                    estudianteAux.HojaVida.estado_HV = true;


                }
                else
                {
                    estudianteAux.HojaVida.estado_HV = false;


                }
                db.SaveChanges();

                ViewBag.estado = estudianteAux.HojaVida.estado_HV;
                db.Entry(estudianteAux).State = EntityState.Modified;
                return true;
            }
            else
            {
                Estudiante estudianteAux = db.Estudiantes.Find(estudiante.estudianteId);
                estudianteAux.HojaVida.estado_HV = false;
                ViewBag.estado = estudianteAux.HojaVida.estado_HV;
                estudianteAux.HojaVida.municipio_procedencia = ".";
                estudianteAux.HojaVida.num_celular = 3000000000;
                db.Entry(estudianteAux).State = EntityState.Modified;

                try
                {

                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }


                return false;
            }
        }


        public ActionResult Index()
        {
            List<Estudiante> estudiantes = db.Estudiantes.Include(e => e.HojaVida).Include(e => e.Programa).ToList();
            foreach(Estudiante estudiante  in estudiantes)
            {
                validarCampos(estudiante, false);
            }
            return View(estudiantes.ToList());
        }
        public ActionResult RotacionEstudiante(string searchString, int id = 0)
        {

            string rotacion = Request.Params["rotacion"];
            if (rotacion == null)
            {
                ViewBag.id = id;
            }
            else
            {
                Estudiante estudiante = db.Estudiantes.Find(id);
                Rotacion rotacionE = db.Rotacions.Find(Convert.ToInt32(rotacion));
                rotacionE.numero_estudiantes = rotacionE.numero_estudiantes + 1;
                db.Entry(rotacionE).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();

            }
            var estudiantes = from s in db.Estudiantes
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int ss = Convert.ToInt32(searchString);
                estudiantes = estudiantes.Where(s => s.codigo == ss);
            }


            return View(estudiantes.ToList());
        }

        public ActionResult ReporteEstudiante(string searchString, int id = 0)
        {
            var estudiantes = from s in db.Estudiantes
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                int ss = Convert.ToInt32(searchString);
                estudiantes = estudiantes.Where(s => s.codigo == ss);
            }


            return View(estudiantes.ToList());
        }
        public ActionResult ReporteEstudianteA(int id = 0)
        {




            Estudiante estudiante = db.Estudiantes.Find(id);

            ReportDocument rptH = new ReportDocument();
            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/reporteEstudiante.rpt");
            rptH.Load(strRptPath);
            DateTime dt = DateTime.Now.Date.AddMonths(-20);
            var vacunas = db.Vacunas.Where(r => r.hojaVidaId == estudiante.hojaVidaId).Where(r => r.fecha_vacunacion > dt);
            List<Vacuna> listav = vacunas.ToList();

            rptH.Database.Tables[0].SetDataSource(listav);

            rptH.SetParameterValue("programa", estudiante.Programa.nombre);
            rptH.SetParameterValue("modalidad", estudiante.modalidad + "");
            rptH.SetParameterValue("semestre", estudiante.semestre + "");
            rptH.SetParameterValue("estadom", estudiante.estado_academico + "");
            rptH.SetParameterValue("tipodoc", estudiante.tipo_documento);
            rptH.SetParameterValue("numdoc", estudiante.num_documento);
            rptH.SetParameterValue("codigo", estudiante.codigo);
            rptH.SetParameterValue("genero", estudiante.HojaVida.genero + "");
            rptH.SetParameterValue("nombre", estudiante.HojaVida.primer_nombre + " " + estudiante.HojaVida.segundo_nombre);
            rptH.SetParameterValue("apellidos", estudiante.HojaVida.primer_apellido + " " + estudiante.HojaVida.segundo_apellido);
            rptH.SetParameterValue("fecha_nacimiento", estudiante.HojaVida.fecha_nacimiento + "");
            rptH.SetParameterValue("hemoclasificacion", estudiante.HojaVida.hemoclasificacion + "");
            rptH.SetParameterValue("dpto_procedencia", estudiante.HojaVida.departamento_procedencia + "");
            rptH.SetParameterValue("mun_procedencia", estudiante.HojaVida.municipio_procedencia + "");
            rptH.SetParameterValue("barrio_procedencia", estudiante.barrio_procedencia + "");
            rptH.SetParameterValue("dir_procedencia", estudiante.direccion_procedencia + "");
            rptH.SetParameterValue("dir_local", estudiante.HojaVida.direccion_manizales + "");
            rptH.SetParameterValue("image", estudiante.HojaVida.imagen_DI + "");
            rptH.SetParameterValue("edad", estudiante.barrio_procedencia + "");
            rptH.SetParameterValue("tel_proc", estudiante.HojaVida.num_telefono + "");
            rptH.SetParameterValue("tel_local", "");
            rptH.SetParameterValue("estado_civil", estudiante.HojaVida.estado_civil + "");
            rptH.SetParameterValue("num_hijos", estudiante.HojaVida.hijos + "");
            rptH.SetParameterValue("mail", estudiante.HojaVida.correo + "");
            rptH.SetParameterValue("num_cel", estudiante.HojaVida.num_celular + "");
            rptH.SetParameterValue("nombre_padre", estudiante.HojaVida.Familia.primer_nombre_padre + " " + estudiante.HojaVida.Familia.primer_apellido_padre + " " + estudiante.HojaVida.Familia.segundo_apellido_padre);
            rptH.SetParameterValue("direccion_padre", estudiante.HojaVida.Familia.direccion_padre + "");
            rptH.SetParameterValue("tel_padre", estudiante.HojaVida.Familia.telefono_padre + "");
            rptH.SetParameterValue("nombre_madre", estudiante.HojaVida.Familia.primer_nombre_madre + " " + estudiante.HojaVida.Familia.primer_apellido_madre + " " + estudiante.HojaVida.Familia.segundo_apellido_madre);
            rptH.SetParameterValue("direccion_madre", estudiante.HojaVida.Familia.direccion_madre + "");
            rptH.SetParameterValue("tel_madre", estudiante.HojaVida.Familia.telefono_madre + "");
            rptH.SetParameterValue("nombre_acudiente", estudiante.HojaVida.Familia.primer_nombre_acudiente + " " + estudiante.HojaVida.Familia.primer_apellido_acudiente + " " + estudiante.HojaVida.Familia.segundo_apellido_acudiente);
            rptH.SetParameterValue("direccion_acudiente", estudiante.HojaVida.Familia.direccion_acudiente + "");
            rptH.SetParameterValue("tel_acudiente", estudiante.HojaVida.Familia.telefono_acudiente + "");
            
            for (int i = 0; i < Constantes.documentos_estudiante.Length; i++)
            {
                string path1 = string.Format("{0}{1}{2}", "http://salud.ucaldas.edu.co/Proyecto/Uploads/" + Constantes.documentos_estudiante[i], +estudiante.codigo, ".jpg");
                rptH.SetParameterValue(Constantes.documentos_estudiante[i], path1);



            }


           

            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");

        }
        public ActionResult RotacionEstudiante2(string searchString, int id = 0)
        {

            string rotacion = Request.Params["rotacion"];
            if (rotacion == null)
            {
                ViewBag.id = id;
            }
            else
            {
                Estudiante estudiante = db.Estudiantes.Find(id);
                Rotacion rotacionE = db.Rotacions.Find(Convert.ToInt32(rotacion));
                rotacionE.numero_estudiantes = rotacionE.numero_estudiantes + 1;
                db.Entry(rotacionE).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();

            }


            return RedirectToAction("RotacionEstudiante/" + rotacion);
        }


        public ActionResult Details(int id = 0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }



        //
        // GET: /Estudiante/Create

        public ActionResult Create()
        {
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre");
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre");
            ViewBag.rotacionId = new SelectList(db.Rotacions, "rotacionId", "rotacionId");
            return View();
        }

        //
        // POST: /Estudiante/Create

        [HttpPost]


        [ValidateAntiForgeryToken]
        public ActionResult Create(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {

                db.Estudiantes.Add(estudiante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", estudiante.hojaVidaId);
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre", estudiante.programaId);
            return View(estudiante);
        }

        //
        // GET: /Estudiante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", estudiante.hojaVidaId);
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre", estudiante.programaId);
            return View(estudiante);
        }

        //
        // POST: /Estudiante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudiante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hojaVidaId = new SelectList(db.HojaVidas, "hojaVidaId", "primer_nombre", estudiante.hojaVidaId);
            ViewBag.programaId = new SelectList(db.Programas, "programaId", "nombre", estudiante.programaId);
            return View(estudiante);
        }

        //
        // GET: /Estudiante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }

        //
        // POST: /Estudiante/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estudiante estudiante = db.Estudiantes.Find(id);
            db.Estudiantes.Remove(estudiante);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        //
        //--------------------- METODOS PARA GESTIONAR LAS VISTAS DE LA-------------------
        //---------------------------- HOJA DE VIDA DE ESTUDIANTES-------------------


        //
        //--------------------- Vista para Logeo del estudiante




        public ActionResult Login()
        {

            return View();

        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(Estudiante estudiante) //PRIMER VERSION
        //{


        //    var b = db.Estudiantes.Where(s => s.codigo==estudiante.codigo).Where(s => s.clave==estudiante.clave);
        //    List<Estudiante> estudianteList = b.ToList();



        //    if (estudianteList.Count == 0)
        //    {
        //        ViewBag.AlertMessage = "El usuario y la contraseña que has introducido no coinciden.";
        //        return View(estudiante);
        //    }
        //    else 
        //    {
        //        ViewBag.AlertMessage = null;
        //        Estudiante docente_aux = estudianteList.ElementAt(0);

        //        return RedirectToAction("../Estudiante/Personales/" + docente_aux.estudianteId);

        //    }
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Estudiante estudiante)
        {
            var b = db.Estudiantes.Where(s => s.codigo == estudiante.codigo).Where(s => s.clave == estudiante.clave);
            List<Estudiante> estudianteList = b.ToList();

            if (estudianteList.Count == 0)
            {
                ViewBag.AlertMessage = "El usuario y la contraseña que has introducido no coinciden.";
                return View(estudiante);
            }

            else
            {
                Estudiante e1 = estudianteList.ElementAt(0);
                int id = e1.programaId;
                ViewBag.AlertMessage = null;

                if (id != 33 && id != 34 && id != 35 && id != 55 && id != 56 && id != 57 && id != 58 && id != 59 && id != 60 && id != 61)
                {
                    return RedirectToAction("../Estudiante/PersonalesResidentes/" + e1.estudianteId);
                }
                else
                {
                    return RedirectToAction("../Estudiante/Personales/" + e1.estudianteId);

                }

            }

        }




        public ActionResult LoginCC(int id = 0)
        {
            TempData["notice"] = null;
            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCC(Estudiante estudiante)
        {
            Estudiante estudianteReal = db.Estudiantes.Find(estudiante.estudianteId);
            if (estudiante.clave.Equals(estudianteReal.clave))
            {

                ViewBag.AlertMessage = null;
                return RedirectToAction("../Estudiante/CambioContraseña/" + estudianteReal.estudianteId);
            }
            else
            {

                ViewBag.AlertMessage = "La contraseña que has introducido no coincide.";
                return View(estudiante);
                // return RedirectToAction("../Estudiante/LoginCC/" + estudianteReal.estudianteId);

            }
        }

        public ActionResult PaginaPrincipal()
        {
            return View();

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoginCC(Estudiante estudiante)
        //{

        //    var b = db.Estudiantes.Where(s => s.num_documento.Equals(estudiante.num_documento)).Where(s => s.clave.Equals(estudiante.clave));
        //    List<Estudiante> estudianteList = b.ToList();


        //    if (estudianteList.Count == 0)
        //    {

        //        return RedirectToAction("../Estudiante/LoginCC/");
        //    }
        //    else
        //    {
        //        Estudiante estudiante_aux = estudianteList.ElementAt(0);

        //        return RedirectToAction("../Estudiante/CambioContraseña/" + estudiante_aux.estudianteId);
        //    }
        //}

        public ActionResult CambioContraseña(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambioContraseña(Estudiante estudiante)
        {
            Estudiante est = db.Estudiantes.Find(estudiante.estudianteId);
            if (estudiante.clave != null)
            {
                if (estudiante.clave.Equals(estudiante.tipo_documento))
                {
                    est.clave = estudiante.clave;


                    db.Entry(est).State = EntityState.Modified;


                    ViewBag.AlertMessage = null;
                    db.SaveChanges();
                    return RedirectToAction("../Estudiante/Personales/" + est.estudianteId);

                }
            }


            ViewBag.AlertMessage = "Las contrasenias no coinciden";
            return View(estudiante);
            //  return RedirectToAction("../Estudiante/CambioContraseña/" + estudiante.estudianteId);
        }





        //
        //------------------------- Vista para datos personales del estudiante
        public ActionResult cargaImagen(Estudiante estudiante)
        {

            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            try
            {
                var request = WebRequest.Create(oHojaVida.imagen_DI);
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        // Process the stream
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        oHojaVida.imagen_DI = "http://www.tagetik.com/intouch2015/user.png";
                    }
                    else
                    {
                        // Do something else
                    }
                }
                else
                {
                    // Do something else
                }
            }
            if (estudiante == null)
            {
                return HttpNotFound();
            } return View(estudiante);
        }

        public ActionResult Personales(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            cargaImagen(estudiante);

            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad
            validarCampos(estudiante,false);

            cargaDocumentos(estudiante);
            return View(estudiante);

        }

        public ActionResult PersonalesResidentes(int id = 0)
        {

            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            cargaImagen(estudiante);

            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad
            validarCampos(estudiante,true);

            cargaDocumentoResidentes(estudiante);
            return View(estudiante);

        }

        //metodo que muestra imagen
        public ActionResult cargaDocumentos(Estudiante estudiante)
        {
            {


                string path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[0] + estudiante.codigo, ".jpg");


                if (Utilidades.remoteFileExists(path1))
                {

                    ViewBag.imagen1 = path1;
                    ViewBag.imagen1a = Constantes.documentos_estudiante[0] + estudiante.codigo + ".jpg";
                }
                else
                {
                    ViewBag.imagen1 = Constantes.url_noimage;

                }




                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[1] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen2a = Constantes.documentos_estudiante[1] + estudiante.codigo + ".jpg";


                    ViewBag.imagen2 = path1;

                }
                else
                {
                    ViewBag.imagen2 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[2] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen3a = Constantes.documentos_estudiante[2] + estudiante.codigo + ".jpg";

                    ViewBag.imagen3 = path1;

                }
                else
                {
                    ViewBag.imagen3 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[3] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen4a = Constantes.documentos_estudiante[3] + estudiante.codigo + ".jpg";

                    ViewBag.imagen4 = path1;

                }
                else
                {
                    ViewBag.imagen4 = Constantes.url_noimage;

                }




                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[4] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen5a = Constantes.documentos_estudiante[4] + estudiante.codigo + ".jpg";

                    ViewBag.imagen5 = path1;

                }
                else
                {
                    ViewBag.imagen5 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[5] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen6a = Constantes.documentos_estudiante[5] + estudiante.codigo + ".jpg";

                    ViewBag.imagen6 = path1;

                }
                else
                {
                    ViewBag.imagen6 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[6] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen7a = Constantes.documentos_estudiante[6] + estudiante.codigo + ".jpg";

                    ViewBag.imagen7 = path1;

                }
                else
                {
                    ViewBag.imagen7 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[7] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen8a = Constantes.documentos_estudiante[7] + estudiante.codigo + ".jpg";

                    ViewBag.imagen8 = path1;

                }
                else
                {
                    ViewBag.imagen8 = Constantes.url_noimage;

                }

                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudiante[8] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    ViewBag.imagen9a = Constantes.documentos_estudiante[8] + estudiante.codigo + ".jpg";

                    ViewBag.imagen9 = path1;

                }
                else
                {
                    ViewBag.imagen9 = Constantes.url_noimage;

                }




                return View(estudiante);


            }
        }


        public ActionResult DeleteImage(int id)
        {

            string imagen = Request.Params["imagen"];
            imagen = imagen.Replace("%", "/");
            string path1 = string.Format("{0}{1}", Server.MapPath("../../Uploads/"), imagen);
            if (System.IO.File.Exists(path1))
                System.IO.File.Delete(path1);

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }


            ViewBag.imagen1 = imagen;
            return RedirectToAction("../Estudiante/Personales/" + estudiante.estudianteId);



        }

        public ActionResult 

            cargaDocumentoResidentes(Estudiante estudiante)
        {
            {


                string path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[0] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {

                    ViewBag.imagen1 = path1;
                    ViewBag.imagen1a = Constantes.documentos_estudianteResidentes[0] + estudiante.codigo + ".jpg";

                }
                else
                {
                    ViewBag.imagen1 = Constantes.url_noimage;

                }

                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[1] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[1] + estudiante.codigo, ".jpg");

                    ViewBag.imagen2 = path1;
                    ViewBag.imagen2a = Constantes.documentos_estudianteResidentes[1] + estudiante.codigo + ".jpg";

                }
                else
                {
                    ViewBag.imagen2 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[2] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[2] + estudiante.codigo, ".jpg");

                    ViewBag.imagen3 = path1;
                    ViewBag.imagen3a = Constantes.documentos_estudianteResidentes[2] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen3 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[3] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[3] + estudiante.codigo, ".jpg");

                    ViewBag.imagen4 = path1;
                    ViewBag.imagen4a = Constantes.documentos_estudianteResidentes[3] + estudiante.codigo + ".jpg";

                }
                else
                {
                    ViewBag.imagen4 = Constantes.url_noimage;

                }




                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[4] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[4] + estudiante.codigo, ".jpg");

                    ViewBag.imagen5 = path1;
                    ViewBag.imagen5a = Constantes.documentos_estudianteResidentes[4] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen5 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[5] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[5] + estudiante.codigo, ".jpg");

                    ViewBag.imagen6 = path1;
                    ViewBag.imagen6a = Constantes.documentos_estudianteResidentes[5] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen6 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[6] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[6] + estudiante.codigo, ".jpg");

                    ViewBag.imagen7 = path1;
                    ViewBag.imagen7a = Constantes.documentos_estudianteResidentes[6] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen7 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[7] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[7] + estudiante.codigo, ".jpg");

                    ViewBag.imagen8 = path1;
                    ViewBag.imagen8a = Constantes.documentos_estudianteResidentes[7] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen8 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[8] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[8] + estudiante.codigo, ".jpg");

                    ViewBag.imagen9 = path1;
                    ViewBag.imagen9a = Constantes.documentos_estudianteResidentes[8] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen9 = Constantes.url_noimage;

                }

                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[9] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[9] + estudiante.codigo, ".jpg");

                    ViewBag.imagen10 = path1;
                    ViewBag.imagen10a = Constantes.documentos_estudianteResidentes[9] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen10 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[10] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[10] + estudiante.codigo, ".jpg");

                    ViewBag.imagen11 = path1;
                    ViewBag.imagen11a = Constantes.documentos_estudianteResidentes[10] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen11 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[11] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[11] + estudiante.codigo, ".jpg");

                    ViewBag.imagen12 = path1;
                    ViewBag.imagen12a = Constantes.documentos_estudianteResidentes[11] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen12 = Constantes.url_noimage;

                }


                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[12] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[12] + estudiante.codigo, ".jpg");

                    ViewBag.imagen13 = path1;
                    ViewBag.imagen13a = Constantes.documentos_estudianteResidentes[12] + estudiante.codigo + ".jpg";


                }
                else
                {
                    ViewBag.imagen13 = Constantes.url_noimage;

                }



                path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[13] + estudiante.codigo, ".jpg");

                if (Utilidades.remoteFileExists(path1))
                {
                    path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, Constantes.documentos_estudianteResidentes[13] + estudiante.codigo, ".jpg");

                    ViewBag.imagen14 = path1;
                    ViewBag.imagen14a = Constantes.documentos_estudianteResidentes[13] + estudiante.codigo + ".jpg";


                }

                else
                {
                    ViewBag.imagen14 = Constantes.url_noimage;

                }

                return View(estudiante);


            }
        }



       
        public ActionResult
            guardaDocumentos(Estudiante estudiante)//GUARDA ARCHIVOS
        {
            int numFiles = Request.Files.Count;
            if (Request != null)
            {


                int uploadedCount = 0;

                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        string path1 = string.Format("{0}{1}{2}", Server.MapPath("../../Uploads/"), Constantes.documentos_estudiante[i] + estudiante.codigo, ".jpg");
                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);

                        file.SaveAs(path1);
                        uploadedCount++;
                    }
                }
            }
            return View(estudiante);
        }






        public Boolean validaDocumentos(Estudiante estudiante, string[] documentos)//
        {
            Boolean estado = true;
            int uploadedCount = 0;
            int numFiles = Request.Files.Count;


            for (int i = 0; i < Constantes.documentos_estudiante.Length-1; i++)
            {
                string path1 = string.Format("{0}/{1}{2}", Constantes.url_folder, documentos[i] + estudiante.codigo, ".jpg");



                if (!Utilidades.remoteFileExists(path1))
                {
                    estado = false;
                    return estado;
                }


                uploadedCount++;

            }
            return estado;

        }

        public ActionResult guardaDocumentosResidentes(Estudiante estudiante)//GUARDA ARCHIVOS
        {
            int numFiles = Request.Files.Count;
            if (Request != null)
            {


                int uploadedCount = 0;

                string[] documentose = Constantes.documentos_estudianteResidentes;
                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        string path1 = string.Format("{0}/{1}{2}", Server.MapPath("../../Uploads/"), Constantes.documentos_estudianteResidentes[i] + estudiante.codigo, ".jpg");
                        if (Utilidades.remoteFileExists(path1))
                            System.IO.File.Delete(path1);

                        file.SaveAs(path1);
                        uploadedCount++;
                    }
                }
            }
            return View(estudiante);
        }

        public ActionResult guardaDocumentosResidentesDS(Estudiante estudiante)//GUARDA ARCHIVOS
        {
            int numFiles = Request.Files.Count;
            if (Request != null)
            {


                int uploadedCount = 0;


                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        string path1 = string.Format("{0}/{1}{2}", Server.MapPath("../../Uploads/"), Constantes.documentos_estudianteResidentes[i] + estudiante.codigo, ".jpg");
                        if (Utilidades.remoteFileExists(path1))

                            System.IO.File.Delete(path1);

                        file.SaveAs(path1);
                        uploadedCount++;
                    }
                }
            }
            return View(estudiante);
        }








        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Personales(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {

                HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
                Estudiante est = db.Estudiantes.Find(estudiante.estudianteId);
                oHojaVida.direccion_manizales = estudiante.HojaVida.direccion_manizales;
                oHojaVida.hemoclasificacion = estudiante.HojaVida.hemoclasificacion;
                oHojaVida.municipio_procedencia = estudiante.HojaVida.municipio_procedencia;
                oHojaVida.hijos = estudiante.HojaVida.hijos;
                oHojaVida.correo = estudiante.HojaVida.correo;
                oHojaVida.estado_civil = estudiante.HojaVida.estado_civil;
                oHojaVida.num_celular = estudiante.HojaVida.num_celular;
                oHojaVida.num_telefono = estudiante.HojaVida.num_telefono;
                oHojaVida.Familia = estudiante.HojaVida.Familia;

                int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
                string edadDocente = edad.ToString();
                est.barrio_procedencia = edadDocente;//Reemplaza edad
                estudiante.HojaVida = null;
                db.Entry(est).State = EntityState.Modified;
                guardaDocumentos(estudiante);
                db.SaveChanges();
                validarCampos(estudiante,false);
                cargaDocumentos(estudiante);
                return RedirectToAction("../Estudiante/Personales/" + est.estudianteId);

            }
            else
            {

                validarCampos(estudiante,false);
                cargaDocumentos(estudiante);
                Estudiante estudiante2 = db.Estudiantes.Find(estudiante.estudianteId);
                return View(estudiante2);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalesResidentes(Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {

                HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
                Estudiante est = db.Estudiantes.Find(estudiante.estudianteId);

                oHojaVida.direccion_manizales = estudiante.HojaVida.direccion_manizales;
                oHojaVida.hemoclasificacion = estudiante.HojaVida.hemoclasificacion;
                oHojaVida.municipio_procedencia = estudiante.HojaVida.municipio_procedencia;
                oHojaVida.hijos = estudiante.HojaVida.hijos;
                oHojaVida.correo = estudiante.HojaVida.correo;
                oHojaVida.estado_civil = estudiante.HojaVida.estado_civil;
                oHojaVida.num_celular = estudiante.HojaVida.num_celular;
                oHojaVida.num_telefono = estudiante.HojaVida.num_telefono;
                oHojaVida.Familia = estudiante.HojaVida.Familia;

                int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
                string edadDocente = edad.ToString();
                est.barrio_procedencia = edadDocente;//Reemplaza edad

                estudiante.HojaVida = null;

                db.Entry(est).State = EntityState.Modified;

                guardaDocumentosResidentes(estudiante);

                db.SaveChanges();
                validarCampos(estudiante,true);

                cargaDocumentoResidentes(estudiante);

                return RedirectToAction("../Estudiante/PersonalesResidentes/" + est.estudianteId);
                // return View(est);
            }
            else
            {
                validarCampos(estudiante,true);
                cargaDocumentoResidentes(estudiante);
                Estudiante estudiante2 = db.Estudiantes.Find(estudiante.estudianteId);


                return View(estudiante2);
            }

        }








        public String ImagePath(Estudiante estudiante)
        {

            {
                return "~/Uploads/img/cedula_" + estudiante.codigo + ".jpg";
            }

        }


        //
        //------------------------- Vista para datos familia del estudiante




        public ActionResult PersonalesDS(int id = 0)
        {

            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            cargaImagen(estudiante);

            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad
            validarCampos(estudiante,false);

            cargaDocumentos(estudiante);
            return View(estudiante);
        }

        public ActionResult PersonalesDpto(int id = 0)
        {

              TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            cargaImagen(estudiante);

            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad
            validarCampos(estudiante,false);

            cargaDocumentos(estudiante);
            return View(estudiante);

        }


        public ActionResult PersonalesResidentesDS(int id = 0)
        {



            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);
            cargaImagen(estudiante);

            int edad = DateTime.Today.AddTicks(-estudiante.HojaVida.fecha_nacimiento.Ticks).Year - 1;
            string edadDocente = edad.ToString();
            estudiante.barrio_procedencia = edadDocente;//Reemplaza edad
            validarCampos(estudiante, true);

            cargaDocumentos(estudiante);
            return View(estudiante);
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SolicitarActualizacion(Estudiante estudiante)
        {
            estudiante = db.Estudiantes.Find(estudiante.estudianteId);
            @ViewBag.actualizar = "si";
            if (estudiante.HojaVida.correo != null && !estudiante.HojaVida.correo.Equals(""))
            {
                var fromAddress = new MailAddress("info@salud.ucaldas.edu.co", "Decanatura – Oficina Docencia Servicio");
                var toAddress = new MailAddress(estudiante.HojaVida.correo, "To Name");
                const string fromPassword = "descargar";
                const string subject = "Solicitud de actualizacion, Hoja de vida";
                const string body = "<h3>Cordial saludo</h3><h3 style=\"text-align: justify;\">La Facultad de Ciencias para la Salud a través de su Oficina Docencia Servicio le solicita actualizar su hoja de vida; para ello disponemos de la nueva plataforma web la cual podrá acceder a través del siguiente enlace.</h3><h3>&nbsp;<a href=\"http://salud.ucaldas.edu.co\">http://salud.ucaldas.edu.co/</a></h3><h3>Los datos de ingreso son:&nbsp;</h3><h3><strong>Usuario</strong>: Código de estudiante</h3><h3><strong>Contrase&ntilde;a</strong>: Num de Documento de Identidad&nbsp;</h3><p>&nbsp;</p><p>&nbsp;</p><p><img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Universidad_De_Caldas_-_Logo.jpg/180px-Universidad_De_Caldas_-_Logo.jpg\" alt=\"\" width=\"160\" height=\"160\" /></p><p>&nbsp;</p><p>Copyright &copy; <a href=\"http://www.ucaldas.edu.co/portal\"><strong>Facultad de Ciencias para la Salud </strong></a> - Sede Versalles Carrera 25  48-57 / Tel +57 878 30 60 Ext. 31255 / E-mail docencia.servicio@ucaldas.edu.co</p> ";
                //const string bodys = "<h3>Cordial saludo</h3><h3 style=\"text-align: justify;\">La Facultad de Ciencias para la Salud a través de su Oficina Docencia Servicio le solicita actualizar su hoja de vida; para ello disponemos de la nueva plataforma web la cual podrá acceder a través del siguiente enlace.</h3><h3>&nbsp;<a href=\"http://localhost:34649/Estudiante/Login\">http://localhost:34649/</a></h3><h3>Los datos de ingreso son:&nbsp;</h3><h3><strong>Usuario</strong>: Código de estudiante</h3><h3><strong>Contrase&ntilde;a</strong>: Código de estudiante&nbsp;</h3><p>&nbsp;</p><p>&nbsp;</p><p><img src=\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Universidad_De_Caldas_-_Logo.jpg/180px-Universidad_De_Caldas_-_Logo.jpg\" alt=\"\" width=\"160\" height=\"160\" /></p><p>&nbsp;</p><p>Copyright &copy; <a href=\"http://www.ucaldas.edu.co/portal\"><strong>Facultad de Ciencias para la Salud </strong></a> - Sede Versalles Carrera 25  48-57 / Tel +57 878 30 60 Ext. 31255 / E-mail docencia.servicio@ucaldas.edu.co</p> ";


                try
                {

                    var smtp = new SmtpClient
                    {
                        Host = "72.29.75.91",
                        Port = 25,
                        EnableSsl = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Timeout = 10000,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    var message = new MailMessage(fromAddress, toAddress);
                    message.To.Add("servidor.facsalud@ucaldas.edu.co");


                    message.IsBodyHtml = true;
                    message.Subject = subject;
                    message.Body = body;



                    smtp.EnableSsl = false;
                    smtp.Send(message);


                }


                catch (Exception e)
                {

                    Console.WriteLine("Ouch!" + e.ToString());

                }

            }
            // var fromAddress = new MailAddress("docenciaservicioucaldas@hotmail.com", "Decanatura – Oficina Docencia Servicio");


            return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalesDS(Estudiante estudiante)
        {

            estudiante = db.Estudiantes.Find(estudiante.estudianteId);

            guardaDocumentos(estudiante);


            Boolean estado = validarCampos(estudiante,false);

            //return View(estudiante);
            return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalesDpto(Estudiante estudiante)
        {


            estudiante = db.Estudiantes.Find(estudiante.estudianteId);

            guardaDocumentos(estudiante);


            Boolean estado = validarCampos(estudiante, false);

            //return View(estudiante);
            return RedirectToAction("../Estudiante/PersonalesDpto/" + estudiante.estudianteId);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalesResidentesDS(Estudiante estudiante)
        {

            estudiante = db.Estudiantes.Find(estudiante.estudianteId);

            guardaDocumentos(estudiante);


            Boolean estado = validarCampos(estudiante, true);

            //return View(estudiante);
            return RedirectToAction("../Estudiante/PersonalesDS/" + estudiante.estudianteId);


        }

        public ActionResult SoportesCompletos(int id = 0)
        {
            //TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            cargaDocumentos(estudiante);
            return View(estudiante);
        }

        public ActionResult SoportesCompletosDS(int id = 0)
        {
            //TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            cargaDocumentos(estudiante);
            return View(estudiante);
        }


        public ActionResult SoportesCompletosResidentes(int id = 0)
        {
            //TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            cargaDocumentoResidentes(estudiante);
            return View(estudiante);
        }

        public ActionResult SoportesCompletosResidentesDS(int id = 0)
        {
            //TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            cargaDocumentoResidentes(estudiante);
            return View(estudiante);
        }






        //
        //------------------------- Vista para datos academicos del estudiante

        public ActionResult Academicos(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult AcademicosDS(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }


        public ActionResult AcademicosResidentes(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult AcademicosResidentesDS(int id = 0)
        {

            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }


        //
        //------------------------- Vista para datos de vacunacion del estudiante

        public ActionResult CarnetVacunacion(int id = 0)
        {


            Estudiante estudiante = db.Estudiantes.Find(id);

            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = estudiante.HojaVida.Vacunas;
            return View(estudiante);

        }

        public ActionResult CarnetVacunacionResidentes(int id = 0)
        {


            Estudiante estudiante = db.Estudiantes.Find(id);

            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = estudiante.HojaVida.Vacunas;
            return View(estudiante);

        }

        public ActionResult CarnetVacunacionResidentesODS(int id = 0)
        {


            Estudiante estudiante = db.Estudiantes.Find(id);

            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = estudiante.HojaVida.Vacunas;
            return View(estudiante);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarnetVacunacionDS(Estudiante estudiante, int id = 0)
        {


            return RedirectToAction("../Vacuna/EsquemaVacunacion/" + estudiante.estudianteId);


        }





        public ActionResult CarnetVacunacionDS(int id = 0)
        {
            TempData["notice"] = null;

            Estudiante estudiante = db.Estudiantes.Find(id);
            HojaVida oHojaVida = db.HojaVidas.Find(estudiante.hojaVidaId);

            try
            {
                var request = WebRequest.Create(oHojaVida.imagen_DI);
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        // Process the stream
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError &&
                    ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        oHojaVida.imagen_DI = "http://www.tagetik.com/intouch2015/user.png";
                    }
                    else
                    {
                        // Do something else
                    }
                }
                else
                {
                    // Do something else
                }
            }

            return View(estudiante);

        }



        public ActionResult CarnetVacunacionODS(int id = 0)
        {


            Estudiante estudiante = db.Estudiantes.Find(id);

            if (estudiante == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = estudiante.HojaVida.Vacunas;
            return View(estudiante);

        }

        //
        //------------------------- Vista para datos de salud del estudiante
        public ActionResult Salud(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];
            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }



        public ActionResult SaludDS(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];
            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult SaludResidentes(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];
            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        public ActionResult SaludResidentesDS(int id = 0)
        {
            string vacuna = Request.Params["vacuna"];
            Estudiante estudiante = db.Estudiantes.Find(id);
            cargaImagen(estudiante);
            if (estudiante == null)
            {
                return HttpNotFound();
            }
            return View(estudiante);

        }

        //
        //------------------------- METODO PARA CARGAR ARCHIVOS
        public ActionResult save()
        {
            int idestudiante = Int32.Parse(Request.Params["idEstudiante"]);
            Estudiante estudiante = db.Estudiantes.Find(idestudiante);

            if (estudiante == null)
            {
                return HttpNotFound();
            }
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["InputFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    string extension = System.IO.Path.GetExtension(Request.Files["InputFile"].FileName);
                    string path1 = string.Format("{0}/{1}{2}", Server.MapPath("~/"), "cedula_", extension);
                    if (Utilidades.remoteFileExists(path1))
                        System.IO.File.Delete(path1);

                    Request.Files["InputFile"].SaveAs(path1);
                }
            }
            return RedirectToAction("../Estudiante/Personales/" + estudiante.estudianteId);

            // return View("Personales");
        }
    }

}