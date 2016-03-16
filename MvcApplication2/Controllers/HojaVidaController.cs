using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Data.SqlTypes;
using System.Net;
using System.IO;
using System.Text;
using System.Data.Entity.Validation;

namespace MvcApplication2.Controllers
{
    public class HojaVidaController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /HojaVida/
        public ActionResult vistaHV()
        {
            return View();

        }

        public ActionResult HV_IPS_Universitaria()
        {

            return View();

        }

        public ActionResult HV_Departamento()
        {

            return View();

        }
      
        public ActionResult Index(int id = 0)
        {



            var hojavidas = db.HojaVidas.Include(h => h.Familia);
             //importaDocentes();
            return View(hojavidas.ToList());
        }

        //
        // GET: /HojaVida/Details/5

        public ActionResult Details(int id = 0)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            if (hojavida == null)
            {
                return HttpNotFound();
            }
            return View(hojavida);
        }

        //
        // GET: /HojaVida/Create

        public ActionResult Create()
        {
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre");
            return View();
        }

        public ActionResult CreateDocente()
        {
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre");
            return View();
        }
        //
        // POST: /HojaVida/Create


        //public ActionResult Create(HojaVida hojavida)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.HojaVidas.Add(hojavida);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre", hojavida.familiaId);
        //    return View(hojavida);
        //}
        public HojaVida getSalud(HojaVida hojavida)
        {
            string urlAddress = "";

            if (hojavida.Docente != null)
                urlAddress = "http://www.fosyga.gov.co/Aplicaciones/AfiliadoWebBDUA/Afiliado/Formulario/buda_consulta_afil_sin_dnn.aspx?id=" + hojavida.Docente.ElementAt(0).num_documento + "&tipodocumento=CC";
            else
                urlAddress = "http://www.fosyga.gov.co/Aplicaciones/AfiliadoWebBDUA/Afiliado/Formulario/buda_consulta_afil_sin_dnn.aspx?id=" + hojavida.Estudiante.ElementAt(0).num_documento + "&tipodocumento=CC";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                int pos = data.IndexOf("ldlEstadodata2");
                string estado = data.Substring(pos + 16);
                estado = estado.Substring(0, 1);
                hojavida.estado_afiliacion = estado;
                // if (estado.Equals("A"))
                //{
                pos = data.IndexOf("lblEntidadData2");

                string entidad = data.Substring(pos);
                int pos2 = entidad.IndexOf("<");
                string pos3 = pos2.ToString();
                entidad = entidad.Substring(17, pos2 - 17);
                hojavida.entidad_salud = entidad;

                //  }


            }

            return hojavida;

        }

        //
        // GET: HOJA DE VIDA EDITADA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HojaVida hojavida)
        {
            if (ModelState.IsValid)
            {

                Familia familia = new Familia();
                familia.primer_nombre_padre = "";
                familia.segundo_nombre_padre = "";
                familia.primer_apellido_padre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_padre = 0;
                familia.primer_nombre_madre = "";
                familia.segundo_nombre_madre = "";
                familia.primer_apellido_madre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_madre = 0;
                familia.primer_nombre_acudiente = "";
                familia.segundo_nombre_acudiente = "";
                familia.primer_apellido_acudiente = "";
                familia.segundo_apellido_acudiente = "";
                familia.telefono_acudiente = 0;
                db.Familias.Add(familia);
                db.SaveChanges();



                var iffam = db.Familias.Max(p => p.familiaId);
                hojavida.familiaId = iffam;
                db.HojaVidas.Add(hojavida);
                db.SaveChanges();

                iffam = db.HojaVidas.Max(p => p.hojaVidaId);


                //pte crear estudiante
                //Estudiante estudiante = new Estudiante();
                //estudiante.tipo_documento = "CC";
                //estudiante.num_documento = "10184756378";
                //estudiante.codigo = 1700921759;
                //estudiante.direccion_procedencia = "calle 56";
                //estudiante.barrio_procedencia = "linares";
                //estudiante.telefono_procedencia = "564565";
                //estudiante.clave = "12345";
                //estudiante.modalidad = "Universitario";
                //estudiante.programaId = 1;
                //estudiante.semestre = 8;
                //estudiante.estado_academico = "matriculado";
                //estudiante.hojaVidaId = iffam;
                //estudiante.rotacionId = 1;
                //db.Estudiantes.Add(estudiante);
                //db.SaveChanges();


                List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
                foreach (var item2 in departamentos)
                {
                    ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();

                    string json = ser.getProfesoresActivos(item2.codigo);
                }
                Docente docente = new Docente();
                docente.tipo_documento = "CC";
                docente.num_documento = "1053793956";


                docente.clave = "12345";

                docente.hojaVidaId = iffam;

                db.Docentes.Add(docente);
                db.SaveChanges();


                if (
                    hojavida.Docente != null)
                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";
                // else
                //    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";

                Vacuna vacuna = new Vacuna();
                vacuna.hojaVidaId = iffam;


                vacuna.nombre_generico = ("Hepatitis B Dosis 1");
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Triple Viral Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Varicela Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 4");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 5");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Fiebre Amarilla Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Influenza Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("DTP Acelular Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Virus del papiloma humano Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra varicela");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra hepatitis B");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();






                db.SaveChanges();
                return View(hojavida);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDocente(HojaVida hojavida)
        {
            if (ModelState.IsValid)
            {

                Familia familia = new Familia();
                familia.primer_nombre_padre = "";
                familia.segundo_nombre_padre = "";
                familia.primer_apellido_padre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_padre = 0;
                familia.primer_nombre_madre = "";
                familia.segundo_nombre_madre = "";
                familia.primer_apellido_madre = "";
                familia.segundo_apellido_madre = "";
                familia.telefono_madre = 0;
                familia.primer_nombre_acudiente = "";
                familia.segundo_nombre_acudiente = "";
                familia.primer_apellido_acudiente = "";
                familia.segundo_apellido_acudiente = "";
                familia.telefono_acudiente = 0;
                db.Familias.Add(familia);
                db.SaveChanges();



                var iffam = db.Familias.Max(p => p.familiaId);
                hojavida.familiaId = iffam;
                db.HojaVidas.Add(hojavida);
                db.SaveChanges();

                iffam = db.HojaVidas.Max(p => p.hojaVidaId);


                //pte crear estudiante
                Docente docente = new Docente();
                docente.tipo_documento = "CC";
                docente.num_documento = "10184756378";
                docente.clave = "12345";
                docente.DepartamentoSaludId = 1;
                docente.num_libreta_militar = "";
                docente.hojaVidaId = iffam;
                db.Docentes.Add(docente);
                db.SaveChanges();


                hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";

                Vacuna vacuna = new Vacuna();
                vacuna.hojaVidaId = iffam;


                vacuna.nombre_generico = ("Hepatitis B Dosis 1");
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;

                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis B Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Hepatitis A Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Triple Viral Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Varicela Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 2");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 3");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 4");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Toxoide Tetánico Dosis 5");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Fiebre Amarilla Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Influenza Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("DTP Acelular Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Virus del papiloma humano Dosis 1");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra varicela");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

                vacuna.nombre_generico = ("Anticuerpos contra hepatitis B");
                db.Vacunas.Add(vacuna);
                db.SaveChanges();






                db.SaveChanges();
                return View(hojavida);
            }
            return RedirectToAction("Index");
        }


        //
        // Adicionar anticuerpos
        public void AdicionarAnticuerpo()
        {
            List<HojaVida> listaHV = db.HojaVidas.ToList();
            foreach (var item in listaHV)
            {
                Vacuna vacuna = new Vacuna();

                vacuna.hojaVidaId = item.hojaVidaId;

                vacuna.nombre_generico = "Anticuerpos contra varicela";
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.laboratorio_vacunacion = "";
                vacuna.laboratorioOtro = "";
                vacuna.lote = "0";
                try
                {
                    db.Vacunas.Add(vacuna);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
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
                vacuna.nombre_generico = "Anticuerpos contra hepatitis B ";
                vacuna.fecha_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.fecha_prox_vacunacion = SqlDateTime.MinValue.Value;
                vacuna.laboratorio_vacunacion = "";
                vacuna.laboratorioOtro = "";
                vacuna.lote = "0";
                db.Vacunas.Add(vacuna);
                db.SaveChanges();

            }


        }

        //
        // GET: /HojaVida/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            if (hojavida == null)
            {
                return HttpNotFound();
            }
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre", hojavida.familiaId);
            return View(hojavida);
        }

        //
        // POST: /HojaVida/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HojaVida hojavida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hojavida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.familiaId = new SelectList(db.Familias, "familiaId", "primer_nombre_padre", hojavida.familiaId);
            return View(hojavida);
        }

        //
        // GET: /HojaVida/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            if (hojavida == null)
            {
                return HttpNotFound();
            }
            return View(hojavida);
        }

        //
        // POST: /HojaVida/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HojaVida hojavida = db.HojaVidas.Find(id);
            db.HojaVidas.Remove(hojavida);
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