using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GenerarPaquetes.DAO;
using GenerarPaquetes.Entities.DTOs;

namespace GenerarPaquetes.Business
{
    public class PackageBusiness
    {
        private readonly PackageDao _dao;
        public Action<string>? EnviarLog { get; set; }

        public PackageBusiness()
        {
            _dao = new PackageDao();
        }

        private static readonly Dictionary<string, List<string>> MapeoRunbooks = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            // Base de Datos
            { "BD", new List<string> { "00_RunBook_AplicarCambios_BD.doc" } },
            { "BD_MAS", new List<string> { "00_RunBook_AplicarCambios_BD_MAS.doc" } },

            // Aplicativos
            { "IDCMAS", new List<string> { "01_RunBook_AplicarCambios_IDC_MAS.doc" } },
            { "MAS", new List<string> { "01_RunBook_AplicarCambios_IDC_MAS.doc" } },
            { "IDCSIAP", new List<string> { "01_RunBook_AplicarCambiosSIAP.doc" } },
            { "SIAP", new List<string> { "01_RunBook_AplicarCambiosSIAP.doc" } },
            { "CFDI", new List<string> { "01_RunBook_ActualizarPortalCFDI.doc" } },
            { "PortalCFDI", new List<string> { "01_RunBook_ActualizarPortalCFDI.doc" } },
            { "portcfdi", new List<string> { "01_RunBook_ActualizarPortalCFDI.doc" } },
            { "RobotCFD", new List<string> { "01_RunBook_ActualizarPortalCFDI.doc" } },
            { "IDCSIAPApi", new List<string> { "01_RunBook_AplicarCambios_IDC_SIAPApi.doc" } },
            { "BatchLauncher", new List<string> { "01_RunBook_BatchLauncher.doc" } },
            { "BusinessServiceSIAP", new List<string> { "01_RunBook_BusinessServiceSIAP.doc" } },
            { "CatalogsWS", new List<string> { "01_RunBook_CatalogsWS.doc" } },
            { "CloseService", new List<string> { "01_RunBook_Zurich.CloseServices.doc" } },
            { "PortalAPIRest", new List<string> { "01_RunBook_InstalarPortalAPIRest.doc" } },
            { "QuotationWeb", new List<string> { "01_RunBook_Zurich.QuotationWeb.doc" } },
            { "ProcesarMov", new List<string> { "01_RunBook_cw_ProcesarMovimiento_PortalAgentes.doc" } },
            { "PDFiscales", new List<string> { "00_RunBook_AplicarCambiosBasePortalDatosFiscales.doc" } },
            { "MASWeb", new List<string> { "01_RunBook_AplicarCambiosMAS_Web.doc" } },
            { "ServicioIntegracion", new List<string> { "01_RunBook_ServicioIntegracion.doc" } },
            { "ServicioEmision", new List<string> { "01_RunBook_ServicioEmision.doc" } },
            { "ServicioDocumentacion", new List<string> { "01_RunBook_AplicarCambiosServicioDocumentacion.doc" } },
            { "SIRI", new List<string> { "01_RunBook_AplicarCambiosServicioAutomaticLoadSIRI.doc" } },
            { "ServicioIntegracionPermisos", new List<string> { "01_RunBook_ServicioIntegracion - ConPermisos a TempMasivo.doc" } },
            { "ServicioMAS", new List<string> { "01_RunBook_AplicarCambiosMAS_Servicios.doc" } },
            { "WSCLPortAgt", new List<string> { "01_RunBook_WS_CL_Portal_Agentes.doc" } },
            { "DTS", new List<string> { "01_RunBook_AplicarCambiosServicioDTSMiscelaneosService.doc" } },
            { "IAdi", new List<string> { "01_RunBook_AplicarCambiosInterfazAdiSIAP.doc" } },
            { "WSCalcPrimQA", new List<string> { "01_RunBook_Ws_CalculoPrima_QA.doc" } },
            { "CargaQA", new List<string> { "01_RunBook_WS_Carga_QA.doc" } }
        };

