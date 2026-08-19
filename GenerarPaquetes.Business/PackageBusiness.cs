using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GenerarPaquetes.Business.Implementations;
using GenerarPaquetes.Business.Interfaces;
using GenerarPaquetes.Business.Mappers;
using GenerarPaquetes.Entities;
using GenerarPaquetes.Entities.DTOs;

namespace GenerarPaquetes.Business
{
    public class PackageBusiness
    {
        private readonly IFileSystemBusiness _fileSystem;
        private readonly ITfsMockBusiness _tfsMock;
        private string rutaDocs = "C:\\Users\\DAY-V\\Desktop\\Estructura_Completa_PruebasUAT\\GenerarPaqUAT";
        private string rutaTfs = "C:\\Users\\DAY-V\\Desktop\\Estructura_Completa_PruebasUAT\\TFS_Mock";
        public Action<string>? EnviarLog { get; set; }
        public PackageBusiness()
        {
            _fileSystem = new FileSystemBusiness();
            _tfsMock = new TfsMockBusiness();
        }
        public void GenerarPaquete(PaqueteUatRequestDto paquete)
        {
            if (string.IsNullOrEmpty(paquete.BuildNumber))
            {
                throw new Exception("El numero del build no puede estar vacio");
            }
            if (string.IsNullOrEmpty(paquete.DestinationPath))
            {
                throw new Exception("La ruta de destino no puede estar vacia");
            }
            if (paquete.SelectedApps == null || paquete.SelectedApps.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos una aplicacion");
            }

            //Crear la carpeta raiz

            _fileSystem.CrearDirectorio(paquete.DestinationPath);

            EmitirLog("==================================================");
            EmitirLog("INICIANDO PROCESO DE GENERACIÓN DE PAQUETE");
            EmitirLog("Build TFS: " + paquete.BuildNumber);
            EmitirLog("Destino: " + paquete.DestinationPath);
            EmitirLog("==================================================");
            ProcesarArchivosBase(paquete.DestinationPath, paquete.SelectedApps);
            List<string> modulosReales = ProcesarModulos(paquete);
            EmitirLog("==================================================");
            EmitirLog("Paquete UAT generado exitosamente.");
            EmitirLog("==================================================");

        }

