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
                EnviarLog?.Invoke($"Destino: {request.DestinationPath}");

                // 2. Copiar Excel Q-Mex
                CopiarExcelQMex(rutaDocs, request.DestinationPath);

                // 3. Copiar Runbooks completos a la carpeta destino
                CopiarCarpetaRunbooks(rutaRunbooks, request.DestinationPath);

                // 4. Generar y personalizar InstruccionesLiberacion.txt
                ProcesarInstrucciones(rutaDocs, request);

                // 5. Copiar los aplicativos seleccionados desde la carpeta UAT
                foreach (var app in request.SelectedApps)
                {
                    CopiarModuloAplicativo(app, request.BuildNumber, rutaUatApps, request.DestinationPath);
                }

                EnviarLog?.Invoke("=== PROCESO FINALIZADO CON ÉXITO ===");
            }
            catch (Exception ex)
            {
                EnviarLog?.Invoke($"[ERROR CRÍTICO]: {ex.Message}");
                throw;
            }
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

        private void CopiarCarpetaRunbooks(string rutaRunbooks, string rutaDestino)
        {
            if (_dao.ExisteDirectorio(rutaRunbooks))
            {
                string destinoRunbooks = Path.Combine(rutaDestino, "Runbooks");
                _dao.CopiarDirectorioRecursivo(rutaRunbooks, destinoRunbooks);
                EnviarLog?.Invoke("[OK] Carpeta Runbooks copiada exitosamente.");
            }
            else
            {
                EnviarLog?.Invoke($"[AVISO] No se encontró la carpeta de Runbooks en: {rutaRunbooks}");
            }
        }

        private void CopiarModuloAplicativo(string app, string build, string rutaBaseUat, string rutaDestino)
        {
            // Opciones de estructura comunes:
            // 1. UAT \ Build \ App
            string rutaOpcion1 = Path.Combine(rutaBaseUat, build, app);
            // 2. UAT \ App \ Build
            string rutaOpcion2 = Path.Combine(rutaBaseUat, app, build);
            // 3. UAT \ App (directa)
            string rutaOpcion3 = Path.Combine(rutaBaseUat, app);

            string rutaOrigenFinal = string.Empty;

            if (_dao.ExisteDirectorio(rutaOpcion1))
                rutaOrigenFinal = rutaOpcion1;
            else if (_dao.ExisteDirectorio(rutaOpcion2))
                rutaOrigenFinal = rutaOpcion2;
            else if (_dao.ExisteDirectorio(rutaOpcion3))
                rutaOrigenFinal = rutaOpcion3;

            if (!string.IsNullOrEmpty(rutaOrigenFinal))
            {
                string carpetaDestinoApp = Path.Combine(rutaDestino, app);
                _dao.CopiarDirectorioRecursivo(rutaOrigenFinal, carpetaDestinoApp);
                EnviarLog?.Invoke($"[OK] {app} copiado correctamente desde: {rutaOrigenFinal}");
            }
            else
            {
                EnviarLog?.Invoke($"[OMITIDO] No se encontró carpeta para '{app}'. (Ruta evaluada: {rutaOpcion1})");
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

            bool contieneApi = request.SelectedApps.Exists(a =>
                a.IndexOf("WebAPI", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("API", StringComparison.OrdinalIgnoreCase) >= 0);

            if (contieneApi)
            {
                listaStep1.Add("Generar SnapShot del servidor 10.110.10.175");
                listaStep2.Add("Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1");
            }

            bool contieneSiap = request.SelectedApps.Exists(a =>
                a.IndexOf("SIAP", StringComparison.OrdinalIgnoreCase) >= 0);

            if (contieneSiap)
            {
                listaStep1.Add("Cada Runbook contiene su paso de respaldo");
                listaStep2.Add("Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook");
            }

            string resultadoStep1 = listaStep1.Count > 0
                ? string.Join(Environment.NewLine + "               ", listaStep1)
                : "N/A";

            string resultadoStep2 = listaStep2.Count > 0
                ? string.Join(Environment.NewLine + "                   ", listaStep2)
                : "N/A";

            texto = texto.Replace("destino", request.DestinationPath);
            texto = texto.Replace("step1", resultadoStep1);
            texto = texto.Replace("step2", resultadoStep2);

            _dao.EscribirTexto(destino, texto);
            EnviarLog?.Invoke("[OK] InstruccionesLiberacion.txt generado y reemplazado.");
        }
    }
}