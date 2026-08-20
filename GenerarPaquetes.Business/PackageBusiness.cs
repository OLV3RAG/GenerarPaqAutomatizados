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
                EnviarLog?.Invoke("--- INICIANDO GENERACIÓN DE PAQUETE ---");

                string rutaDocs = _dao.ObtenerRutaDocumentos();
                string rutaTfs = _dao.ObtenerRutaTFS();

                // 1. Crear carpeta destino si no existe
                _dao.CrearDirectorio(request.DestinationPath);

                // 2. Copiar Excel Q-Mex únicamente
                string excelOrigen = Path.Combine(rutaDocs, "Q-MexFile.xlsx");
                string excelDestino = Path.Combine(request.DestinationPath, "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx");

                if (_dao.ExisteArchivo(excelOrigen))
                {
                    _dao.CopiarArchivo(excelOrigen, excelDestino, true);
                    EnviarLog?.Invoke("[OK] Excel Q-Mex copiado.");
                }

                // 3. Procesar y reemplazar archivo de Instrucciones
                ProcesarInstrucciones(rutaDocs, request);

                // 4. Copiar aplicativos seleccionados
                foreach (var app in request.SelectedApps)
                {
                    string carpetaOrigenApp = Path.Combine(rutaTfs, request.BuildNumber, app);
                    string carpetaDestinoApp = Path.Combine(request.DestinationPath, app);

                    if (_dao.ExisteDirectorio(carpetaOrigenApp))
                    {
                        _dao.CopiarDirectorioRecursivo(carpetaOrigenApp, carpetaDestinoApp);
                        EnviarLog?.Invoke($"[OK] Aplicativo copiado: {app}");
                    }
                    else
                    {
                        EnviarLog?.Invoke($"[OMITIDO] No existe carpeta: {carpetaOrigenApp}");
                    }
                }

                EnviarLog?.Invoke("--- PROCESO TERMINADO CON ÉXITO ---");
            }
            catch (Exception ex)
            {
                EnviarLog?.Invoke($"[ERROR CRÍTICO]: {ex.Message}");
                throw;
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

            // 1. Detectar si se seleccionó WebAPI / API
            bool contieneApi = request.SelectedApps.Exists(a =>
                a.IndexOf("WebAPI", StringComparison.OrdinalIgnoreCase) >= 0 ||
                a.IndexOf("API", StringComparison.OrdinalIgnoreCase) >= 0);

            if (contieneApi)
            {
                listaStep1.Add("Generar SnapShot del servidor 10.110.10.175");
                listaStep2.Add("Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1");
            }

            // 2. Detectar si se seleccionó SIAP / Runbooks
            bool contieneSiap = request.SelectedApps.Exists(a =>
                a.IndexOf("SIAP", StringComparison.OrdinalIgnoreCase) >= 0);

            if (contieneSiap)
            {
                listaStep1.Add("Cada Runbook contiene su paso de respaldo");
                listaStep2.Add("Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook");
            }

            // 3. Formatear el contenido de los pasos
            string resultadoStep1 = listaStep1.Count > 0
                ? string.Join(Environment.NewLine + "               ", listaStep1)
                : "N/A";

            string resultadoStep2 = listaStep2.Count > 0
                ? string.Join(Environment.NewLine + "                   ", listaStep2)
                : "N/A";

            // 4. Reemplazar las palabras clave exactas del archivo .txt
            texto = texto.Replace("destino", request.DestinationPath);
            texto = texto.Replace("step1", resultadoStep1);
            texto = texto.Replace("step2", resultadoStep2);

            // 5. Guardar en destino
            _dao.EscribirTexto(destino, texto);
            EnviarLog?.Invoke("[OK] InstruccionesLiberacion.txt generado con las descripciones técnicas.");
        }
    }
}