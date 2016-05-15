using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Util
{
    public class Constantes
    {
        public static string url_folder="http://salud.ucaldas.edu.co/Proyecto/Uploads";

        public static string url_noimage = "http://www.logan.es/wp-content/themes/logan/images/dummy-image.jpg";

        public static string[] documentos_docente = { "doc_identidad", "acta_grado", "dip_prof", "acta_grado_post", "dip_espe", "tpd", "tpn", "cv1", "cv2", "ant_varicela", "ant_hp" };

        public static string[] documentos_estudiante = { "doc_identidad", "carne_LS", "carne_estudiantil", "carne_EPS", "EV1", "EV2", "ant_varicela", "ant_hepatitisB", "influenza" };


        public static string[] documentos_estudianteResidentes = { "doc_identidad", "carne_LS", "carne_estudiantil", "carne_EPS", "carne_ARL", "EV1", "EV2", "ant_varicela", "ant_hepatitisB", "dip_pre", "tp", "acta_grado", "rcp_basico", "rcp_avanzado" };

    }
}