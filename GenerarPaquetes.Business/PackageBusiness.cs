using System;
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

                // 1. Asegurar carpeta de destino
                _dao.CrearDirectorio(request.DestinationPath);

                // 2. Copiar documentos base
                CopiarDocumentosBase(rutaDocs, request.DestinationPath);

                // 3. Procesar aplicativos
                foreach (var app in request.SelectedApps)
                {
                    ProcesarModulo(app, request.BuildNumber, rutaTfs, request.DestinationPath);
                }

                EnviarLog?.Invoke("--- PROCESO TERMINADO CON ÉXITO ---");
            }
            catch (Exception ex)
            {
                EnviarLog?.Invoke($"[ERROR CRÍTICO]: {ex.Message}");
                throw;
            }
        }

        private void CopiarDocumentosBase(string rutaDocs, string rutaDestino)
        {
            // Archivo Excel Q-Mex
            string excelOrigen = Path.Combine(rutaDocs, "Q-MexFile.xlsx");
            string excelDestino = Path.Combine(rutaDestino, "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx");

            if (_dao.ExisteArchivo(excelOrigen))
            {
                _dao.CopiarArchivo(excelOrigen, excelDestino, true);
                EnviarLog?.Invoke("[OK] Excel Q-Mex copiado.");
            }
            else
            {
                EnviarLog?.Invoke($"[AVISO] No se encontró: {excelOrigen}");
            }

            // Archivo de Instrucciones (si existe en la carpeta base)
            string instOrigen = Path.Combine(rutaDocs, "InstruccionesLiberacion.txt");
            string instDestino = Path.Combine(rutaDestino, "InstruccionesLiberacion.txt");

            if (_dao.ExisteArchivo(instOrigen))
            {
                _dao.CopiarArchivo(instOrigen, instDestino, true);
                EnviarLog?.Invoke("[OK] InstruccionesLiberacion.txt copiado.");
            }
        }

        private void ProcesarModulo(string app, string build, string rutaTfs, string rutaDestino)
        {
            string carpetaOrigenApp = Path.Combine(rutaTfs, build, app);
            string carpetaDestinoApp = Path.Combine(rutaDestino, app);

            if (_dao.ExisteDirectorio(carpetaOrigenApp))
            {
                _dao.CopiarDirectorioRecursivo(carpetaOrigenApp, carpetaDestinoApp);
                EnviarLog?.Invoke($"[OK] Aplicativo copiado con éxito: {app}");
            }
            else
            {
                EnviarLog?.Invoke($"[OMITIDO] No existe la ruta de origen: {carpetaOrigenApp}");
            }
        }
    }
}