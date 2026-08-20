using System;
using System.Collections.Generic;
using System.IO;
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

        public void ProcesarPaquete(PaqueteUatRequestDto request)
        {
            try
            {
                EnviarLog?.Invoke("=== INICIANDO GENERACIÓN DE PAQUETE UAT ===");

                string rutaDocs = _dao.ObtenerRutaDocumentos();
                string rutaUatApps = _dao.ObtenerRutaTFS();
                string rutaRunbooks = _dao.ObtenerRutaRunbooks();

                // 1. Asegurar carpeta destino
                _dao.CrearDirectorio(request.DestinationPath);
                EnviarLog?.Invoke($"Ruta destino: {request.DestinationPath}");

                // 2. Copiar Excel Q-Mex
                CopiarExcelQMex(rutaDocs, request.DestinationPath);

                // 3. Generar InstruccionesLiberacion.txt con las reglas del .bat
                ProcesarInstrucciones(rutaDocs, request);

                // 4. Copiar aplicaciones y sus Runbooks específicos seleccionados
                foreach (var app in request.SelectedApps)
                {
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

        private void CopiarRunbookEspecifico(string app, string rutaRunbooks, string rutaDestino)
        {
            if (!_dao.ExisteDirectorio(rutaRunbooks))
            {
                return;
            }

            // Buscar archivos de Runbook que contengan el nombre del módulo
            string[] runbooksEncontrados = _dao.ObtenerArchivos(rutaRunbooks, $"*{app}*");

            foreach (string rbArchivo in runbooksEncontrados)
            {
                string nombreArchivo = Path.GetFileName(rbArchivo);
                // Se copia directamente a la raíz de rutaDestino
                string destinoFinal = Path.Combine(rutaDestino, nombreArchivo);

                _dao.CopiarArchivo(rbArchivo, destinoFinal, true);
                EnviarLog?.Invoke($"[OK] Runbook copiado: {nombreArchivo}");
            }
        }


        private void CopiarExcelQMex(string rutaDocs, string rutaDestino)
        {
            string excelOrigen = Path.Combine(rutaDocs, "Q-MexFile.xlsx");
            string excelDestino = Path.Combine(rutaDestino, "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx");

            if (_dao.ExisteArchivo(excelOrigen))
            {
                _dao.CopiarArchivo(excelOrigen, excelDestino, true);
                EnviarLog?.Invoke("[OK] Excel Q-Mex copiado.");
            }
            else
            {
                EnviarLog?.Invoke($"[AVISO] No se encontró Excel en: {excelOrigen}");
            }
        }

   
        private void CopiarModuloAplicativo(string app, string build, string rutaBaseUat, string rutaDestino)
        {
            // Búsqueda en todas las posibles ubicaciones dentro de UAT
            string ruta1 = Path.Combine(rutaBaseUat, app);                     // Directo: UAT\BatchLauncher
            string ruta2 = Path.Combine(rutaBaseUat, build, app);              // Con Build: UAT\12345\BatchLauncher
            string ruta3 = Path.Combine(rutaBaseUat, app, build);              // Inverso: UAT\BatchLauncher\12345

            string rutaOrigenFinal = string.Empty;

            if (_dao.ExisteDirectorio(ruta1))
                rutaOrigenFinal = ruta1;
            else if (_dao.ExisteDirectorio(ruta2))
                rutaOrigenFinal = ruta2;
            else if (_dao.ExisteDirectorio(ruta3))
                rutaOrigenFinal = ruta3;

            if (!string.IsNullOrEmpty(rutaOrigenFinal))
            {
                string carpetaDestinoApp = Path.Combine(rutaDestino, app);
                _dao.CopiarDirectorioRecursivo(rutaOrigenFinal, carpetaDestinoApp);
                EnviarLog?.Invoke($"[OK] {app} copiado.");
            }
            else
            {
                EnviarLog?.Invoke($"[OMITIDO] No existe carpeta para '{app}' en: {ruta1}");
            }
        }

        private void ProcesarInstrucciones(string rutaDocs, PaqueteUatRequestDto request)
        {
            string origen = Path.Combine(rutaDocs, "InstruccionesLiberacion.txt");
            string destino = Path.Combine(request.DestinationPath, "InstruccionesLiberacion.txt");

            if (!_dao.ExisteArchivo(origen))
            {
                EnviarLog?.Invoke($"[AVISO] No se encontró la plantilla de instrucciones en: {origen}");
                return;
            }

            string texto = _dao.LeerTexto(origen);

            List<string> listaStep1 = new List<string>();
            List<string> listaStep2 = new List<string>();

            // Regla 1: Si contiene API, WebAPI, PortalAPI o Datos Fiscales
            bool contieneApiOpdf = request.SelectedApps.Exists(a =>
                a.IndexOf("API", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("PDFiscales", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("Fiscal", StringComparison.OrdinalIgnoreCase) >= 0);

            if (contieneApiOpdf)
            {
                listaStep1.Add("Generar SnapShot del servidor 10.110.10.175");
                listaStep2.Add("Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1");
            }

            // Regla 2: Si contiene SIAP o cualquier servicio relacionado
            bool contieneSiap = request.SelectedApps.Exists(a =>
                a.IndexOf("SIAP", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("BServiceSIAP", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("IDCSIAP", StringComparison.OrdinalIgnoreCase) >= 0);

            if (contieneSiap)
            {
                listaStep1.Add("Cada Runbook contiene su paso de respaldo");
                listaStep2.Add("Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook");
            }

            // Si se seleccionaron otros módulos generales que no sean solo API o SIAP
            if (listaStep1.Count == 0 && request.SelectedApps.Count > 0)
            {
                listaStep1.Add($"Respaldar versión productiva actual de los aplicativos: {string.Join(", ", request.SelectedApps)}");
                listaStep2.Add("Plan de reversión, restaurar los respaldos generados en el paso 1");
            }

            string resultadoStep1 = listaStep1.Count > 0
                ? string.Join(Environment.NewLine + "               ", listaStep1)
                : "Sin pasos previos requeridos";

            string resultadoStep2 = listaStep2.Count > 0
                ? string.Join(Environment.NewLine + "                   ", listaStep2)
                : "Sin pasos de reversión requeridos";

            texto = texto.Replace("destino", request.DestinationPath);
            texto = texto.Replace("step1", resultadoStep1);
            texto = texto.Replace("step2", resultadoStep2);

            _dao.EscribirTexto(destino, texto);
            EnviarLog?.Invoke("[OK] InstruccionesLiberacion.txt generado y reemplazado con éxito.");
        }
    }
}