using MvcApplication2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication2.Controllers
{
    public class ActualizacionController : Controller
    {
        private UsersContext2 db = new UsersContext2();

        //
        // GET: /Actualizacion/

        public ActionResult Index()
        {
            //importaMaterias();
            //importaGruposMateria();

            //importaDocentes();
            //importaDocentestest();

            //importaEstudiantes();
            importaEstudiantesRotacion();
            // actualizaImagenDocentes();
            //importaEstudiantesRotacionDetalle();
            //importaEstudiantesRotacionTest();


            return View();
        }
        public void importaEstudiantesRotacionDetalle()
        {
            List<RotacionEstudiante> rotacionEstudiantes = db.RotacionEstudiantes.ToList();
            foreach (RotacionEstudiante rotacionEstudiante in rotacionEstudiantes)
            {
                RotacionEstudianteDetalle rotacionEstudianteDetalle = new RotacionEstudianteDetalle();
                rotacionEstudianteDetalle.rotacionEstudianteId = rotacionEstudiante.rotacionEstudianteId;

                rotacionEstudianteDetalle.IPS_ESEId = rotacionEstudiante.IPS_ESEId;
                rotacionEstudianteDetalle.docentes = db.RotacionDocentes.Where(r => r.rotacionEstudianteId == rotacionEstudiante.rotacionEstudianteId).ToList().ElementAt(0).nombre;
                rotacionEstudianteDetalle.fecha_inicio = rotacionEstudiante.Rotacion.fecha_inicio;
                rotacionEstudianteDetalle.fecha_terminacion = rotacionEstudiante.Rotacion.fecha_terminacion;
                List<RotacionEstudianteDetalle> rotacionEstudianteDetalleAux = db.RotacionEstudianteDetalle.Where(r => r.fecha_inicio == rotacionEstudianteDetalle.fecha_inicio).Where(r => r.fecha_terminacion == rotacionEstudianteDetalle.fecha_terminacion)
                    .Where(r => r.IPS_ESEId == rotacionEstudianteDetalle.IPS_ESEId)
                    .Where(r => r.docentes == rotacionEstudianteDetalle.docentes)
                    .Where(r => r.rotacionEstudianteId==rotacionEstudianteDetalle.rotacionEstudianteId).ToList(); 

                if(rotacionEstudianteDetalleAux.Count==0)
                {
                    db.RotacionEstudianteDetalle.Add(rotacionEstudianteDetalle);
                    db.SaveChanges();
                }
            }

        }
        public void enviaSolicituudTodosEstudiantes()
        {
            List<Estudiante> estudiantes = db.Estudiantes.Include(e => e.HojaVida).Include(e => e.Programa).ToList();

            foreach (Estudiante estudiante in estudiantes)
            {
                string correo = estudiante.HojaVida.correo;
                if (correo != null && correo.Equals(""))
                {
                    enviarCorreo(estudiante.HojaVida.correo);


                }
            }

        }
        public void enviarCorreo(string mail)
        {

            var fromAddress = new MailAddress("info@salud.ucaldas.edu.co", "Decanatura – Oficina Docencia Servicio");
            var toAddress = new MailAddress(mail, "To Name");
            const string fromPassword = "descargar";
            const string subject = "Solicitud actualizacion hoja de vida";
            const string body = "<p><br />Estimado Estudiante,&nbsp;</p><p>&nbsp;Para continuar avanzando con el proceso acad&eacute;mico, es importante realizar la actualizaci&oacute;n de su hoja de vida en el nuevo aplicativo de la Facultad de Ciencias para la Salud.</p><p>&nbsp;</p><p style=\"text-align: center;\"><strong>POR FAVOR LEA ESTE CORREO (antes de iniciar el proceso)</strong></p><p><br /><br />Como parte del proceso de inscripci&oacute;n a rotaciones, es&nbsp;<strong>OBLIGATORIO</strong>&nbsp;que realice v&iacute;a internet la actualizaci&oacute;n de sus datos personales y carga de archivos en nuestro sistema SDS (Sistema Docencia Servicio), porque se ha implementado como iniciativa de un cambio en la mejora de los procesos que actualmente se gestionan desde la oficina Docencia Servicio.<br /><br /></p><p>A continuaci&oacute;n se adjunta el manual con las instrucciones para que realice la actualizaci&oacute;n, cualquier inquietud estamos a sus &oacute;rdenes.&nbsp;</p><h2><span style=\"color: #ff0000;\"><a href=\"http://salud.ucaldas.edu.co/Proyecto/Manual%20Estudiante.pdf\"><strong>Ver Aqui</strong></a></span><br /><br /><strong>Fecha l&iacute;mite de actualizaci&oacute;n: 26 de Febrero&nbsp;de 2016</strong>&nbsp;<strong><br /></strong></h2><p>De tener dudas o inquietudes, puede escribirnos a nuestro correo&nbsp;<a href=\"mailto:servidor.facsalud@ucaldas.edu.co\">servidor.facsalud@ucaldas.edu.co</a>,&nbsp;&nbsp;docencia.servicio@ucaldas.edu.co &oacute; acercarse a la Oficina Docencia Servicio.<br /><br /></p><p>Tener a mano en formato jpg/png y por separado los siguientes documentos:</p><p>1.&nbsp;&nbsp;&nbsp;&nbsp;Documento de identidad (Ambos lados)<br />2.&nbsp;&nbsp;&nbsp;&nbsp;Carn&eacute; Liberty Seguros<br />3.&nbsp;&nbsp;&nbsp;&nbsp;Carn&eacute; estudiantil<br />4.&nbsp;&nbsp;&nbsp;&nbsp;Carn&eacute; EPS<br />5.&nbsp;&nbsp;&nbsp;&nbsp;Carn&eacute; vacunaci&oacute;n 1 (Carnet de vacunaci&oacute;n con esquema obligatorio)<br />6.&nbsp;&nbsp;&nbsp;&nbsp;Carn&eacute; vacunaci&oacute;n 2 (En caso que tenga m&aacute;s de un soporte de&nbsp; vacunaci&oacute;n)<br />7.&nbsp;&nbsp;&nbsp;&nbsp;Anticuerpos contra varicela<br />8.&nbsp;&nbsp;&nbsp;&nbsp;Anticuerpos contra hepatitis B<br />&nbsp;</p><p><strong>ANTICUERPOS DE VARICELA son OBLIGATORIOS, esto a ra&iacute;z de que las hojas de vida que estaban completas el semestre pasado a la gran mayor&iacute;a se les recomend&oacute; que se realizaran estos anticuerpos.</strong></p><p>&nbsp;</p><p>&nbsp;</p><p><strong>MAR&Iacute;A DEL PILAR GIL VALENCIA&nbsp;<br />Coordinadora&nbsp;<br />Oficina Docencia Servicio<br />Facultad Ciencias para la Salud<br />Universidad de Caldas<br />Carrera 25&nbsp; 48-57- Sede Versalles<br /></strong></p><div><div>878 30 60 Ext. 31255</div></div><p><img src=\"https://ci6.googleusercontent.com/proxy/FL7efXE8rOXxE9fg--htniHt2dU5_zUelHPV_ZgolYIoiqbitLuy6UTr-A56XfSCPeLLVSg4rVV1LUzivDRc7OrbZhxftNYOzxpCWRPk_Gf4zUuypCmb3-9aU1q_=s0-d-e1-ft#https://udecaldas.files.wordpress.com/2015/12/firma-institucional_n.jpg\" alt=\"\" width=\"900\" height=\"150\" /></p><p>&nbsp;</p>";


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
            var message = new MailMessage();
            message.From = fromAddress;

            ///   List<Estudiante> estudiantes = db.Estudiantes.Include(e => e.HojaVida);
            var estudiantes = db.Estudiantes.Include(e => e.HojaVida).Include(e => e.Programa);

            foreach (Estudiante estudiante in estudiantes.ToList())
            {
                string correo = estudiante.HojaVida.correo;
                if (correo != null && !correo.Equals(""))
                {
                    string textResultado = Regex.Replace(correo, @"[^a-zA-z0-9 ]+", "");


                    message.To.Add(textResultado);


                }
            }
            message.To.Add(mail);



            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;





            string file = string.Format("{0}/{1}{2}", Server.MapPath("~/Images/"), "ManualEstudiante", ".pdf");

            message.Attachments.Add(new System.Net.Mail.Attachment(file));




            smtp.EnableSsl = false;
            smtp.Send(message);






        }
        public void actualizaImagenEstudiantes()
        {
            List<HojaVida> estudiantes = db.HojaVidas.Where(r => r.imagen_DI == null).ToList();
            foreach (HojaVida hojavida in estudiantes)
            {
                List<Estudiante> estudiante = db.Estudiantes.Where(r => r.hojaVidaId == hojavida.hojaVidaId).ToList();
                if (estudiante.Count() > 0)
                {
                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante[0].codigo + ".jpg";
                    hojavida.municipio_procedencia = ".";
                    hojavida.num_celular = 3000000000;


                    db.Entry(hojavida).State = EntityState.Modified;
                    try
                    {

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

                }


            }
        }


        public void actualizaImagenDocentes()
        {
            List<HojaVida> estudiantes = db.HojaVidas.Where(r => r.imagen_DI == null).ToList();
            foreach (HojaVida hojavida in estudiantes)
            {
                List<Docente> estudiante = db.Docentes.Where(r => r.hojaVidaId == hojavida.hojaVidaId).ToList();
                if (estudiante.Count() > 0)
                {
                    hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante[0].num_documento + ".jpg";

                    hojavida.municipio_procedencia = ".";
                    hojavida.num_celular = 3000000000;

                    db.Entry(hojavida).State = EntityState.Modified;
                    try
                    {

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

                }


            }
        }
        public void importaMaterias()
        {
            ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();

            string json = ser.getMaterias();

            json = json.Replace("\"materias\"", "6@");

            json = json.Replace("\":\"", "1@");
            json = json.Replace("\",\"", "2@");
            json = json.Replace("{\"", "3@");
            json = json.Replace("\"}", "4@");
            json = json.Replace("\"\"", "5@");

            json = json.Replace("\"", "");


            json = json.Replace("1@", "\":\"");
            json = json.Replace("2@", "\",\"");
            json = json.Replace("3@", "{\"");
            json = json.Replace("4@", "\"}");
            json = json.Replace("5@", "\"\"");
            json = json.Replace("6@", "\"materias\"");



            MvcApplication2.Models.Materia.ESObject0 listmaterias = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Materia.ESObject0>(json);
            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            int cont = 0;
            foreach (var item in listmaterias.materias)
            {
                var materias = db.ActividadAcademicas.Where(r => r.codigo_AA.Equals(item.COD_MATERIA));
                if (materias.ToList().Count == 0)
                {
                    ActividadAcademica academica = new ActividadAcademica();
                    Boolean estado = false;
                    int iddept = 0;


                    foreach (var item2 in departamentos)
                    {

                        if (item2.nombre.ToUpper().Equals(item.NOM_DEPTO))
                        {
                            estado = true;

                            iddept = item2.DepartamentoSaludId;
                        }



                    }
                    if (estado)
                    {
                        academica.DepartamentoSaludId = iddept;

                    }

                    cont++;
                    if (academica.DepartamentoSaludId != 0)
                    {
                        academica.asignatura = item.NOM_MATERIA;
                        academica.nombre = item.NOM_MATERIA;
                        academica.codigo_AA = item.COD_MATERIA;

                        academica.modalidad_practica = item.PMO_NOMBRE;
                        if (item.GRUPOS_MAXIMO != null && !item.GRUPOS_MAXIMO.Equals(String.Empty))
                        {
                            academica.grupo_maximo = Int32.Parse(item.GRUPOS_MAXIMO);

                        }
                        db.ActividadAcademicas.Add(academica);
                        db.SaveChanges();

                    }
                }


            }
        }

        public void importaEstudiantesRotacionTest()
        {

            ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();

            Rotacion rotacion = db.Rotacions.Find(106150);

            string jsonInscritosGrupo = ser.getInscritosGrupo(rotacion.ActividadAcademica.codigo_AA, rotacion.grupo, rotacion.year_academico + "", rotacion.periodo_academico + "");
            
            if (jsonInscritosGrupo != null && !jsonInscritosGrupo.Equals(""))
            {
                MvcApplication2.Models.GruposInscritos.ESObject0 gruposInscritos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.GruposInscritos.ESObject0>(jsonInscritosGrupo);

                for (int i = 0; i < gruposInscritos.inscritosGrupo.Count; i++)
                {
                    var item3 = gruposInscritos.inscritosGrupo.ElementAt(i);

                    string[] cedulas = item3.CEDULA_PROFESOR.Split(';');

                    long codigo = Int64.Parse(item3.CODIGO);
                    var datos = db.RotacionEstudiantes.Where(r => r.Estudiante.codigo == codigo).Where(r => r.rotacionId == rotacion.rotacionId);
                    List<RotacionEstudiante> lista = datos.ToList();
                    if (lista.Count() == 0)
                    {
                        Estudiante estudiante2 = null;

                        var estudiantes = db.Estudiantes.Where(r => r.codigo == codigo).ToList();
                        if (estudiantes.Count == 0)
                        {
                            continue;

                        }
                        estudiante2 = (Estudiante)estudiantes.ElementAt(0);
                        RotacionEstudiante rotacionEstudiante = new RotacionEstudiante();
                        rotacionEstudiante.estudianteId = estudiante2.estudianteId;
                        rotacionEstudiante.rotacionId = rotacion.rotacionId;
                        rotacionEstudiante.IPS_ESEId = 1;
                        db.RotacionEstudiantes.Add(rotacionEstudiante);
                        db.SaveChanges();
                        var rotacionEstudianteId = db.RotacionEstudiantes.Max(p => p.rotacionEstudianteId);
                        InsertarRotacionDocente(cedulas, rotacionEstudianteId);





                    }
                }
            }
        }


        public void importaEstudiantesRotacion()
        {
            ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();

            List<Rotacion> rotaciones = db.Rotacions.ToList();
            foreach (var rotacion in rotaciones)
            {

                string jsonInscritosGrupo = ser.getInscritosGrupo(rotacion.ActividadAcademica.codigo_AA, rotacion.grupo, rotacion.year_academico + "", rotacion.periodo_academico + "");
                if (jsonInscritosGrupo != null && !jsonInscritosGrupo.Equals(""))
                {

                    MvcApplication2.Models.GruposInscritos.ESObject0 gruposInscritos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.GruposInscritos.ESObject0>(jsonInscritosGrupo);

                    for (int i = 0; i < gruposInscritos.inscritosGrupo.Count; i++)
                    {
                        var item3 = gruposInscritos.inscritosGrupo.ElementAt(i);

                        string[] cedulas = item3.CEDULA_PROFESOR.Split(';');

                        long codigo = Int64.Parse(item3.CODIGO);
                        var datos = db.RotacionEstudiantes.Where(r => r.Estudiante.codigo == codigo).Where(r => r.rotacionId == rotacion.rotacionId);
                        List<RotacionEstudiante> lista = datos.ToList();
                        if (lista.Count() == 0)
                        {
                            Estudiante estudiante2 = null;

                            var estudiantes = db.Estudiantes.Where(r => r.codigo == codigo).ToList();
                            if (estudiantes.Count == 0)
                            {
                                continue;

                            }
                            estudiante2 = (Estudiante)estudiantes.ElementAt(0);
                            RotacionEstudiante rotacionEstudiante = new RotacionEstudiante();
                            rotacionEstudiante.estudianteId = estudiante2.estudianteId;
                            rotacionEstudiante.rotacionId = rotacion.rotacionId;
                            rotacionEstudiante.IPS_ESEId = 1;
                            db.RotacionEstudiantes.Add(rotacionEstudiante);
                            db.SaveChanges();
                            var rotacionEstudianteId = db.RotacionEstudiantes.Max(p => p.rotacionEstudianteId);
                            InsertarRotacionDocente(cedulas, rotacionEstudianteId);





                        }
                    }



                }


            }
        }



        public void InsertarRotacionDocente(string[] cedulas, int rotacionEstudianteId)
        {
            for (int i = 0; i < cedulas.Length; i++)
            {
                string cedula = cedulas[i];
                if (!cedula.Equals(""))
                {
                    RotacionDocente rotacionDocente = new RotacionDocente();

                    var docente = db.Docentes.Where(r => r.num_documento == cedula);
                    if (docente.ToList().Count() > 0)
                    {
                        Docente docenteTemp = (Docente)docente.ToList().ElementAt(0);

                        rotacionDocente.docenteId = docenteTemp.docenteId;
                        rotacionDocente.nombre = docenteTemp.HojaVida.primer_nombre;
                    }
                    else
                    {

                        rotacionDocente.docenteId = 590;

                    }
                    rotacionDocente.rotacionEstudianteId = rotacionEstudianteId;
                    db.RotacionDocentes.Add(rotacionDocente);
                    db.SaveChanges();
                }

            }
        }
        public void importaGruposMateria()
        {
            List<ActividadAcademica> materias = db.ActividadAcademicas.ToList();
            foreach (var item in materias)
            {
                ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();

                string json = ser.getGruposMateria(item.codigo_AA);
                if (json != null && !json.Equals(""))
                {


                    MvcApplication2.Models.Grupos.ESObject0 gruposMaterias = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Grupos.ESObject0>(json);
                    foreach (var item2 in gruposMaterias.gruposMaterias)
                    {
                      
                            var datos = db.Rotacions.Where(r => r.actividadacademicaId == item.actividadacademicaId).Where(r => r.year_academico == item2.ANO).Where(r => r.periodo_academico == item2.PERIODO).Where(r => r.grupo.Equals(item2.GRUPO));
                            List<Rotacion> lista = datos.ToList();
                            if (lista.Count() == 0)
                            {
                                if (item2.ANO > 2015)
                                {
                                    Rotacion rotacion = new Rotacion();
                                    rotacion.year_academico = item2.ANO;
                                    rotacion.periodo_academico = item2.PERIODO;


                                    rotacion.grupo = item2.GRUPO;

                                    rotacion.horario = "";
                                    rotacion.numero_estudiantes = item2.INSCRITOS;
                                    DateTime myDate = DateTime.ParseExact(item2.FECHA_INICIO, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                    rotacion.fecha_inicio = myDate;

                                    DateTime myDate2 = DateTime.ParseExact(item2.FECHA_FINAL, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                    rotacion.fecha_terminacion = myDate2;
                                    rotacion.actividadacademicaId = item.actividadacademicaId;
                                    rotacion.IPS_ESEId = 1;
                                    rotacion.grupo = item2.GRUPO;
                                    db.Rotacions.Add(rotacion);
                                    db.SaveChanges();
                                }
                            }

                        


                    }
                }
            }
        }




        public void importaDocentes()
        {

            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            foreach (var item in departamentos)
            {
                ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();
                string json;

                try
                {
                    json = ser.getProfesoresActivos(item.codigo);
                }

                catch (Exception e)
                {
                    json = null;
                }
                if (json != null)
                {
                    MvcApplication2.Models.Profesor.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Profesor.ESObject0>(json);
                    foreach (var item2 in profesoresActivos.profesoresActivos)
                    {
                        string json2;
                        try
                        {
                            json2 = ser.getDatosProfesor(item2.CEDULA);
                        }

                        catch (Exception e)
                        {
                            json2 = null;
                        }
                        if (json2 != null)
                        {
                            MvcApplication2.Models.DocenteWS.ESObject0 profesores = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.DocenteWS.ESObject0>(json2);
                            foreach (var item3 in profesores.datosProfesor)
                            {


                                string imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + item3.CEDULA + ".jpg";

                                var hv = db.HojaVidas.Where(r => r.imagen_DI.Equals(imagen_DI));

                                List<HojaVida> hvs = hv.ToList();
                                if (hvs.Count > 0)
                                {

                                    HojaVida hvida = hvs.ElementAt(0);
                                    var docentes = db.Docentes.Where(r => r.hojaVidaId == hvida.hojaVidaId);
                                    Docente docente = null;
                                    try
                                    {
                                        List<Docente> sts = docentes.ToList();
                                        docente = sts.ElementAt(0);
                                        docente.titulo_pregrado = item3.CHIN_TITULO;
                                        docente.maximo_nivel_formacion = item3.CNIA_DESCRIPCION;
                                        docente.dedicacion = item3.CTUR_DESCRIPCION;
                                    }
                                    catch (Exception e)
                                    {

                                    }



                                    db.Entry(docente).State = EntityState.Modified;
                                    try
                                    {

                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }

                                }
                                else
                                {


                                    InsertaFamilia();
                                    var iffam = db.Familias.Max(p => p.familiaId);
                                    HojaVida hojavida = new HojaVida();
                                    hojavida.familiaId = iffam;


                                    Docente docente = new Docente();
                                    docente.tipo_documento = "CC";
                                    docente.num_documento = item3.CEDULA;
                                    if (!item3.LIBREMIL.Equals(""))
                                    {
                                        docente.num_libreta_militar = item3.LIBREMIL;
                                    }

                                    docente.clave = item3.CEDULA;
                                    docente.titulo_pregrado = item3.CHIN_TITULO;
                                    docente.maximo_nivel_formacion = item3.CNIA_DESCRIPCION;
                                    docente.dedicacion = item3.CTUR_DESCRIPCION;

                                    hojavida.primer_nombre = item3.NOMBRE;
                                    hojavida.primer_apellido = item3.P_APELLIDO;
                                    hojavida.segundo_apellido = item3.S_APELLIDO;
                                    if (!item3.DIRECCION.Equals(""))
                                    {
                                        hojavida.direccion_manizales = item3.DIRECCION;

                                    }
                                    else
                                    {
                                        hojavida.direccion_manizales = ".";
                                    }
                                    hojavida.num_celular = 3000000000;
                                    hojavida.municipio_procedencia = ".";

                                    hojavida.num_telefono = item3.TELEFONO;

                                    if (!item3.FECHANAC.Equals(""))
                                    {

                                        DateTime myDate = DateTime.ParseExact(item3.FECHANAC, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                        hojavida.fecha_nacimiento = myDate;
                                    }
                                    else
                                    {
                                        hojavida.fecha_nacimiento = SqlDateTime.MinValue.Value;
                                    }

                                    if (!item3.EMAIL.Equals(""))
                                    {
                                        hojavida.correo = item3.EMAIL;

                                    }
                                    else
                                    {
                                        hojavida.correo = item3.NOMBRE + item3.P_APELLIDO + item3.S_APELLIDO + "@ucaldas.edu.co";

                                    }


                                    try
                                    {
                                        db.HojaVidas.Add(hojavida);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }
                                    iffam = db.HojaVidas.Max(p => p.hojaVidaId);

                                    docente.hojaVidaId = iffam;
                                    Boolean estado = false;
                                    int iddept = 3;
                                    foreach (var item4 in departamentos)
                                    {

                                        if (item3.NOM_DEPTO.Equals(item4.nombre.ToUpper()))
                                        {
                                            estado = true;
                                            iddept = item4.DepartamentoSaludId;
                                        }

                                    }

                                    docente.DepartamentoSaludId = iddept;

                                    try
                                    {

                                        db.Docentes.Add(docente);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }


                                    if (hojavida.Docente != null)
                                        hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";

                                    InsertaVacunas(iffam);

                                }
                            }
                        }


                    }




                }

            }



        }

        public void importaDocentestest()
        {

            List<DepartamentoSalud> departamentos = db.DepartamentoSaluds.ToList();
            //foreach (var item in departamentos)
            //{
                ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();
                string json;

                try
                {
                    json = ser.getProfesoresActivos("G9K");
                }

                catch (Exception e)
                {
                    json = null;
                }
                if (json != null)
                {
                    MvcApplication2.Models.Profesor.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Profesor.ESObject0>(json);
                    foreach (var item2 in profesoresActivos.profesoresActivos)
                    {
                        string json2;
                        try
                        {
                            json2 = ser.getDatosProfesor(item2.CEDULA);
                        }

                        catch (Exception e)
                        {
                            json2 = null;
                        }
                        if (json2 != null)
                        {
                            MvcApplication2.Models.DocenteWS.ESObject0 profesores = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.DocenteWS.ESObject0>(json2);
                            foreach (var item3 in profesores.datosProfesor)
                            {


                                string imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + item3.CEDULA + ".jpg";

                                var hv = db.HojaVidas.Where(r => r.imagen_DI.Equals(imagen_DI));

                                List<HojaVida> hvs = hv.ToList();
                                if (hvs.Count > 0)
                                {

                                    HojaVida hvida = hvs.ElementAt(0);
                                    var docentes = db.Docentes.Where(r => r.hojaVidaId == hvida.hojaVidaId);
                                    Docente docente = null;
                                    try
                                    {
                                        List<Docente> sts = docentes.ToList();
                                        docente = sts.ElementAt(0);
                                        docente.titulo_pregrado = item3.CHIN_TITULO;
                                        docente.maximo_nivel_formacion = item3.CNIA_DESCRIPCION;
                                        docente.dedicacion = item3.CTUR_DESCRIPCION;
                                    }
                                    catch (Exception e)
                                    {

                                    }



                                    db.Entry(docente).State = EntityState.Modified;
                                    try
                                    {

                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }

                                }
                                else
                                {


                                    InsertaFamilia();
                                    var iffam = db.Familias.Max(p => p.familiaId);
                                    HojaVida hojavida = new HojaVida();
                                    hojavida.familiaId = iffam;


                                    Docente docente = new Docente();
                                    docente.tipo_documento = "CC";
                                    docente.num_documento = item3.CEDULA;
                                    if (!item3.LIBREMIL.Equals(""))
                                    {
                                        docente.num_libreta_militar = item3.LIBREMIL;
                                    }

                                    docente.clave = item3.CEDULA;
                                    docente.titulo_pregrado = item3.CHIN_TITULO;
                                    docente.maximo_nivel_formacion = item3.CNIA_DESCRIPCION;
                                    docente.dedicacion = item3.CTUR_DESCRIPCION;

                                    hojavida.primer_nombre = item3.NOMBRE;
                                    hojavida.primer_apellido = item3.P_APELLIDO;
                                    hojavida.segundo_apellido = item3.S_APELLIDO;
                                    if (!item3.DIRECCION.Equals(""))
                                    {
                                        hojavida.direccion_manizales = item3.DIRECCION;

                                    }
                                    else
                                    {
                                        hojavida.direccion_manizales = ".";
                                    }
                                    hojavida.num_celular = 3000000000;
                                    hojavida.municipio_procedencia = ".";

                                    hojavida.num_telefono = item3.TELEFONO;

                                    if (!item3.FECHANAC.Equals(""))
                                    {

                                        DateTime myDate = DateTime.ParseExact(item3.FECHANAC, "dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                        hojavida.fecha_nacimiento = myDate;
                                    }
                                    else
                                    {
                                        hojavida.fecha_nacimiento = SqlDateTime.MinValue.Value;
                                    }

                                    if (!item3.EMAIL.Equals(""))
                                    {
                                        hojavida.correo = item3.EMAIL;

                                    }
                                    else
                                    {
                                        hojavida.correo = item3.NOMBRE + item3.P_APELLIDO + item3.S_APELLIDO + "@ucaldas.edu.co";

                                    }


                                    try
                                    {
                                        db.HojaVidas.Add(hojavida);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }
                                    iffam = db.HojaVidas.Max(p => p.hojaVidaId);

                                    docente.hojaVidaId = iffam;
                                    Boolean estado = false;
                                    int iddept = 3;
                                    foreach (var item4 in departamentos)
                                    {

                                        if (item3.NOM_DEPTO.Equals(item4.nombre.ToUpper()))
                                        {
                                            estado = true;
                                            iddept = item4.DepartamentoSaludId;
                                        }

                                    }

                                    docente.DepartamentoSaludId = iddept;

                                    try
                                    {

                                        db.Docentes.Add(docente);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }


                                    if (hojavida.Docente != null)
                                        hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotosp/" + hojavida.Docente.ElementAt(0).num_documento + ".jpg";

                                    InsertaVacunas(iffam);

                                }
                            }
                        }


                    }




                }

            //}



        }

        private void InsertaVacunas(int iffam)
        {
            Vacuna vacuna = new Vacuna();
            vacuna.hojaVidaId = iffam;
            vacuna.lote = ".";

            try
            {
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

            }
        }

        public void importaEstudiantes()
        {

            List<Programa> programas = db.Programas.ToList();
            foreach (var item in programas)
            {
                ServiceReference2.WSFacultadSaludSoapClient ser = new ServiceReference2.WSFacultadSaludSoapClient();

                string json = ser.getEstudiantesMatriculados(item.codigo);
                if (json != null)
                {


                    MvcApplication2.Models.Estudiante2.ESObject0 profesoresActivos = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.Estudiante2.ESObject0>(json);
                    foreach (var item2 in profesoresActivos.estudiantesMatriculados)
                    {
                        string json2;
                        try
                        {
                            json2 = ser.getDatosEstudiante(item2.NUM_DOC);

                        }
                        catch (Exception e)
                        {
                            json2 = null;
                        }
                        if (json2 != null)
                        {
                            MvcApplication2.Models.EstudianteWS.ESObject0 profesores = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<MvcApplication2.Models.EstudianteWS.ESObject0>(json2);
                            foreach (var item3 in profesores.datosEstudiante)
                            {
                                string imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + item3.CODIGO + ".jpg";

                                var hv = db.HojaVidas.Where(r => r.imagen_DI.Equals(imagen_DI));
                                List<HojaVida> hvs = hv.ToList();
                                if (hvs.Count > 0)
                                {
                                    //   Estudiante estudiante = new Estudiante();
                                    HojaVida hvida = hvs.ElementAt(0);
                                    var estudiantes = db.Estudiantes.Where(r => r.hojaVidaId == hvida.hojaVidaId);
                                    Estudiante estudiante = null;
                                    try
                                    {
                                        List<Estudiante> sts = estudiantes.ToList();
                                        estudiante = sts.ElementAt(0);

                                    }
                                    catch (Exception e)
                                    {

                                    }

                                    estudiante.estado_academico = item3.NOM_ESTADO;

                                    short s;
                                    short.TryParse(item3.SEMESTRE, out s);
                                    estudiante.semestre = s;




                                    db.Entry(estudiante).State = EntityState.Modified;
                                    try
                                    {

                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }



                                }
                                else
                                {
                                    try
                                    {

                                        InsertaFamilia();

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
                                    var iffam = db.Familias.Max(p => p.familiaId);
                                    HojaVida hojavida = new HojaVida();
                                    hojavida.familiaId = iffam;


                                    Estudiante estudiante = new Estudiante();
                                    estudiante.num_documento = item3.NUM_DOC;
                                    estudiante.codigo = Int64.Parse(item3.CODIGO);
                                    estudiante.tipo_documento = item3.NOM_DOC;

                                    estudiante.modalidad = item3.MODALIDAD;
                                    estudiante.clave = item3.NUM_DOC;



                                    estudiante.estado_academico = item3.NOM_ESTADO;

                                    hojavida.primer_nombre = item3.NOMBRE;
                                    hojavida.primer_apellido = item3.P_APELLIDO;
                                    hojavida.segundo_apellido = item3.S_APELLIDO;
                                    estudiante.direccion_procedencia = item3.DIR_CORREO;
                                    hojavida.direccion_manizales = item3.DIR_CORREO;
                                    hojavida.num_celular = 3000000000;
                                    if (hojavida.direccion_manizales.Equals(String.Empty))
                                    {
                                        hojavida.direccion_manizales = ".";

                                    }
                                    hojavida.genero = item3.SEXO;
                                    hojavida.municipio_procedencia = item3.MUN_PROC;
                                    if (hojavida.municipio_procedencia.Equals(String.Empty))
                                    {
                                        hojavida.municipio_procedencia = ".";

                                    }
                                    short s;
                                    short.TryParse(item3.SEMESTRE, out s);
                                    estudiante.semestre = s;
                                    hojavida.num_telefono = item3.TEL_CORREO;

                                    if (!item3.FECHA_NACIMIENTO.Equals("") && !item3.FECHA_NACIMIENTO.Equals("//"))
                                    {

                                        DateTime myDate = DateTime.ParseExact(item3.FECHA_NACIMIENTO, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        hojavida.fecha_nacimiento = myDate;
                                    }
                                    else
                                    {
                                        hojavida.fecha_nacimiento = SqlDateTime.MinValue.Value;
                                    }


                                    hojavida.correo = item3.EMAIL;
                                    try
                                    {
                                        hojavida.imagen_DI = "http://acad.ucaldas.edu.co/fotos/" + estudiante.codigo + ".jpg";

                                        db.HojaVidas.Add(hojavida);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    }



                                    iffam = db.HojaVidas.Max(p => p.hojaVidaId);

                                    estudiante.hojaVidaId = iffam;


                                    estudiante.programaId = item.programaId;

                                    try
                                    {

                                        db.Estudiantes.Add(estudiante);
                                        db.SaveChanges();
                                    }
                                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                                    {
                                        Console.WriteLine(e.Data);
                                    };

                                    // hojavida = getSalud(hojavida);
                                    InsertaVacunas(iffam);
                                }
                            }
                        }


                    }

                }

            }
        }

        private void InsertaFamilia()
        {
            Familia familia = new Familia();
            familia.primer_nombre_padre = "";
            familia.segundo_nombre_padre = "";
            familia.primer_apellido_padre = "";
            familia.segundo_apellido_madre = "";
            familia.telefono_padre = 3000000000;
            familia.primer_nombre_madre = "";
            familia.segundo_nombre_madre = "";
            familia.primer_apellido_madre = "";
            familia.segundo_apellido_madre = "";
            familia.telefono_madre = 3000000000;
            familia.primer_nombre_acudiente = ".";
            familia.segundo_nombre_acudiente = ".";
            familia.primer_apellido_acudiente = ".";
            familia.segundo_apellido_acudiente = ".";
            familia.telefono_acudiente = 3000000000;
            familia.celular_acudiente = 3000000000;
            familia.direccion_acudiente = ".";
            familia.direccion_madre = "";
            familia.direccion_padre = "";
            try
            {
                db.Familias.Add(familia);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                Console.WriteLine(e.Data);
            }


        }





















    }

}
