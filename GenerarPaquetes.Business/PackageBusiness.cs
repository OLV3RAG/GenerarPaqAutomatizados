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
                EnviarLog?.Invoke("Iniciando validaciones...");

                // 1. Obtener rutas base desde el DAO
                string rutaBaseDocs = _dao.ObtenerRutaDocumentos();
                string rutaBaseTFS = _dao.ObtenerRutaTFS();

                // 2. Validar o crear la carpeta de destino
                if (!_dao.ExisteDirectorio(request.DestinationPath))
                {
                    _dao.CrearDirectorio(request.DestinationPath);
                    EnviarLog?.Invoke($"Carpeta de destino creada: {request.DestinationPath}");
                }

                // 3. Copiar documentos base (Excel, Instrucciones, etc.)
                CopiarDocumentosBase(rutaBaseDocs, request.DestinationPath);

                // 4. Procesar cada módulo/aplicativo seleccionado
                foreach (var app in request.SelectedApps)
                {
                    ProcesarModulo(app, request.BuildNumber, rutaBaseTFS, request.DestinationPath);
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
            string archivoExcelOrigen = Path.Combine(rutaDocs, "Q-MexFile.xlsx");
            string archivoExcelDestino = Path.Combine(rutaDestino, "Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx");

            if (_dao.ExisteArchivo(archivoExcelOrigen))
            {
                _dao.CopiarArchivo(archivoExcelOrigen, archivoExcelDestino, true);
                EnviarLog?.Invoke("Archivo Excel copiado correctamente.");
            }
            else
            {
                EnviarLog?.Invoke("[AVISO] No se encontró la plantilla de Excel en el origen.");
            }
        }

        private void ProcesarModulo(string app, string build, string rutaTfs, string rutaDestino)
        {
            EnviarLog?.Invoke($"Procesando aplicativo: {app}...");

            string carpetaOrigenBuild = Path.Combine(rutaTfs, build, app);
            string carpetaDestinoApp = Path.Combine(rutaDestino, app);

            if (_dao.ExisteDirectorio(carpetaOrigenBuild))
            {
                _dao.CrearDirectorio(carpetaDestinoApp);

                string[] archivos = _dao.ObtenerArchivos(carpetaOrigenBuild, "*.*");
                foreach (var archivo in archivos)
                {
                    string nombreArchivo = Path.GetFileName(archivo);
                    string destinoFinal = Path.Combine(carpetaDestinoApp, nombreArchivo);

                    _dao.CopiarArchivo(archivo, destinoFinal, true);
                }

                EnviarLog?.Invoke($"[OK] {app} copiado correctamente ({archivos.Length} archivos).");
            }
            else
            {
                EnviarLog?.Invoke($"[OMITIDO] No se encontró la carpeta para {app} en la Build {build}.");
            }
        }
    }
}