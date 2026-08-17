using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerarPaquetes.Business.Mappers
{
    public class AliasMapper
    {
        public static string ResolverAlias(string modulo)
        {
            if (string.IsNullOrEmpty(modulo))
            {
                return "";
            }
            string texto = modulo.ToLower().Trim();
            switch(texto)
            {
                case "servmas":
                case "serviciosmas":
                    return "ServiciosMAS";

                case "emision":
                case "servemision":
                    return "ServicioEmision";

                case "cfd":
                case "servcfd":
                    return "ServicioCFD";

                case "docu":
                case "sdoc":
                    return "ServicioDocumentacion";

                case "permisos":
                case "perm":
                    return "ServicioIntegracionPermisos";

                case "sinteg":
                case "integracion":
                    return "ServicioIntegracion";

                case "fiscal":
                case "datosf":
                    return "PortalDatosFiscales";

                case "apirest":
                case "rest":
                    return "InstalarPortalAPIRest";

                case "portcfdi":
                case "portalcfdi":
                    return "PortalCFDI";

                case "agentes":
                case "clagentes":
                    return "WS-CLPortalAgentes";

                case "calcprima":
                case "calculoprima":
                    return "WSCalculoPrima";

                case "cargaqa":
                case "wscarga":
                    return "WSCargaQA";

                case "caralogsws":
                case "catalogos":
                    return "CatalogsWS";

                case "servicios_grupo":
                case "servs_grupo":
                    return "Grupo_Servicio";

                case "portales_grupo":
                case "ports_grupo":
                    return "Grupo_Portal";

                case "ws_grupo":
                case "webservices_grupo":
                    return "Grupo_WS";

                case "mas":
                case "sistema":
                    return "MAS";

                case "siap":
                    return "SIAP";

                case "bd":
                    return "BD";

                case "bd_mas":
                    return "BD_MAS";


                default:
                    return modulo.Trim();





            }
        }
    }
}