        private void ProcesarArchivosBase(String rutaDestino, List<string> appsSeleccionadas)
        {
            string rutaExcelOrigen = Path.Combine(rutaDocs, "Q-MexFile.xlsx");
            string rutaExcelDestino = Path.Combine(rutaDestino, "Declaracion de Indisponibilidad de URLs Q3_Q42026 - MEX.xlsx");

            if (_fileSystem.ExisteArchivo(rutaExcelOrigen))
            {
                _fileSystem.CopiarArchivo(rutaExcelOrigen, rutaExcelDestino);
                EmitirLog("Archivo Excel Q-Mex COPIADO!");

            }

            string rutaTxtOrigen = Path.Combine(rutaDocs, "InstruccionesLiberacion.txt");
            string rutaTxtDestino = Path.Combine(rutaDestino, "InstruccionesLiberacion.txt");

            if (_fileSystem.ExisteArchivo(rutaTxtOrigen))
            {
                string[] lineasOriginales = _fileSystem.LeerLineasDeArchivo(rutaTxtOrigen);
                List<string> lineasNuevas = new List<string>();

                bool requiereSnapshot = EsModuloApiOFiscal(appsSeleccionadas);

                foreach (string linea in lineasOriginales)
                {
                    string lineaModificada = linea;

                    if (lineaModificada.Contains("step1"))
                    {
                        lineaModificada = requiereSnapshot ? "Generar SnapShot del Generar SnapShot del servidor 10.110.10.175" : "Cada Runbook contiene su paso de respaldo";
                    }

                    if (lineaModificada.Contains("destino"))
                    {
                        lineaModificada = lineaModificada.Replace("destino", rutaDestino);
                    }

                    if (lineaModificada.Contains("step2"))
                    {
                        lineaModificada = requiereSnapshot ? "Plan de reversion,restaurar snapshot del servidor 10.110.10.175" : "Plan de reversion, restaurar el respaldo";
                    }

                    lineasNuevas.Add(lineaModificada);
                }
                _fileSystem.EscribirLineasDeArchivo(rutaTxtDestino, lineasNuevas.ToArray());
                EmitirLog("InstruccionesLiberacion.txt generado y personalizado.");
            }
        }
        private List<string> ProcesarModulos(PaqueteUatRequestDto paquete)
        {
            List<string> listaModulos = new List<string>();
            string rutaRunbooks = Path.Combine(rutaDocs, "Runbooks");
            string rutaUat = Path.Combine(rutaDocs, "UAT");

            foreach (string app in paquete.SelectedApps)
            {
                string nombreReal = AliasMapper.ResolverAlias(app);
                listaModulos.Add(nombreReal);

                EmitirLog("Procesando modulo: " + app + " -> " + nombreReal);

                if (nombreReal == "BD" || nombreReal == "BD_MAS")
                {
                    _fileSystem.CrearDirectorio(Path.Combine(paquete.DestinationPath, "DB", "Scripts"));
                    _fileSystem.CrearDirectorio(Path.Combine(paquete.DestinationPath, "DB", "StoredProcedures"));
                    EmitirLog("Estructura de la base de datos creada (Scripts / StoredProcedures). ");
                }

                //Copiar los runbooks
                if (_fileSystem.ExisteDirectorio(rutaRunbooks))
                {
                    string[] runbooks = _fileSystem.ObtenerArchivosDelDirectorio(rutaRunbooks, "*" + nombreReal + "*.doc");

                    foreach (string archivo in runbooks)
                    {
                        string nombreDoc = Path.GetFileName(archivo);
                        _fileSystem.CopiarArchivo(archivo, Path.Combine(paquete.DestinationPath, nombreDoc));
                        EmitirLog("Runbook copoiado: " + nombreDoc);
                    }
                }
                //Copiar carpetas de UAT

                if (_fileSystem.ExisteDirectorio(rutaUat))
                {
                    string[] carpetasUat = _fileSystem.ObtenerArchivosDelDirectorio(rutaUat, "*" + nombreReal + "*");
                    foreach(string carpeta in carpetasUat)
                    {
                        string nombreCarpeta = Path.GetFileName(carpeta);
                        _fileSystem.CopiarDirectorioRecursivo(carpeta, Path.Combine(paquete.DestinationPath, nombreCarpeta));
                        EmitirLog("Carpeta UAR Copiada: " + nombreCarpeta);
                    }
                }
            }
            return listaModulos;
        }

        private ITfsMockBusiness Get_tfsMock()
        {
            return _tfsMock;
        }

        private void ProcesarCompiladosTfs(String buildNumber, string destino, List<string> modulos, ITfsMockBusiness _tfsMock)
        {
            if(!_tfsMock.ExisteBuild(rutaTfs, buildNumber))
            {
                EmitirLog("AVISO: No se encontro la carpeta del build en el TFS");
                return;
            }
            EmitirLog("--------------------------------------------------");
            string rutaBuild = Path.Combine(rutaTfs, buildNumber);

            foreach(string modulo in modulos)
            {
                List<string> compilados = _tfsMock.BuscarCompilados(rutaBuild, modulo);
                foreach(String carpetaCompilado in compilados)
                {
                    string nombreCompilado = Path.GetFileName(carpetaCompilado);
                    _fileSystem.CopiarDirectorioRecursivo(carpetaCompilado, Path.Combine(destino, nombreCompilado));
                    EmitirLog("Compilado copiado desde TFS: " + nombreCompilado);
                }
            }
        }
        private bool EsModuloApiOFiscal(List<string> apps)
        {
            foreach(string app in apps)
            {
                string txt = app.ToLower();
                if (txt.Contains("fiscal") || txt.Contains("apirest"))
                {
                    return true;
                }
            }
            return false;
        }
        private void EmitirLog(string msj)
        {
            if(EnviarLog != null)
            {
                EnviarLog(msj);
            }
        }
    }
}