        private static readonly Dictionary<string, string[]> MapeoCarpetasFisicas = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "CFDI", new[] { "Robot_CFD", "RobotCFD", "PortalCFDI" } },
            { "PortalCFDI", new[] { "Robot_CFD", "RobotCFD", "PortalCFDI" } },
            { "portcfdi", new[] { "Robot_CFD", "RobotCFD", "PortalCFDI" } },
            { "RobotCFD", new[] { "Robot_CFD", "RobotCFD" } },
            { "IDCMAS", new[] { "IDC_MAS", "IDCMAS" } },
            { "MAS", new[] { "IDC_MAS", "IDCMAS" } },
            { "IDCSIAP", new[] { "IDC_SIAP", "IDCSIAP" } },
            { "SIAP", new[] { "IDC_SIAP", "IDCSIAP" } },
            { "CloseService", new[] { "Zurich.CloseServices", "CloseService" } },
            { "QuotationWeb", new[] { "Zurich.QuotationWeb", "QuotationWeb" } },
            { "ProcesarMov", new[] { "cw_ProcesarMovimiento_PortalAgentes", "ProcesarMov" } },
            { "WSCLPortAgt", new[] { "WS_CL_Portal_Agentes", "WSCLPortAgt" } }
        };

        public void ProcesarPaquete(PaqueteUatRequestDto request)
        {
            try
            {
                request.DestinationPath = NormalizarRuta(request.DestinationPath);

                EnviarLog?.Invoke("=== INICIANDO GENERACIÓN DE PAQUETE UAT ===");

                string rutaDocs = _dao.ObtenerRutaDocumentos();
                string rutaUatApps = _dao.ObtenerRutaTFS();
                string rutaRunbooks = _dao.ObtenerRutaRunbooks();

                _dao.CrearDirectorio(request.DestinationPath);
                EnviarLog?.Invoke($"Destino: {request.DestinationPath}");

                CopiarExcelQMex(rutaDocs, request.DestinationPath);
                ProcesarInstrucciones(rutaDocs, request);

                foreach (var app in request.SelectedApps)
                {
                    EnviarLog?.Invoke($"--- Procesando: {app} ---");

                    // CONDICIÓN ESPECÍFICA BD (SIAP)
                    if (app.Equals("BD", StringComparison.OrdinalIgnoreCase))
                    {
                        CrearEstructuraBaseDatos(request.DestinationPath);
                        CopiarRunbookEspecifico("BD", rutaRunbooks, request.DestinationPath);
                        continue;
                    }

                    // CONDICIÓN ESPECÍFICA BD_MAS
                    if (app.Equals("BD_MAS", StringComparison.OrdinalIgnoreCase))
                    {
                        CrearEstructuraBaseDatos(request.DestinationPath);
                        CopiarRunbookEspecifico("BD_MAS", rutaRunbooks, request.DestinationPath);
                        continue;
                    }

                    // RESTO DE APLICATIVOS
                    CopiarModuloAplicativo(app, request.BuildNumber, rutaUatApps, request.DestinationPath);
                    CopiarRunbookEspecifico(app, rutaRunbooks, request.DestinationPath);
                }

                EnviarLog?.Invoke("=== PROCESO FINALIZADO CON ÉXITO ===");
            }
            catch (Exception ex)
            {
                EnviarLog?.Invoke($"[ERROR CRÍTICO]: {ex.Message}");
                throw;
            }
        }

        private void CrearEstructuraBaseDatos(string rutaDestino)
        {
            string rutaDBScripts = Path.Combine(rutaDestino, "DB", "Scripts");
            string rutaDBSp = Path.Combine(rutaDestino, "DB", "StoredProcedures");
            string rutaDBFunciones = Path.Combine(rutaDestino, "DB", "Funciones");

            _dao.CrearDirectorio(rutaDBScripts);
            _dao.CrearDirectorio(rutaDBSp);
            _dao.CrearDirectorio(rutaDBFunciones);

            string archivoScripts = Path.Combine(rutaDBScripts, "Scripts_1.sql");
            string archivoSp = Path.Combine(rutaDBSp, "StoredProcedures_1.sql");
            string archivoFunciones = Path.Combine(rutaDBFunciones, "Funciones_1.sql");

            _dao.EscribirTexto(archivoScripts, string.Empty);   
            _dao.EscribirTexto(archivoSp, string.Empty);
            _dao.EscribirTexto(archivoFunciones, string.Empty);

            EnviarLog?.Invoke("[OK] Archivos SQL generados correctamente en los respectivos directorios");
            EnviarLog?.Invoke("[OK] Generados directorios: DB\\Scripts y DB\\StoredProcedures");
        }

        private string NormalizarRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return string.Empty;

            return ruta.Trim().Trim('"', '\'').Trim();
        }

        private void CopiarExcelQMex(string rutaDocs, string rutaDestino)
        {
            string excelOrigen = Path.Combine(rutaDocs, "Q-MexFile.xlsx");
            string excelDestino = Path.Combine(rutaDestino, "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx");

            if (_dao.ExisteArchivo(excelOrigen))
            {
                _dao.CopiarArchivo(excelOrigen, excelDestino, true);
                EnviarLog?.Invoke("[OK] Archivo Excel Q-Mex copiado.");
            }
            else
            {
                EnviarLog?.Invoke($"[AVISO] No se encontró Excel en: {excelOrigen}");
            }
        }

        private void CopiarRunbookEspecifico(string app, string rutaRunbooks, string rutaDestino)
        {
            if (!_dao.ExisteDirectorio(rutaRunbooks))
            {
                EnviarLog?.Invoke($"[AVISO] Directorio de Runbooks no existe: {rutaRunbooks}");
                return;
            }

            if (MapeoRunbooks.TryGetValue(app, out List<string> archivosRunbook))
            {
                foreach (string rbNombre in archivosRunbook)
                {
                    string origen = Path.Combine(rutaRunbooks, rbNombre);
                    string destino = Path.Combine(rutaDestino, rbNombre);

                    if (_dao.ExisteArchivo(origen))
                    {
                        _dao.CopiarArchivo(origen, destino, true);
                        EnviarLog?.Invoke($"[OK] Runbook copiado: {rbNombre}");
                    }
                    else
                    {
                        EnviarLog?.Invoke($"[AVISO] No se encontró el Runbook '{rbNombre}' en: {rutaRunbooks}");
                    }
                }
            }
            else
            {
                EnviarLog?.Invoke($"[AVISO] No hay Runbook mapeado para: {app}");
            }
        }

        private void CopiarModuloAplicativo(string app, string build, string rutaBaseUat, string rutaDestino)
        {
            if (!_dao.ExisteDirectorio(rutaBaseUat))
            {
                EnviarLog?.Invoke($"[ERROR] Ruta base UAT no encontrada: {rutaBaseUat}");
                return;
            }

            string rutaOrigenFinal = string.Empty;

            if (MapeoCarpetasFisicas.TryGetValue(app, out string[] posiblesCarpetas))
            {
                foreach (string nombreCarpeta in posiblesCarpetas)
                {
                    string rutaDirecta = Path.Combine(rutaBaseUat, nombreCarpeta);
                    string rutaConBuild = Path.Combine(rutaBaseUat, build, nombreCarpeta);

                    if (_dao.ExisteDirectorio(rutaDirecta))
                    {
                        rutaOrigenFinal = rutaDirecta;
                        break;
                    }
                    if (_dao.ExisteDirectorio(rutaConBuild))
                    {
                        rutaOrigenFinal = rutaConBuild;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(rutaOrigenFinal))
            {
                string r1 = Path.Combine(rutaBaseUat, app);
                string r2 = Path.Combine(rutaBaseUat, build, app);

                if (_dao.ExisteDirectorio(r1)) rutaOrigenFinal = r1;
                else if (_dao.ExisteDirectorio(r2)) rutaOrigenFinal = r2;
            }

            if (string.IsNullOrEmpty(rutaOrigenFinal))
            {
                string[] subdirectorios = _dao.ObtenerDirectorios(rutaBaseUat, $"*{app}*", SearchOption.TopDirectoryOnly);

                if (app.Equals("SIAP", StringComparison.OrdinalIgnoreCase) || app.Equals("IDCSIAP", StringComparison.OrdinalIgnoreCase))
                {
                    subdirectorios = subdirectorios
                        .Where(dir => Path.GetFileName(dir).IndexOf("api", StringComparison.OrdinalIgnoreCase) < 0)
                        .ToArray();
                }

                if (app.Equals("MAS", StringComparison.OrdinalIgnoreCase) || app.Equals("IDCMAS", StringComparison.OrdinalIgnoreCase))
                {
                    subdirectorios = subdirectorios
                        .Where(dir => Path.GetFileName(dir).IndexOf("web", StringComparison.OrdinalIgnoreCase) < 0 &&
                                      Path.GetFileName(dir).IndexOf("servicio", StringComparison.OrdinalIgnoreCase) < 0)
                        .ToArray();
                }

                if (subdirectorios.Length > 0)
                {
                    rutaOrigenFinal = subdirectorios[0];
                }
            }

            if (!string.IsNullOrEmpty(rutaOrigenFinal))
            {
                string nombreCarpetaDestino = Path.GetFileName(rutaOrigenFinal);
                string carpetaDestinoApp = Path.Combine(rutaDestino, nombreCarpetaDestino);

                _dao.CopiarDirectorioRecursivo(rutaOrigenFinal, carpetaDestinoApp);
                EnviarLog?.Invoke($"[OK] Carpeta copiada: {nombreCarpetaDestino}");
            }
            else
            {
                EnviarLog?.Invoke($"[OMITIDO] No se localizó la carpeta física para '{app}' en UAT.");
            }
        }

        private void ProcesarInstrucciones(string rutaDocs, PaqueteUatRequestDto request)
        {
            string origen = Path.Combine(rutaDocs, "InstruccionesLiberacion.txt");
            string destino = Path.Combine(request.DestinationPath, "InstruccionesLiberacion.txt");

            if (!_dao.ExisteArchivo(origen))
            {
                EnviarLog?.Invoke($"[AVISO] No se encontró la plantilla en: {origen}");
                return;
            }

            string descStepOneAPI = "Generar SnapShot del servidor 10.110.10.175";
            string descStepTwoAPI = "Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1";
            string descSIAP = "Cada Runbook contiene su paso de respaldo";
            string descStepTwoSIAP = "Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook";

            bool esPdf = request.SelectedApps.Exists(a =>
                a.IndexOf("DatosFiscales", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("PDFiscales", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("Fiscal", StringComparison.OrdinalIgnoreCase) >= 0);

            bool esApi = request.SelectedApps.Exists(a =>
                a.IndexOf("WebAPI", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("API", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("PortalAPIRest", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("IDCSIAPApi", StringComparison.OrdinalIgnoreCase) >= 0);

            string textoStep1;
            string textoStep2;

            if (esPdf || esApi)
            {
                textoStep1 = descStepOneAPI;
                textoStep2 = descStepTwoAPI;
            }
            else
            {
                textoStep1 = descSIAP;
                textoStep2 = descStepTwoSIAP;
            }

            string contenido = _dao.LeerTexto(origen);

            contenido = contenido.Replace("step1", textoStep1);
            contenido = contenido.Replace("step2", textoStep2);
            contenido = contenido.Replace("destino", request.DestinationPath);

            _dao.EscribirTexto(destino, contenido);
            EnviarLog?.Invoke("[OK] InstruccionesLiberacion.txt generado y reemplazado con éxito.");
        }
    }
}