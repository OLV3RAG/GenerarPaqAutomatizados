using GenerarPaquetes.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerarPaquetes.Business.Helpers;
using GenerarPaquetes.Business.Interfaces;
using System.IO;

namespace GenerarPaquetes.Business.Implementations
{
    public class FileSystemBusiness : IFileSystemBusiness
    {
        public void CrearDirectorio(string ruta) => Directory.CreateDirectory(ruta);
        public bool ExisteArchivo(string ruta) => File.Exists(ruta);
        public bool ExisteDirectorio(string ruta) => Directory.Exists(ruta);
        public void CopiarArchivo(string origen, string destino, bool sobreescribir = true)
        {
            if (File.Exists(origen))
            {
                File.Copy(origen, destino, sobreescribir);
            }
        }
        public void CopiarDirectorioRecursivo(string origen, string destino)
        {
            FileOperationHelper.CopiarEstructuraDelDirectorio(origen, destino);
        }
        public void EscribirLineasDeArchivo(string ruta, string[] lineas) => File.WriteAllLines(ruta, lineas);
        public string[] LeerLineasDeArchivo(string ruta) => File.ReadAllLines(ruta);
        public string[] ObtenerArchivosDelDirectorio(string ruta, string patronBusqueda) => Directory.GetFiles(ruta, patronBusqueda);
        public string[] ObtenerLosSubdirectorios(string ruta, string patronBusqueda, SearchOption op = SearchOption.TopDirectoryOnly) => Directory.GetDirectories(ruta, patronBusqueda, op);
    }
}
